import { Component, inject } from '@angular/core';
import { NgClass } from '@angular/common';
import { UiNotificationService } from '../../services/ui-notification.service';

@Component({
  selector: 'app-notification-toast',
  standalone: true,
  imports: [NgClass],
  templateUrl: './notification-toast.html',
  styleUrl: './notification-toast.scss',
})
export class NotificationToast {
  readonly uiNotification = inject(UiNotificationService);
}
