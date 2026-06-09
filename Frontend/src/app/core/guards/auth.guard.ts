import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take } from 'rxjs';
import * as AuthSelectors from '../../features/auth/store/auth.selectors';
import { UiNotificationService } from '../../shared/services/ui-notification.service';

export const authGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);
  const uiNotificationService = inject(UiNotificationService);

  return store.select(AuthSelectors.selectIsAuthenticated).pipe(
    take(1),
    map((isAuthenticated) => {
      if (isAuthenticated) {
        return true;
      } else {
        uiNotificationService.show('You must be logged in to access this page.');
        return router.createUrlTree(['/login']);
      }
    }),
  );
};

export const isAdminGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);
  const uiNotificationService = inject(UiNotificationService);

  return store.select(AuthSelectors.selectIsAdmin).pipe(
    take(1),
    map((isAdmin) => {
      if (isAdmin) {
        return true;
      }

      uiNotificationService.show('You do not have permission to access the admin panel.');
      return router.createUrlTree(['/matches']);
    }),
  );
};
