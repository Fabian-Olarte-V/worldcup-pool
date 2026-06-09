import { Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import * as AuthActions from './features/auth/store/auth.actions';
import * as AuthSelectors from './features/auth/store/auth.selectors';
import { filter, startWith } from 'rxjs';
import { NotificationToast } from './shared/components/notification-toast/notification-toast';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, NotificationToast],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private readonly store = inject(Store);
  private readonly router = inject(Router);
  readonly isAdmin = this.store.selectSignal(AuthSelectors.selectIsAdmin);
  showHeader = true;

  constructor() {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        startWith(null),
      )
      .subscribe(() => {
        this.showHeader = !this.isCurrentRouteHidden();
      });
  }

  logout(): void {
    this.store.dispatch(AuthActions.logout());
  }

  private isCurrentRouteHidden(): boolean {
    let current = this.router.routerState.snapshot.root;

    while (current.firstChild) {
      current = current.firstChild;
    }

    return Boolean(current.data['hideHeader']);
  }
}
