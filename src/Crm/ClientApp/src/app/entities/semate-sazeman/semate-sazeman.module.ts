import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CrmSharedModule } from 'app/shared/shared.module';
import { SemateSazemanComponent } from './semate-sazeman.component';
import { SemateSazemanDetailComponent } from './semate-sazeman-detail.component';
import { SemateSazemanUpdateComponent } from './semate-sazeman-update.component';
import { SemateSazemanDeleteDialogComponent } from './semate-sazeman-delete-dialog.component';
import { semateSazemanRoute } from './semate-sazeman.route';

@NgModule({
  imports: [CrmSharedModule, RouterModule.forChild(semateSazemanRoute)],
  declarations: [SemateSazemanComponent, SemateSazemanDetailComponent, SemateSazemanUpdateComponent, SemateSazemanDeleteDialogComponent],
  entryComponents: [SemateSazemanDeleteDialogComponent],
})
export class CrmSemateSazemanModule {}
