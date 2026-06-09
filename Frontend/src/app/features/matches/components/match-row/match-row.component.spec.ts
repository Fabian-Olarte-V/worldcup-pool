import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { MatchRowComponent } from './match-row.component';
import { MatchEditService } from '../../services/match-edit.service';
import { ResultRowMatchViewModel } from '../../../../shared/components/result-row/result-row.component';

describe('MatchRowComponent', () => {
  let component: MatchRowComponent;
  let fixture: ComponentFixture<MatchRowComponent>;
  let matchEditService: {
    getEditingState$: ReturnType<typeof vi.fn>;
    startEditing: ReturnType<typeof vi.fn>;
    cancelEditing: ReturnType<typeof vi.fn>;
  };

  const match: ResultRowMatchViewModel = {
    id: 'match-1',
    dateLabel: '08 June',
    timeLabel: '18:00',
    homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
    awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
    result: { homeGoals: null, awayGoals: null },
    status: 'Pendiente',
  };

  beforeEach(async () => {
    matchEditService = {
      getEditingState$: vi.fn().mockReturnValue(of(false)),
      startEditing: vi.fn(),
      cancelEditing: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [MatchRowComponent],
      providers: [{ provide: MatchEditService, useValue: matchEditService }],
    }).compileComponents();

    fixture = TestBed.createComponent(MatchRowComponent);
    component = fixture.componentInstance;
    component.match = structuredClone(match);
    fixture.detectChanges();
  });

  it('should update the row, emit payload and close edit mode when saving', () => {
    const emitSpy = vi.spyOn(component.submitMatch, 'emit');

    component.onSaveResult({
      matchId: 'match-1',
      homeGoals: 3,
      awayGoals: 2,
    });

    expect(component.match.result).toEqual({ homeGoals: 3, awayGoals: 2 });
    expect(component.match.status).toBe('Enviado');
    expect(emitSpy).toHaveBeenCalledWith({
      payload: {
        MatchId: 'match-1',
        HomeGoals: 3,
        AwayGoals: 2,
      },
      previousResult: { homeGoals: null, awayGoals: null },
      previousStatus: 'Pendiente',
      match: component.match,
    });
    expect(matchEditService.cancelEditing).toHaveBeenCalledWith('match-1');
  });
});
