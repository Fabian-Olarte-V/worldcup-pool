import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaderboardUserViewModel } from '../../models/leaderboard.models';

@Component({
  selector: 'app-leaderboard-row',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './leaderboard-row.component.html',
  styleUrls: ['./leaderboard-row.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LeaderboardRowComponent {
  @Input() user!: LeaderboardUserViewModel;
  @Output() open = new EventEmitter<LeaderboardUserViewModel>();

  onOpen() {
    this.open.emit(this.user);
  }
}
