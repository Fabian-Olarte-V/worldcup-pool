import { Injectable, signal } from '@angular/core';

export enum UiNotificationType {
  Error = 'error',
  Success = 'success',
}

export interface UiNotificationState {
  message: string;
  type: UiNotificationType;
}

@Injectable({ providedIn: 'root' })
export class UiNotificationService {
  readonly notification = signal<UiNotificationState | null>(null);
  private clearTimeoutId: ReturnType<typeof setTimeout> | null = null;

  show(message: string, type: UiNotificationType = UiNotificationType.Error): void {
    this.notification.set({ message, type });

    if (this.clearTimeoutId) {
      clearTimeout(this.clearTimeoutId);
    }

    this.clearTimeoutId = setTimeout(() => {
      this.clear();
    }, 2000);
  }

  clear(): void {
    this.notification.set(null);

    if (this.clearTimeoutId) {
      clearTimeout(this.clearTimeoutId);
      this.clearTimeoutId = null;
    }
  }
}
