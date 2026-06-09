import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export interface TeamResultCardViewModel {
  id: string;
  name: string;
  code: string;
}

@Component({
  selector: 'app-team-result-card',
  standalone: true,
  templateUrl: './team-result-card.component.html',
  styleUrl: './team-result-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TeamResultCardComponent {
  @Input({ required: true }) team!: TeamResultCardViewModel;
}
