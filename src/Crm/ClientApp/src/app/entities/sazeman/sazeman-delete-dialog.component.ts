import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { JhiEventManager } from 'ng-jhipster';

import { ISazeman } from 'app/shared/model/sazeman.model';
import { SazemanService } from './sazeman.service';

@Component({
  templateUrl: './sazeman-delete-dialog.component.html',
})
export class SazemanDeleteDialogComponent {
  sazeman?: ISazeman;

  constructor(protected sazemanService: SazemanService, public activeModal: NgbActiveModal, protected eventManager: JhiEventManager) {}

  cancel(): void {
    this.activeModal.dismiss();
  }

  confirmDelete(id: number): void {
    this.sazemanService.delete(id).subscribe(() => {
      this.eventManager.broadcast('sazemanListModification');
      this.activeModal.close();
    });
  }
}
