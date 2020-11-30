import { Component, OnInit } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { ISemateSazeman, SemateSazeman } from 'app/shared/model/semate-sazeman.model';
import { SemateSazemanService } from './semate-sazeman.service';
import { ISazeman } from 'app/shared/model/sazeman.model';
import { SazemanService } from 'app/entities/sazeman/sazeman.service';

@Component({
  selector: 'jhi-semate-sazeman-update',
  templateUrl: './semate-sazeman-update.component.html',
})
export class SemateSazemanUpdateComponent implements OnInit {
  isSaving = false;
  sazemen: ISazeman[] = [];

  editForm = this.fb.group({
    id: [],
    semateSazemanName: [null, [Validators.required]],
    sazeman: [],
  });

  constructor(
    protected semateSazemanService: SemateSazemanService,
    protected sazemanService: SazemanService,
    protected activatedRoute: ActivatedRoute,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ semateSazeman }) => {
      this.updateForm(semateSazeman);

      this.sazemanService.query().subscribe((res: HttpResponse<ISazeman[]>) => (this.sazemen = res.body || []));
    });
  }

  updateForm(semateSazeman: ISemateSazeman): void {
    this.editForm.patchValue({
      id: semateSazeman.id,
      semateSazemanName: semateSazeman.semateSazemanName,
      sazeman: semateSazeman.sazeman,
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const semateSazeman = this.createFromForm();
    if (semateSazeman.id !== undefined) {
      this.subscribeToSaveResponse(this.semateSazemanService.update(semateSazeman));
    } else {
      this.subscribeToSaveResponse(this.semateSazemanService.create(semateSazeman));
    }
  }

  private createFromForm(): ISemateSazeman {
    return {
      ...new SemateSazeman(),
      id: this.editForm.get(['id'])!.value,
      semateSazemanName: this.editForm.get(['semateSazemanName'])!.value,
      sazeman: this.editForm.get(['sazeman'])!.value,
    };
  }

  protected subscribeToSaveResponse(result: Observable<HttpResponse<ISemateSazeman>>): void {
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

  trackById(index: number, item: ISazeman): any {
    return item.id;
  }
}
