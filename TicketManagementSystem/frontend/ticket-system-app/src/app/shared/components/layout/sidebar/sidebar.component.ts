import { Component, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthState } from '../../../../core/authentication/state/auth.state';

interface NavItem {
  label: string;
  route: string;
  icon: string;
  roles?: string[];
  badge?: number;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <aside class="w-64 h-[calc(100vh-4rem)] sticky top-16 backdrop-blur-xl bg-white/5 border-r border-white/10 overflow-hidden">
      <!-- Navigation -->
      <nav class="p-4 space-y-2">
        <p class="px-3 text-xs font-semibold text-white/40 uppercase tracking-wider mb-4">Navigation</p>

        <ul class="space-y-1">
          <li *ngFor="let item of filteredNavItems(); let i = index">
            <a
              [routerLink]="item.route"
              routerLinkActive="active-nav-item"
              [routerLinkActiveOptions]="{exact: item.route === '/dashboard'}"
              class="group flex items-center px-4 py-3 text-sm font-medium text-white/70 rounded-xl hover:bg-white/10 hover:text-white transition-all duration-300 relative overflow-hidden"
            >
              <!-- Hover gradient effect -->
              <div class="absolute inset-0 bg-gradient-to-r from-primary-600/20 to-primary-500/10 opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>

              <!-- Icon container -->
              <div class="relative z-10 w-9 h-9 flex items-center justify-center rounded-lg bg-white/5 group-hover:bg-primary-500/20 mr-3 transition-all duration-300">
                <svg class="w-5 h-5 group-hover:text-primary-400 transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path [attr.d]="item.icon" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                </svg>
              </div>

              <span class="relative z-10 flex-1">{{ item.label }}</span>

              <!-- Badge -->
              <span *ngIf="item.badge" class="relative z-10 px-2 py-0.5 text-xs font-bold rounded-full bg-primary-500/30 text-primary-300 border border-primary-500/30">
                {{ item.badge }}
              </span>

              <!-- Active indicator -->
              <div class="absolute left-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary-500 rounded-r-full opacity-0 transition-opacity duration-300"></div>
            </a>
          </li>
        </ul>
      </nav>

      <!-- Bottom section -->
      <div class="absolute bottom-0 left-0 right-0 p-4 border-t border-white/10 backdrop-blur-xl bg-white/5">
        <div class="p-4 rounded-xl bg-gradient-to-br from-primary-600/20 to-primary-500/10 border border-primary-500/20">
          <div class="flex items-center space-x-3 mb-3">
            <div class="w-10 h-10 rounded-xl bg-primary-500/30 flex items-center justify-center">
              <svg class="w-5 h-5 text-primary-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"></path>
              </svg>
            </div>
            <div>
              <p class="text-sm font-medium text-white/90">Pro Features</p>
              <p class="text-xs text-white/50">Unlock more</p>
            </div>
          </div>
          <button class="w-full py-2 px-4 rounded-lg bg-primary-600/50 hover:bg-primary-600/70 text-white text-sm font-medium transition-all duration-300 hover:shadow-lg hover:shadow-primary-500/20">
            Upgrade Now
          </button>
        </div>
      </div>
    </aside>
  `,
  styles: [`
    .active-nav-item {
      background: linear-gradient(90deg, rgba(139, 92, 246, 0.15) 0%, rgba(139, 92, 246, 0.05) 100%);
      color: white;
      border-left: 2px solid rgb(139, 92, 246);
    }

    .active-nav-item .absolute.left-0 {
      opacity: 1 !important;
    }

    .active-nav-item svg {
      color: rgb(167, 139, 250);
    }

    .active-nav-item > div:first-of-type {
      opacity: 1;
    }
  `]
})
export class SidebarComponent {
  private authState = inject(AuthState);
  isCollapsed = signal(false);

  navItems: NavItem[] = [
    {
      label: 'Dashboard',
      route: '/dashboard',
      icon: 'M4 5a1 1 0 011-1h14a1 1 0 011 1v2a1 1 0 01-1 1H5a1 1 0 01-1-1V5zM4 13a1 1 0 011-1h6a1 1 0 011 1v6a1 1 0 01-1 1H5a1 1 0 01-1-1v-6zM16 13a1 1 0 011-1h2a1 1 0 011 1v6a1 1 0 01-1 1h-2a1 1 0 01-1-1v-6z'
    },
    {
      label: 'Tickets',
      route: '/tickets',
      icon: 'M15 5v2m0 4v2m0 4v2M5 5a2 2 0 00-2 2v3a2 2 0 110 4v3a2 2 0 002 2h14a2 2 0 002-2v-3a2 2 0 110-4V7a2 2 0 00-2-2H5z',
      badge: 3
    },
    {
      label: 'Users',
      route: '/users',
      icon: 'M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z',
      roles: ['Admin']
    },
    {
      label: 'Profile',
      route: '/profile',
      icon: 'M5.121 17.804A13.937 13.937 0 0112 16c2.5 0 4.847.655 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0zm6 2a9 9 0 11-18 0 9 9 0 0118 0z'
    }
  ];

  filteredNavItems = computed(() => {
    const userRole = this.authState.userRole();
    if (!userRole) return [];

    return this.navItems.filter(item => {
      if (!item.roles) return true;
      return item.roles.includes(userRole);
    });
  });
}
