import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AuthService } from '../services/auth.service';
import * as AuthActions from './auth.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { AppUser, UserRole } from '../models/appUser';

@Injectable()
export class AuthEffects {
  private readonly actions$ = inject(Actions);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  private buildSessionUser(userId: string, token: string): AppUser {
    return {
      id: userId,
      role: this.extractUserRole(token),
    };
  }

  private extractUserRole(token: string): UserRole | null {
    try {
      const [, payload] = token.split('.');
      if (!payload) {
        return null;
      }

      const decodedPayload = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/'))) as Record<
        string,
        unknown
      >;

      const candidateRole =
        decodedPayload['role'] ??
        decodedPayload['roles'] ??
        decodedPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      const normalizedRole = Array.isArray(candidateRole) ? candidateRole[0] : candidateRole;

      if (normalizedRole === 'ADMIN' || normalizedRole === 'USER') {
        return normalizedRole;
      }

      if (normalizedRole === 'Admin') {
        return 'ADMIN';
      }

      if (normalizedRole === 'User') {
        return 'USER';
      }

      return null;
    } catch {
      return null;
    }
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.message || 'Unexpected authentication error.';
    }

    if (error instanceof Error) {
      return error.message;
    }

    return 'Unexpected authentication error.';
  }

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(({ user }) =>
        this.authService.login(user).pipe(
          map((response) => {
            const auth = response.data;
            return (
            AuthActions.loginSuccess({
              user: this.buildSessionUser(auth.userId, auth.token),
              token: auth.token,
              refreshToken: auth.refreshToken,
              expiresAtUtc: auth.expiresAtUtc,
            })
          );
        }),
          catchError((error) =>
            of(AuthActions.loginFailure({ error: this.getErrorMessage(error) })),
          ),
        ),
      ),
    ),
  );

  signup$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.signup),
      switchMap(({ user }) =>
        this.authService.signup(user).pipe(
          map((response) => {
            const auth = response.data;
            return (
            AuthActions.signupSuccess({
              user: this.buildSessionUser(auth.userId, auth.token),
              token: auth.token,
              refreshToken: auth.refreshToken,
              expiresAtUtc: auth.expiresAtUtc,
            })
          );
        }),
          catchError((error) =>
            of(AuthActions.signupFailure({ error: this.getErrorMessage(error) })),
          ),
        ),
      ),
    ),
  );

  authSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess, AuthActions.signupSuccess, AuthActions.updateUserSession),
        tap(() => {
          this.router.navigate(['/matches']);
        }),
      ),
    { dispatch: false },
  );

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          this.router.navigate(['/login']);
        }),
      ),
    { dispatch: false },
  );
}
