import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TeamResultCardComponent, TeamResultCardViewModel } from '../team-result-card/team-result-card.component';
import { ResultStatusBadgeComponent } from '../result-status-badge/result-status-badge.component';
import { PredictionPayloadDto } from '../../models/prediction/prediction';

export interface MatchResultViewModel {
  homeGoals: number | null;
  awayGoals: number | null;
}

export interface ResultRowMatchViewModel {
  id: string;
  dateLabel: string;
  timeLabel: string;
  homeTeam: TeamResultCardViewModel;
  awayTeam: TeamResultCardViewModel;
  result: MatchResultViewModel;
  status: 'Pendiente' | 'Editando' | 'Finalizado' | 'Enviado';
  points?: number | null;
}

@Component({
  selector: 'app-result-row',
  standalone: true,
  imports: [CommonModule, FormsModule, TeamResultCardComponent, ResultStatusBadgeComponent],
  templateUrl: './result-row.component.html',
  styleUrl: './result-row.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResultRowComponent implements OnInit, OnChanges {
  @Input({ required: true }) match!: ResultRowMatchViewModel;
  @Input() isEditing = false;
  @Input() hideActions = false;
  @Input() forceEnableInputs = false;
  @Input() points: number | null = null;
  @Output() saveResult = new EventEmitter<{
    matchId: string;
    homeGoals: number;
    awayGoals: number;
  }>();
  @Output() editRequested = new EventEmitter<void>();
  @Output() resultChange = new EventEmitter<PredictionPayloadDto>();

  homeGoals: number | null = null;
  awayGoals: number | null = null;
  homeTouched = false;
  awayTouched = false;

  ngOnInit(): void {
    this.syncFromMatch();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['match']) {
      this.syncFromMatch();
    }
  }

  onSaveResult(): void {
    if (this.homeGoals !== null && this.awayGoals !== null) {
      this.saveResult.emit({
        matchId: this.match.id,
        homeGoals: this.homeGoals,
        awayGoals: this.awayGoals,
      });
    }
  }

  onActionClick(event: Event): void {
    event.stopPropagation();
    if (!this.isEditing) {
      this.editRequested.emit();
      return;
    }

    this.onSaveResult();
  }

  isSaveDisabled(): boolean {
    return !this.isEditing || this.homeGoals === null || this.awayGoals === null;
  }

  shouldShowIncompleteHint(): boolean {
    if (!this.forceEnableInputs) {
      return false;
    }

    const hasAnyTouched = this.homeTouched || this.awayTouched;
    const hasPartialResult =
      (this.homeGoals === null && this.awayGoals !== null) ||
      (this.homeGoals !== null && this.awayGoals === null);

    return hasAnyTouched && hasPartialResult;
  }

  onInputChange(event: Event, field: 'home' | 'away'): void {
    const target = event.target as HTMLInputElement;
    const parsedValue = target.value === '' ? null : parseInt(target.value, 10);
    const value = parsedValue === null || Number.isNaN(parsedValue) ? null : Math.max(0, parsedValue);

    if (field === 'home') {
      this.homeGoals = value;
      this.homeTouched = true;
    } else {
      this.awayGoals = value;
      this.awayTouched = true;
    }

    this.resultChange.emit({
      MatchId: this.match.id,
      HomeGoals: this.homeGoals,
      AwayGoals: this.awayGoals,
    });
  }

  private syncFromMatch(): void {
    this.homeGoals = this.match.result.homeGoals;
    this.awayGoals = this.match.result.awayGoals;
    this.homeTouched = false;
    this.awayTouched = false;
  }
}
