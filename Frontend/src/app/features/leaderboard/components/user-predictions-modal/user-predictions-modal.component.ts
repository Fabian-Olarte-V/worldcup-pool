import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PredictionHistoryViewModel } from '../../models/leaderboard.models';

@Component({
  selector: 'app-user-predictions-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-predictions-modal.component.html',
  styleUrls: ['./user-predictions-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserPredictionsModalComponent {
  @Input() fullName = '';
  @Input() predictions: PredictionHistoryViewModel[] = [];
  @Output() closed = new EventEmitter<void>();

  trackById(_: number, item: PredictionHistoryViewModel) {
    return item.id;
  }

  onClose() {
    this.closed.emit();
  }
}
