import { ChangeDetectionStrategy, Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TeamViewModel } from '../../models/match';

@Component({
  selector: 'app-team-card',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './team-card.component.html',
  styleUrl: './team-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TeamCardComponent {
  @Input({ required: true }) team!: TeamViewModel;
  @Input() isEditing = false;
  @Input() goals = 0;
  @Output() goalsChange = new EventEmitter<number>();

  onGoalsChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    const value = parseInt(target.value, 10);
    this.goalsChange.emit(value);
  }
}

