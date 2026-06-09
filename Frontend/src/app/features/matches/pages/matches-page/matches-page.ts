import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Observable, catchError, map, of, startWith, combineLatest } from 'rxjs';
import { MatchesPageViewModel } from '../../models/match';
import { ApiResponse } from '../../../../shared/models/apiResponse/apiResponse';
import { MatchRowComponent } from '../../components/match-row/match-row.component';
import { MatchSubmitEvent } from '../../components/match-row/match-row.component';
import { ResultRowMatchViewModel } from '../../../../shared/components/result-row/result-row.component';
import {
  UiNotificationService,
  UiNotificationType,
} from '../../../../shared/services/ui-notification.service';
import { MatchesService } from '../../../../shared/services/matches.service';
import { mapMatches } from '../../../../shared/utils/match-utils';
import { PredictionService } from '../../../../shared/services/prediction.service';
import { PredictionDto } from '../../../../shared/models/prediction/prediction';

@Component({
  selector: 'app-matches-page',
  standalone: true,
  imports: [AsyncPipe, MatchRowComponent],
  templateUrl: './matches-page.html',
  styleUrl: './matches-page.scss',
  changeDetection: ChangeDetectionStrategy.Default,
})
export class MatchesPage {
  private readonly matchesService = inject(MatchesService);
  private readonly predictionService = inject(PredictionService);
  private readonly uiNotificationService = inject(UiNotificationService);

  readonly vm$: Observable<MatchesPageViewModel> = combineLatest({
    matchesResp: this.matchesService.getMatches(),
    predsResp: this.predictionService.getPredictions().pipe(
      catchError(() =>
        of<ApiResponse<PredictionDto[]>>({
          success: false,
          statusCode: 0,
          message: '',
          data: [],
        }),
      ),
    ),
  }).pipe(
    map(({ matchesResp, predsResp }): MatchesPageViewModel => {
      const baseMatches = mapMatches(matchesResp.data ?? []);
      const preds = predsResp.data ?? [];
      const predMap = new Map<string, PredictionDto>(
        preds.map((p: PredictionDto) => [p.matchId, p] as [string, PredictionDto]),
      );

      const enriched = baseMatches.map((m) => {
        const p = predMap.get(m.id);
        if (!p) return m;
        const data: ResultRowMatchViewModel = {
          ...m,
          result:
            m.status === 'Finalizado'
              ? { homeGoals: m.result.homeGoals, awayGoals: m.result.awayGoals }
              : { homeGoals: p.homeGoals, awayGoals: p.awayGoals },
          status: m.status === 'Finalizado' ? 'Finalizado' : 'Enviado',
          points: p.points ?? null,
        };
        return data;
      });

      return { matches: enriched, loading: false, errorMessage: null };
    }),
    catchError(() =>
      of<MatchesPageViewModel>({
        matches: [],
        loading: false,
        errorMessage: 'Live fixtures could not be loaded.',
      }),
    ),
    startWith<MatchesPageViewModel>({
      matches: [],
      loading: true,
      errorMessage: null,
    }),
  );

  onMatchSubmit({ payload, previousResult, previousStatus, match }: MatchSubmitEvent): void {
    this.predictionService.createPrediction(payload).subscribe({
      next: () => {
        this.uiNotificationService.show(
          'Predicción enviada exitosamente',
          UiNotificationType.Success,
        );
      },
      error: () => {
        match.result = previousResult;
        match.status = previousStatus;
        this.uiNotificationService.show('You must be logged in to access this page.');
      },
    });
  }
}
