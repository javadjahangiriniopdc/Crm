import { Component, OnInit } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { ISazeman, Sazeman } from 'app/shared/model/sazeman.model';
import { SazemanService } from './sazeman.service';

@Component({
  selector: 'jhi-sazeman-update',
  templateUrl: './sazeman-update.component.html',
})
export class SazemanUpdateComponent implements OnInit {
  isSaving = false;

  editForm = this.fb.group({
    id: [],
    sazemanName: [null, [Validators.required]],
  });

  constructor(protected sazemanService: SazemanService, protected activatedRoute: ActivatedRoute, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ sazeman }) => {
      this.updateForm(sazeman);
    });
  }

  updateForm(sazeman: ISazeman): void {
    this.editForm.patchValue({
      id: sazeman.id,
      sazemanName: sazeman.sazemanName,
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const sazeman = this.createFromForm();
    if (sazeman.id !== undefined) {
      this.subscribeToSaveResponse(this.sazemanService.update(sazeman));
    } else {
      this.subscribeToSaveResponse(this.sazemanService.create(sazeman));
    }
  }

  private createFromForm(): ISazeman {
    return {
      ...new Sazeman(),
      id: this.editForm.get(['id'])!.value,
      sazemanName: this.editForm.get(['sazemanName'])!.value,
    };
  }

  protected subscribeToSaveResponse(result: Observable<HttpResponse<ISazeman>>): void {
    result.subscribe(
      () => this.onSaveSuccess(),
      () => this.onSaveError()
    );
  }

  protected onSaveSuccess(): void {
    this.isSaving = false;
    this.previousState();
  }

  protected onSaveError(): void {
    this.isSaving = false;
  }
}
