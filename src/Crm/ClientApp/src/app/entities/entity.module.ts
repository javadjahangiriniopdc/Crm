import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'contact',
        loadChildren: () => import('./contact/contact.module').then(m => m.CrmContactModule),
      },
      {
        path: 'sazeman',
        loadChildren: () => import('./sazeman/sazeman.module').then(m => m.CrmSazemanModule),
      },
      {
        path: 'semate-sazeman',
        loadChildren: () => import('./semate-sazeman/semate-sazeman.module').then(m => m.CrmSemateSazemanModule),
      },
      /* jhipster-needle-add-entity-route - JHipster will add entity modules routes here */
    ]),
  ],
})
export class CrmEntityModule {}
