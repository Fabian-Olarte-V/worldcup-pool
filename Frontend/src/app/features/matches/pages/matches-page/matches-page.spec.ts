import { ComponentFixture, TestBed } from '@angular/core/testing';
import { firstValueFrom, of, skip, take, throwError } from 'rxjs';
import { MatchesPage } from './matches-page';
import { MatchesService } from '../../../../shared/services/matches.service';
import { PredictionService } from '../../../../shared/services/prediction.service';
import {
  UiNotificationService,
  UiNotificationType,
} from '../../../../shared/services/ui-notification.service';
import { MatchDto } from '../../models/match';
import { PredictionDto } from '../../../../shared/models/prediction/prediction';
import { ResultRowMatchViewModel } from '../../../../shared/components/result-row/result-row.component';

describe('MatchesPage', () => {
  let component: MatchesPage;
  let fixture: ComponentFixture<MatchesPage>;
  let matchesService: { getMatches: ReturnType<typeof vi.fn> };
  let predictionService: {
    getPredictions: ReturnType<typeof vi.fn>;
    createPrediction: ReturnType<typeof vi.fn>;
  };
  let uiNotificationService: { show: ReturnType<typeof vi.fn> };

  const match: MatchDto = {
    id: 'match-1',
    groupName: 'A',
    homeTeam: 'Colombia',
    homeTeamCode: 'COL',
    awayTeam: 'Brazil',
    awayTeamCode: 'BRA',
    status: 'Scheduled',
    startTimeUtc: '2026-06-08T18:00:00Z',
  };

  const prediction: PredictionDto = {
    id: 'pred-1',
    matchId: 'match-1',
    homeGoals: 2,
    awayGoals: 1,
    points: 3,
  };

  beforeEach(async () => {
    matchesService = {
      getMatches: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 200,
          message: 'ok',
          data: [match],
        }),
      ),
    };
    predictionService = {
      getPredictions: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 200,
          message: 'ok',
          data: [prediction],
        }),
      ),
      createPrediction: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 201,
          message: 'created',
          data: prediction,
        }),
      ),
    };
    uiNotificationService = {
      show: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [MatchesPage],
      providers: [
        { provide: MatchesService, useValue: matchesService },
        { provide: PredictionService, useValue: predictionService },
        { provide: UiNotificationService, useValue: uiNotificationService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MatchesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should build the view model with matches enriched by saved predictions', async () => {
    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm.loading).toBe(false);
    expect(vm.errorMessage).toBeNull();
    expect(vm.matches).toHaveLength(1);
    expect(vm.matches[0].status).toBe('Enviado');
    expect(vm.matches[0].result).toEqual({ homeGoals: 2, awayGoals: 1 });
    expect(vm.matches[0].points).toBe(3);
  });

  it('should keep matches visible when predictions request fails', async () => {
    predictionService.getPredictions.mockReturnValueOnce(throwError(() => new Error('boom')));
    fixture = TestBed.createComponent(MatchesPage);
    component = fixture.componentInstance;

    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm.loading).toBe(false);
    expect(vm.errorMessage).toBeNull();
    expect(vm.matches).toHaveLength(1);
    expect(vm.matches[0].status).toBe('Pendiente');
    expect(vm.matches[0].points).toBeUndefined();
  });

  it('should expose an error view model when matches fail to load', async () => {
    matchesService.getMatches.mockReturnValueOnce(throwError(() => new Error('boom')));
    fixture = TestBed.createComponent(MatchesPage);
    component = fixture.componentInstance;

    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm).toEqual({
      matches: [],
      loading: false,
      errorMessage: 'Live fixtures could not be loaded.',
    });
  });

  it('should notify success when a prediction is submitted', () => {
    const matchRow: ResultRowMatchViewModel = {
      id: 'match-1',
      dateLabel: '08 June',
      timeLabel: '18:00',
      homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
      awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
      result: { homeGoals: 1, awayGoals: 0 },
      status: 'Enviado',
    };

    component.onMatchSubmit({
      payload: {
        MatchId: 'match-1',
        HomeGoals: 1,
        AwayGoals: 0,
      },
      previousResult: { homeGoals: null, awayGoals: null },
      previousStatus: 'Pendiente',
      match: matchRow,
    });

    expect(predictionService.createPrediction).toHaveBeenCalledWith({
      MatchId: 'match-1',
      HomeGoals: 1,
      AwayGoals: 0,
    });
    expect(uiNotificationService.show).toHaveBeenCalledWith(
      'Predicción enviada exitosamente',
      UiNotificationType.Success,
    );
  });

  it('should notify error when prediction submission fails', () => {
    predictionService.createPrediction.mockReturnValueOnce(
      throwError(() => new Error('not allowed')),
    );
    const matchRow: ResultRowMatchViewModel = {
      id: 'match-1',
      dateLabel: '08 June',
      timeLabel: '18:00',
      homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
      awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
      result: { homeGoals: 1, awayGoals: 0 },
      status: 'Enviado',
    };

    component.onMatchSubmit({
      payload: {
        MatchId: 'match-1',
        HomeGoals: 1,
        AwayGoals: 0,
      },
      previousResult: { homeGoals: null, awayGoals: null },
      previousStatus: 'Pendiente',
      match: matchRow,
    });

    expect(matchRow.result).toEqual({ homeGoals: null, awayGoals: null });
    expect(matchRow.status).toBe('Pendiente');
    expect(uiNotificationService.show).toHaveBeenCalledWith(
      'You must be logged in to access this page.',
    );
  });
});
