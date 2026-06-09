import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ResultRowComponent, ResultRowMatchViewModel } from './result-row.component';

describe('ResultRowComponent', () => {
  let component: ResultRowComponent;
  let fixture: ComponentFixture<ResultRowComponent>;

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
    await TestBed.configureTestingModule({
      imports: [ResultRowComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ResultRowComponent);
    component = fixture.componentInstance;
    component.match = structuredClone(match);
    component.isEditing = true;
    fixture.detectChanges();
  });

  it('should disable save when a goal is greater than 20', () => {
    component.onInputChange({ target: { value: '21' } } as unknown as Event, 'home');
    component.onInputChange({ target: { value: '1' } } as unknown as Event, 'away');

    expect(component.homeGoals).toBe(20);
    expect(component.isSaveDisabled()).toBe(false);
  });

  it('should disable save when a goal is negative', () => {
    component.onInputChange({ target: { value: '-1' } } as unknown as Event, 'home');
    component.onInputChange({ target: { value: '1' } } as unknown as Event, 'away');

    expect(component.homeGoals).toBe(0);
    expect(component.isSaveDisabled()).toBe(false);
  });

  it('should enable save when both goals are between 0 and 20', () => {
    component.onInputChange({ target: { value: '20' } } as unknown as Event, 'home');
    component.onInputChange({ target: { value: '0' } } as unknown as Event, 'away');

    expect(component.isSaveDisabled()).toBe(false);
  });

  it('should update the input element with the clamped value', () => {
    const target = { value: '43' } as HTMLInputElement;

    component.onInputChange({ target } as unknown as Event, 'away');

    expect(target.value).toBe('20');
    expect(component.awayGoals).toBe(20);
  });
});
