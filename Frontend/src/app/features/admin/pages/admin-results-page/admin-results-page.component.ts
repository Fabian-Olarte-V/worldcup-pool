import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, catchError, map, of, startWith } from 'rxjs';
import {
  ResultRowComponent,
  ResultRowMatchViewModel,
} from '../../../../shared/components/result-row/result-row.component';
import {
  UiNotificationService,
  UiNotificationType,
} from '../../../../shared/services/ui-notification.service';
import { MatchesService } from '../../../../shared/services/matches.service';
import { mapMatches } from '../../../../shared/utils/match-utils';
import {
  PredictionPayloadDto,
  PredictionSubmissionPayload,
} from '../../../../shared/models/prediction/prediction';
import { MatchDto } from '../../../matches/models/match';
import { MatchesPageViewModel } from '../../../matches/models/match';
import { MatchResultService } from '../../services/match-result.service';

@Component({
  selector: 'app-admin-results-page',
  standalone: true,
  imports: [CommonModule, ResultRowComponent],
  templateUrl: './admin-results-page.component.html',
  styleUrl: './admin-results-page.component.scss',
  changeDetection: ChangeDetectionStrategy.Default,
})
export class AdminResultsPage {
  private readonly matchesService = inject(MatchesService);
  private readonly matchResultService = inject(MatchResultService);
  private readonly uiNotificationService = inject(UiNotificationService);

  readonly savedResults = signal<PredictionSubmissionPayload[]>([]);
  readonly showResults = signal(false);
  readonly isSubmitting = signal(false);
  readonly rows = signal<ResultRowMatchViewModel[]>([]);

  vm$: Observable<MatchesPageViewModel> = this.buildVm();


  private buildVm(): Observable<MatchesPageViewModel> {
    return this.matchesService.getMatches().pipe(
      map((response): MatchesPageViewModel => {
        const matches = mapMatches(response.data ?? []);
        this.rows.set(matches);

        return {
          matches,
          loading: false,
          errorMessage: null,
        };
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
  }

  private patchFinishedRows(updatedMatches: MatchDto[]): void {
    if (updatedMatches.length === 0) {
      return;
    }

    const updatedById = new Map(updatedMatches.map((item) => [item.id, item] as const));

    this.rows.set(
      this.rows().map((row) => {
        const updated = updatedById.get(row.id);
        if (!updated) {
          return row;
        }

        return {
          ...row,
          result: {
            homeGoals: updated.homeGoals ?? row.result.homeGoals,
            awayGoals: updated.awayGoals ?? row.result.awayGoals,
          },
          status: 'Finalizado',
        };
      }),
    );
  }

  
  onReset(): void {
    this.savedResults.set([]);
    this.showResults.set(false);
    this.isSubmitting.set(false);
  }

  canFinish(matches: ResultRowMatchViewModel[] = []): boolean {
    void matches;
    return this.savedResults().length > 0 && !this.isSubmitting();
  }

  onResultChange(event: PredictionPayloadDto): void {
    if (event.HomeGoals === null || event.AwayGoals === null) {
      this.savedResults.set(this.savedResults().filter((draft) => draft.MatchId !== event.MatchId));
      this.rows.set(
        this.rows().map((row) =>
          row.id === event.MatchId && row.status === 'Editando'
            ? { ...row, status: 'Pendiente' }
            : row,
        ),
      );
      return;
    }

    const nextDrafts = [...this.savedResults()];
    const nextPayload: PredictionSubmissionPayload = {
      MatchId: event.MatchId,
      HomeGoals: event.HomeGoals,
      AwayGoals: event.AwayGoals,
    };
    const existingIndex = nextDrafts.findIndex((draft) => draft.MatchId === event.MatchId);

    if (existingIndex === -1) {
      nextDrafts.push(nextPayload);
    } else {
      nextDrafts[existingIndex] = nextPayload;
    }

    this.savedResults.set(nextDrafts);
    this.rows.set(
      this.rows().map((row) =>
        row.id === event.MatchId && row.status !== 'Finalizado'
          ? {
              ...row,
              result: { homeGoals: event.HomeGoals, awayGoals: event.AwayGoals },
              status: 'Editando',
            }
          : row,
      ),
    );
  }

  onFinishEditing(): void {
    const payloads = this.savedResults();
    if (payloads.length === 0 || this.isSubmitting()) {
      return;
    }

    this.isSubmitting.set(true);
    this.matchResultService.setMatchesResult(payloads).subscribe({
      next: (response) => {
        this.patchFinishedRows(response.data ?? []);
        this.showResults.set(true);
        this.isSubmitting.set(false);
        this.savedResults.set([]);
        this.uiNotificationService.show(
          'Resultados enviados correctamente.',
          UiNotificationType.Success,
        );
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.uiNotificationService.show('No fue posible enviar los resultados seleccionados.');
        console.error('Error submitting admin results:', error);
      },
    });
  }
}
