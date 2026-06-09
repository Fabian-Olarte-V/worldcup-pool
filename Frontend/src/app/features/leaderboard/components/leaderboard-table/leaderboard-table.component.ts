import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { LeaderboardUserViewModel } from '../../models/leaderboard.models';

@Component({
  selector: 'app-leaderboard-table',
  standalone: true,
  templateUrl: './leaderboard-table.component.html',
  styleUrls: ['./leaderboard-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LeaderboardTableComponent {
  @Input() users: LeaderboardUserViewModel[] = [];
  @Output() userSelected = new EventEmitter<LeaderboardUserViewModel>();

  onSelect(user: LeaderboardUserViewModel) {
    this.userSelected.emit(user);
  }
}
