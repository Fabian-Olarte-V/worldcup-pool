import { Routes } from '@angular/router';
import { authGuard, isAdminGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    data: { hideHeader: true },
    loadComponent: () =>
      import('./features/auth/pages/auth-page/auth-page').then((m) => m.AuthPage),
  },
  {
    path: 'matches',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/matches/pages/matches-page/matches-page').then((m) => m.MatchesPage),
  },
  {
    path: 'leaderboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/leaderboard/pages/leaderboard-page/leaderboard-page.component').then(
        (m) => m.LeaderboardPageComponent,
      ),
  },
  {
    path: 'admin-panel',
    canActivate: [authGuard, isAdminGuard],
    loadComponent: () =>
      import('./features/admin/pages/admin-results-page/admin-results-page.component').then(
        (m) => m.AdminResultsPage,
      ),
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
