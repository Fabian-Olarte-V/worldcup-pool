import { ComponentFixture, TestBed } from '@angular/core/testing';
import { firstValueFrom, of, skip, take, throwError } from 'rxjs';
import { LeaderboardPageComponent } from './leaderboard-page.component';
import { LeaderboardService } from '../../services/leaderboard.service';
import { MatchesService } from '../../../../shared/services/matches.service';
import { PredictionService } from '../../../../shared/services/prediction.service';
import { LeaderboardUserViewModel } from '../../models/leaderboard.models';
import { MatchDto } from '../../../matches/models/match';
import { PredictionDto } from '../../../../shared/models/prediction/prediction';
import { By } from '@angular/platform-browser';
import { UserPredictionsModalComponent } from '../../components/user-predictions-modal/user-predictions-modal.component';

describe('LeaderboardPageComponent', () => {
  let component: LeaderboardPageComponent;
  let fixture: ComponentFixture<LeaderboardPageComponent>;
  let leaderboardService: { getLeaderboard: ReturnType<typeof vi.fn> };
  let matchesService: { getMatches: ReturnType<typeof vi.fn> };
  let predictionService: { getPredictionsByUserId: ReturnType<typeof vi.fn> };

  const leaderboardUser: LeaderboardUserViewModel = {
    userId: 'user-1',
    userName: 1,
    fullName: 'Ada Lovelace',
    points: 12,
    wonCount: 4,
    correctOutcomeCount: 3,
    lostCount: 1,
  };

  const match: MatchDto = {
    id: 'match-1',
    groupName: 'A',
    homeTeam: 'Colombia',
    homeTeamCode: 'COL',
    awayTeam: 'Brazil',
    awayTeamCode: 'BRA',
    status: 'Finished',
    startTimeUtc: '2026-06-08T18:00:00Z',
    homeGoals: 2,
    awayGoals: 1,
    hasFinalResult: true,
  };

  const prediction: PredictionDto = {
    id: 'pred-1',
    matchId: 'match-1',
    homeGoals: 2,
    awayGoals: 1,
    points: 3,
  };

  beforeEach(async () => {
    leaderboardService = {
      getLeaderboard: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 200,
          message: 'ok',
          data: [leaderboardUser],
        }),
      ),
    };
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
      getPredictionsByUserId: vi.fn().mockReturnValue(
        of({
          success: true,
          statusCode: 200,
          message: 'ok',
          data: [prediction],
        }),
      ),
    };

    await TestBed.configureTestingModule({
      imports: [LeaderboardPageComponent],
      providers: [
        { provide: LeaderboardService, useValue: leaderboardService },
        { provide: MatchesService, useValue: matchesService },
        { provide: PredictionService, useValue: predictionService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LeaderboardPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should map leaderboard users into ranked rows', async () => {
    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm.loading).toBe(false);
    expect(vm.errorMessage).toBeNull();
    expect(vm.users).toEqual([
      {
        position: 1,
        userId: 'user-1',
        fullName: 'Ada Lovelace',
        points: 12,
        wonCount: 4,
        correctOutcomeCount: 3,
        lostCount: 1,
      },
    ]);
  });

  it('should expose an error view model when leaderboard loading fails', async () => {
    leaderboardService.getLeaderboard.mockReturnValueOnce(throwError(() => new Error('boom')));
    fixture = TestBed.createComponent(LeaderboardPageComponent);
    component = fixture.componentInstance;

    const vm = await firstValueFrom(component.vm$.pipe(skip(1), take(1)));

    expect(vm).toEqual({
      users: [],
      loading: false,
      errorMessage: 'Leaderboard could not be loaded.',
    });
  });

  it('should load and map a user prediction history when opening the modal', () => {
    component.openHistory({
      position: 1,
      userId: 'user-1',
      fullName: 'Ada Lovelace',
      points: 12,
      wonCount: 4,
      correctOutcomeCount: 3,
      lostCount: 1,
    });

    expect(predictionService.getPredictionsByUserId).toHaveBeenCalledWith('user-1');
    expect(component.selectedUser?.fullName).toBe('Ada Lovelace');
    expect(component.selectedPredictions).toHaveLength(1);
    expect(component.selectedPredictions[0]).toMatchObject({
      id: 'pred-1',
      predictedHomeGoals: 2,
      predictedAwayGoals: 1,
      actualHomeGoals: 2,
      actualAwayGoals: 1,
      points: 3,
      status: 'Finalizado',
    });
  });

  it('should open the history modal when clicking the history button', () => {
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('.btn.btn-ghost');
    button.click();
    fixture.detectChanges();

    expect(predictionService.getPredictionsByUserId).toHaveBeenCalledWith('user-1');
    expect(component.selectedUser?.userId).toBe('user-1');
    expect(fixture.nativeElement.querySelector('app-user-predictions-modal')).not.toBeNull();
  });

  it('should keep the modal open with empty predictions when history loading fails', () => {
    predictionService.getPredictionsByUserId.mockReturnValueOnce(
      throwError(() => new Error('boom')),
    );

    component.openHistory({
      position: 1,
      userId: 'user-1',
      fullName: 'Ada Lovelace',
      points: 12,
      wonCount: 4,
      correctOutcomeCount: 3,
      lostCount: 1,
    });

    expect(component.selectedUser?.userId).toBe('user-1');
    expect(component.selectedPredictions).toEqual([]);
  });

  it('should clear the selected history when closing the modal', () => {
    component.selectedUser = {
      position: 1,
      userId: 'user-1',
      fullName: 'Ada Lovelace',
      points: 12,
      wonCount: 4,
      correctOutcomeCount: 3,
      lostCount: 1,
    };
    component.selectedPredictions = [
      {
        id: 'pred-1',
        dateLabel: '08 June',
        timeLabel: '18:00',
        homeTeam: { id: 'col', name: 'Colombia', code: 'COL' },
        awayTeam: { id: 'bra', name: 'Brazil', code: 'BRA' },
        predictedHomeGoals: 2,
        predictedAwayGoals: 1,
        actualHomeGoals: 2,
        actualAwayGoals: 1,
        points: 3,
        status: 'Finalizado',
      },
    ];

    component.closeHistory();

    expect(component.selectedUser).toBeNull();
    expect(component.selectedPredictions).toEqual([]);
  });

  it('should close the history modal when the modal emits closed', () => {
    const button = fixture.nativeElement.querySelector('.btn.btn-ghost');
    button.click();
    fixture.detectChanges();

    const modal = fixture.debugElement.query(By.directive(UserPredictionsModalComponent));
    modal.componentInstance.closed.emit();
    fixture.detectChanges();

    expect(component.selectedUser).toBeNull();
    expect(component.selectedPredictions).toEqual([]);
  });
});
