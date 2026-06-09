import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { UiNotificationService } from '../../shared/services/ui-notification.service';

export const errorHandlerInterceptor: HttpInterceptorFn = (req, next) => {
  const uiNotificationService = inject(UiNotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const message = error.error?.message || error.message || 'Unexpected error';
      uiNotificationService.show(message);

      return throwError(() => error);
    }),
  );
};
