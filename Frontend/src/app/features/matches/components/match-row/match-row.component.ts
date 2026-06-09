import {
  ChangeDetectionStrategy,
  Component,
  Input,
  Output,
  EventEmitter,
  inject,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { ResultRowComponent } from '../../../../shared/components/result-row/result-row.component';
import { ResultRowMatchViewModel } from '../../../../shared/components/result-row/result-row.component';
import { MatchEditService } from '../../services/match-edit.service';

export interface MatchSubmitEvent {
  payload: {
    MatchId: string;
    HomeGoals: number;
    AwayGoals: number;
  };
  previousResult: ResultRowMatchViewModel['result'];
  previousStatus: ResultRowMatchViewModel['status'];
  match: ResultRowMatchViewModel;
}

@Component({
  selector: 'app-match-row',
  standalone: true,
  imports: [CommonModule, ResultRowComponent],
  templateUrl: './match-row.component.html',
  styleUrl: './match-row.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchRowComponent implements OnInit {
  @Input({ required: true }) match!: ResultRowMatchViewModel;
  @Input() points: number | null = null;
  @Output() submitMatch = new EventEmitter<MatchSubmitEvent>();

  private readonly matchEditService = inject(MatchEditService);

  isEditing$!: Observable<boolean>;

  ngOnInit(): void {
    this.isEditing$ = this.matchEditService.getEditingState$(this.match.id);
  }

  onStartEditing(): void {
    this.matchEditService.startEditing(this.match.id);
  }

  onSaveResult(event: { matchId: string; homeGoals: number; awayGoals: number }): void {
    const previousResult = { ...this.match.result };
    const previousStatus = this.match.status;

    this.match.result = { homeGoals: event.homeGoals, awayGoals: event.awayGoals };
    this.match.status = 'Enviado';

    this.submitMatch.emit({
      payload: {
        MatchId: event.matchId,
        HomeGoals: event.homeGoals,
        AwayGoals: event.awayGoals,
      },
      previousResult,
      previousStatus,
      match: this.match,
    });
    this.matchEditService.cancelEditing(this.match.id);
  }
}
