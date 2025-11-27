import { Routes } from '@angular/router';

export const usersRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./containers/users.container').then(m => m.UsersContainer),
    children: [
      // TODO: Create missing components
      // {
      //   path: '',
      //   loadComponent: () => import('./components/user-list.component').then(m => m.UserListComponent)
      // },
      // {
      //   path: 'new',
      //   loadComponent: () => import('./components/user-form.component').then(m => m.UserFormComponent)
      // },
      // {
      //   path: ':id',
      //   loadComponent: () => import('./components/user-detail.component').then(m => m.UserDetailComponent)
      // },
      // {
      //   path: ':id/edit',
      //   loadComponent: () => import('./components/user-form.component').then(m => m.UserFormComponent)
      // },
      // {
      //   path: 'profile',
      //   loadComponent: () => import('./components/profile.component').then(m => m.ProfileComponent)
      // }
    ]
  }
];
