import { Routes } from '@angular/router';

export const ticketsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./containers/tickets.container').then(m => m.TicketsContainer),
    children: [
      {
        path: '',
        loadComponent: () => import('./components/ticket-list.component').then(m => m.TicketListComponent)
      }
      // TODO: Create missing components
      // {
      //   path: 'new',
      //   loadComponent: () => import('./components/ticket-form.component').then(m => m.TicketFormComponent)
      // },
      // {
      //   path: ':id',
      //   loadComponent: () => import('./components/ticket-detail.component').then(m => m.TicketDetailComponent)
      // },
      // {
      //   path: ':id/edit',
      //   loadComponent: () => import('./components/ticket-form.component').then(m => m.TicketFormComponent)
      // }
    ]
  }
];
