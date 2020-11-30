import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { CrmSharedModule } from 'app/shared/shared.module';
import { SazemanComponent } from './sazeman.component';
import { SazemanDetailComponent } from './sazeman-detail.component';
import { SazemanUpdateComponent } from './sazeman-update.component';
import { SazemanDeleteDialogComponent } from './sazeman-delete-dialog.component';
import { sazemanRoute } from './sazeman.route';

@NgModule({
  imports: [CrmSharedModule, RouterModule.forChild(sazemanRoute)],
  declarations: [SazemanComponent, SazemanDetailComponent, SazemanUpdateComponent, SazemanDeleteDialogComponent],
  entryComponents: [SazemanDeleteDialogComponent],
})
export class CrmSazemanModule {}
