import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthState } from '../../../../core/authentication/state/auth.state';

interface NavItem {
  label: string;
  route: string;
  icon: string;
  roles?: string[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="bg-gray-50 h-full w-64 border-r border-gray-200">
      <nav class="mt-8 px-4">
        <ul class="space-y-2">
          <li *ngFor="let item of filteredNavItems()">
            <a
              [routerLink]="item.route"
              routerLinkActive="bg-blue-100 text-blue-700"
              class="flex items-center px-4 py-3 text-sm font-medium text-gray-700 rounded-lg hover:bg-gray-100 hover:text-gray-900 transition-colors duration-200"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path [attr.d]="item.icon" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
              </svg>
              {{ item.label }}
            </a>
          </li>
        </ul>
      </nav>
    </div>
  `,
  styles: []
})
export class SidebarComponent {
  private authState = inject(AuthState);

  navItems: NavItem[] = [
    {
      label: 'Dashboard',
      route: '/dashboard',
      icon: 'M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2H5a2 2 0 00-2-2z M8 5a2 2 0 012-2h4a2 2 0 012 2v2H8V5z'
    },
    {
      label: 'Tickets',
      route: '/tickets',
      icon: 'M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z'
    },
    {
      label: 'Users',
      route: '/users',
      icon: 'M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z',
      roles: ['Admin']
    },
    {
      label: 'Profile',
      route: '/profile',
      icon: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z'
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
