import { ComponentFixture, TestBed } from '@angular/core/testing';
import { firstValueFrom, of, skip, take, throwError } from 'rxjs';
import { AdminResultsPage } from './admin-results-page.component';
import { MatchesService } from '../../../../shared/services/matches.service';
import { MatchResultService } from '../../services/match-result.service';
import {
  UiNotificationService,
  UiNotificationType,
} from '../../../../shared/services/ui-notification.service';
import { MatchDto } from '../../../matches/models/match';

describe('AdminResultsPage', () => {
  let component: AdminResultsPage;
  let fixture: ComponentFixture<AdminResultsPage>;
  let matchesService: { getMatches: ReturnType<typeof vi.fn> };
  let matchResultService: { setMatchesResult: ReturnType<typeof vi.fn> };
  let uiNotificationService: { show: ReturnType<typeof vi.fn> };
  let consoleErrorSpy: ReturnType<typeof vi.spyOn>;

  const pendingMatch: MatchDto = {
    id: 'match-1',
    groupName: 'A',
    homeTeam: 'Colombia',
    homeTeamCode: 'COL',
    awayTeam: 'Brazil',
    awayTeamCode: 'BRA',
    status: 'Scheduled',
    startTimeUtc: '2026-06-08T18:00:00Z',
  };

  beforeEach(async () => {
    consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => undefined);
    matchesService = {
      getMatches: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 200,
          message: 'ok',
          data: [pendingMatch],
        }),
      ),
    };
    matchResultService = {
      setMatchesResult: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 200,
          message: 'ok',
          data: [
            {
              ...pendingMatch,
              status: 'Finished',
              homeGoals: 2,
              awayGoals: 1,
              hasFinalResult: true,
            },
          ],
        }),
      ),
    };
    uiNotificationService = {
      show: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [AdminResultsPage],
      providers: [
        { provide: MatchesService, useValue: matchesService },
        { provide: MatchResultService, useValue: matchResultService },
        { provide: UiNotificationService, useValue: uiNotificationService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminResultsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    consoleErrorSpy.mockRestore();
  });

  it('should load matches into the admin view model and rows signal', async () => {
    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm.loading).toBe(false);
    expect(vm.errorMessage).toBeNull();
    expect(vm.matches).toHaveLength(1);
    expect(component.rows()).toHaveLength(1);
    expect(component.rows()[0].id).toBe('match-1');
  });

  it('should expose an error view model when matches fail to load', async () => {
    matchesService.getMatches.mockReturnValueOnce(throwError(() => new Error('boom')));
    fixture = TestBed.createComponent(AdminResultsPage);
    component = fixture.componentInstance;

    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm).toEqual({
      matches: [],
      loading: false,
      errorMessage: 'Live fixtures could not be loaded.',
    });
  });

  it('should save draft results and mark the edited row', () => {
    component.rows.set([
      {
        id: 'match-1',
        dateLabel: '08 June',
        timeLabel: '18:00',
        homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
        awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
        result: { homeGoals: null, awayGoals: null },
        status: 'Pendiente',
      },
    ]);

    component.onResultChange({
      MatchId: 'match-1',
      HomeGoals: 2,
      AwayGoals: 1,
    });

    expect(component.savedResults()).toEqual([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    expect(component.rows()[0].status).toBe('Editando');
    expect(component.canFinish(component.rows())).toBe(true);
  });

  it('should clear a draft when goals are reset to null', () => {
    component.savedResults.set([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    component.rows.set([
      {
        id: 'match-1',
        dateLabel: '08 June',
        timeLabel: '18:00',
        homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
        awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
        result: { homeGoals: 2, awayGoals: 1 },
        status: 'Editando',
      },
    ]);

    component.onResultChange({
      MatchId: 'match-1',
      HomeGoals: null,
      AwayGoals: null,
    });

    expect(component.savedResults()).toEqual([]);
    expect(component.rows()[0].status).toBe('Pendiente');
  });

  it('should clear a draft when goals are outside the allowed range', () => {
    component.savedResults.set([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    component.rows.set([
      {
        id: 'match-1',
        dateLabel: '08 June',
        timeLabel: '18:00',
        homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
        awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
        result: { homeGoals: 2, awayGoals: 1 },
        status: 'Editando',
      },
    ]);

    component.onResultChange({
      MatchId: 'match-1',
      HomeGoals: 21,
      AwayGoals: 1,
    });

    expect(component.savedResults()).toEqual([]);
    expect(component.rows()[0].status).toBe('Pendiente');
  });

  it('should submit results, mark rows as finished and notify success', () => {
    component.savedResults.set([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    component.rows.set([
      {
        id: 'match-1',
        dateLabel: '08 June',
        timeLabel: '18:00',
        homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
        awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
        result: { homeGoals: 2, awayGoals: 1 },
        status: 'Editando',
      },
    ]);

    component.onFinishEditing();

    expect(matchResultService.setMatchesResult).toHaveBeenCalledWith([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    expect(component.rows()[0].status).toBe('Finalizado');
    expect(component.showResults()).toBe(true);
    expect(component.savedResults()).toEqual([]);
    expect(component.isSubmitting()).toBe(false);
    expect(uiNotificationService.show).toHaveBeenCalledWith(
      'Resultados enviados correctamente.',
      UiNotificationType.Success,
    );
  });

  it('should disable the submit button until there are edited results', () => {
    fixture.detectChanges();
    let button = fixture.nativeElement.querySelector('.admin-results-page__finish-btn');
    expect(button.disabled).toBe(true);

    component.savedResults.set([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    fixture.detectChanges();

    button = fixture.nativeElement.querySelector('.admin-results-page__finish-btn');
    expect(button.disabled).toBe(false);
  });

  it('should submit results when clicking the finish button', () => {
    component.savedResults.set([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('.admin-results-page__finish-btn');
    button.click();

    expect(matchResultService.setMatchesResult).toHaveBeenCalledWith([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
  });

  it('should keep drafts and notify error when result submission fails', () => {
    matchResultService.setMatchesResult.mockReturnValueOnce(
      throwError(() => new Error('boom')),
    );
    component.savedResults.set([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);

    component.onFinishEditing();

    expect(component.isSubmitting()).toBe(false);
    expect(component.savedResults()).toEqual([
      {
        MatchId: 'match-1',
        HomeGoals: 2,
        AwayGoals: 1,
      },
    ]);
    expect(uiNotificationService.show).toHaveBeenCalledWith(
      'No fue posible enviar los resultados seleccionados.',
    );
  });
});
