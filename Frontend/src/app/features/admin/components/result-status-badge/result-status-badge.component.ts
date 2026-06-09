import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-result-status-badge',
  standalone: true,
  templateUrl: './result-status-badge.component.html',
  styleUrl: './result-status-badge.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResultStatusBadgeComponent {
  @Input({ required: true }) status!: 'Pendiente' | 'Finalizado';
}
