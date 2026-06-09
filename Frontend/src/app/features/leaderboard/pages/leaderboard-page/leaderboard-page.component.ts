import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject } from '@angular/core';
import { Observable, catchError, forkJoin, map, of, startWith } from 'rxjs';
import { UserPredictionsModalComponent } from '../../components/user-predictions-modal/user-predictions-modal.component';
import { LeaderboardService } from '../../services/leaderboard.service';
import { MatchesService } from '../../../../shared/services/matches.service';
import { PredictionService } from '../../../../shared/services/prediction.service';
import {
  LeaderboardPageViewModel,
  LeaderboardUserRowViewModel,
  LeaderboardUserViewModel,
  PredictionHistoryViewModel,
} from '../../models/leaderboard.models';
import { PredictionDto } from '../../../../shared/models/prediction/prediction';
import { MatchDto } from '../../../matches/models/match';
import { formatDateLabel, formatTimeLabel } from '../../../../shared/utils/match-utils';

@Component({
  selector: 'app-leaderboard-page',
  standalone: true,
  imports: [AsyncPipe, UserPredictionsModalComponent],
  templateUrl: './leaderboard-page.component.html',
  styleUrls: ['./leaderboard-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LeaderboardPageComponent {
  private readonly leaderboardService = inject(LeaderboardService);
  private readonly matchesService = inject(MatchesService);
  private readonly predictionService = inject(PredictionService);
  private readonly cdr = inject(ChangeDetectorRef);

  selectedUser: LeaderboardUserRowViewModel | null = null;
  selectedPredictions: PredictionHistoryViewModel[] = [];

  readonly vm$: Observable<LeaderboardPageViewModel> = this.leaderboardService
    .getLeaderboard()
    .pipe(
      map((response): LeaderboardPageViewModel => {
        const users = (response.data ?? []).map((user, index) => this.toRow(user, index));

        return {
          users,
          loading: false,
          errorMessage: null,
        };
      }),
      catchError(() =>
        of<LeaderboardPageViewModel>({
          users: [],
          loading: false,
          errorMessage: 'Leaderboard could not be loaded.',
        }),
      ),
      startWith<LeaderboardPageViewModel>({
        users: [],
        loading: true,
        errorMessage: null,
      }),
    );

  private mapPredictionsToHistory(
    predictions: PredictionDto[],
    matches: MatchDto[],
  ): PredictionHistoryViewModel[] {
    const matchMap = new Map(matches.map((match) => [match.id, match] as const));

    return predictions.map((prediction) => {
      const match = matchMap.get(prediction.matchId);

      return {
        id: prediction.id,
        dateLabel: match ? (formatDateLabel(match.startTimeUtc) ?? 'TBD') : 'TBD',
        timeLabel: match ? (formatTimeLabel(match.startTimeUtc) ?? 'TBD') : 'TBD',
        homeTeam: {
          id: match?.homeTeamCode ?? 'unknown',
          name: match?.homeTeam ?? 'Unknown',
          code: match?.homeTeamCode ?? 'UNK',
        },
        awayTeam: {
          id: match?.awayTeamCode ?? 'unknown',
          name: match?.awayTeam ?? 'Unknown',
          code: match?.awayTeamCode ?? 'UNK',
        },
        predictedHomeGoals: prediction.homeGoals ?? 0,
        predictedAwayGoals: prediction.awayGoals ?? 0,
        actualHomeGoals: match?.homeGoals ?? null,
        actualAwayGoals: match?.awayGoals ?? null,
        points: prediction.points ?? null,
        status: match?.status === 'Finished' || match?.hasFinalResult ? 'Finalizado' : 'Pendiente',
      };
    });
  }

  openHistory(user: LeaderboardUserRowViewModel): void {
    this.selectedUser = user;
    this.selectedPredictions = [];
    this.cdr.markForCheck();

    forkJoin({
      predictionsResp: this.predictionService.getPredictionsByUserId(user.userId),
      matchesResp: this.matchesService.getMatches(),
    }).subscribe({
      next: ({ predictionsResp, matchesResp }) => {
        this.selectedPredictions = this.mapPredictionsToHistory(
          predictionsResp.data ?? [],
          matchesResp.data ?? [],
        );
        this.cdr.markForCheck();
      },
      error: () => {
        this.selectedPredictions = [];
        this.cdr.markForCheck();
      },
    });
  }

  closeHistory(): void {
    this.selectedUser = null;
    this.selectedPredictions = [];
    this.cdr.markForCheck();
  }

  private toRow(user: LeaderboardUserViewModel, index: number): LeaderboardUserRowViewModel {
    return {
      position: index + 1,
      userId: user.userId,
      fullName: user.fullName,
      points: user.points,
      wonCount: user.wonCount,
      correctOutcomeCount: user.correctOutcomeCount,
      lostCount: user.lostCount,
    };
  }
}
