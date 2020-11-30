import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { JhiEventManager } from 'ng-jhipster';

import { ISemateSazeman } from 'app/shared/model/semate-sazeman.model';
import { SemateSazemanService } from './semate-sazeman.service';

@Component({
  templateUrl: './semate-sazeman-delete-dialog.component.html',
})
export class SemateSazemanDeleteDialogComponent {
  semateSazeman?: ISemateSazeman;

  constructor(
    protected semateSazemanService: SemateSazemanService,
    public activeModal: NgbActiveModal,
    protected eventManager: JhiEventManager
  ) {}

  cancel(): void {
    this.activeModal.dismiss();
  }

  confirmDelete(id: number): void {
    this.semateSazemanService.delete(id).subscribe(() => {
      this.eventManager.broadcast('semateSazemanListModification');
      this.activeModal.close();
    });
  }
}
