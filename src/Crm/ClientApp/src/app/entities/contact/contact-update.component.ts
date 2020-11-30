import { Component, OnInit, Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { JhiDataUtils, JhiFileLoadError, JhiEventManager, JhiEventWithContent } from 'ng-jhipster';

import { IContact, Contact } from 'app/shared/model/contact.model';
import { ContactService } from './contact.service';
import { AlertError } from 'app/shared/alert/alert-error.model';
import { ISazeman } from 'app/shared/model/sazeman.model';
import { SazemanService } from 'app/entities/sazeman/sazeman.service';
import { ISemateSazeman } from 'app/shared/model/semate-sazeman.model';
import { SemateSazemanService } from 'app/entities/semate-sazeman/semate-sazeman.service';
import { NgbDateStruct, NgbCalendar, NgbDatepickerI18n, NgbCalendarPersian } from '@ng-bootstrap/ng-bootstrap';

type SelectableEntity = ISazeman | ISemateSazeman;

const WEEKDAYS_SHORT = ['د', 'س', 'چ', 'پ', 'ج', 'ش', 'ی'];
const MONTHS = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'];

@Injectable()
export class NgbDatepickerI18nPersian extends NgbDatepickerI18n {
    getWeekdayShortName(weekday: number): any {
        return WEEKDAYS_SHORT[weekday - 1];
    }
    getMonthShortName(month: number): any {
        return MONTHS[month - 1];
    }
    getMonthFullName(month: number): any {
        return MONTHS[month - 1];
    }
    getDayAriaLabel(date: NgbDateStruct): string {
        return `${date.year}-${this.getMonthFullName(date.month)}-${date.day}`;
    }
}



@Component({
  selector: 'jhi-contact-update',
    templateUrl: './contact-update.component.html',
  providers: [
      { provide: NgbCalendar, useClass: NgbCalendarPersian },
      { provide: NgbDatepickerI18n, useClass: NgbDatepickerI18nPersian },
  ],
})
export class ContactUpdateComponent implements OnInit {
  isSaving = false;
  sazemen: ISazeman[] = [];
  sematesazemen: ISemateSazeman[] = [];
  birthDateDp: any;

  editForm = this.fb.group({
    id: [],
    personCode: [null, [Validators.required]],
    contactName: [null, [Validators.required, Validators.maxLength(100)]],
    birthDate: [],
    description: [],
    attachFile: [],
    attachFileContentType: [],
    sazeman: [],
    semateSazeman: [],
  });

  constructor(
    protected dataUtils: JhiDataUtils,
    protected eventManager: JhiEventManager,
    protected contactService: ContactService,
    protected sazemanService: SazemanService,
    protected semateSazemanService: SemateSazemanService,
    protected activatedRoute: ActivatedRoute,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ contact }) => {
      this.updateForm(contact);

      this.sazemanService.query().subscribe((res: HttpResponse<ISazeman[]>) => (this.sazemen = res.body || []));

      this.semateSazemanService.query().subscribe((res: HttpResponse<ISemateSazeman[]>) => (this.sematesazemen = res.body || []));
    });
  }

  updateForm(contact: IContact): void {
    this.editForm.patchValue({
      id: contact.id,
      personCode: contact.personCode,
      contactName: contact.contactName,
      birthDate: contact.birthDate,
      description: contact.description,
      attachFile: contact.attachFile,
      attachFileContentType: contact.attachFileContentType,
      sazeman: contact.sazeman,
      semateSazeman: contact.semateSazeman,
    });
  }

  byteSize(base64String: string): string {
    return this.dataUtils.byteSize(base64String);
  }

  openFile(contentType: string, base64String: string): void {
    this.dataUtils.openFile(contentType, base64String);
  }

  setFileData(event: any, field: string, isImage: boolean): void {
    this.dataUtils.loadFileToForm(event, this.editForm, field, isImage).subscribe(null, (err: JhiFileLoadError) => {
      this.eventManager.broadcast(
        new JhiEventWithContent<AlertError>('crmApp.error', { ...err, key: 'error.file.' + err.key })
      );
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const contact = this.createFromForm();
    if (contact.id !== undefined) {
      this.subscribeToSaveResponse(this.contactService.update(contact));
    } else {
      this.subscribeToSaveResponse(this.contactService.create(contact));
    }
  }

  private createFromForm(): IContact {
    return {
      ...new Contact(),
      id: this.editForm.get(['id'])!.value,
      personCode: this.editForm.get(['personCode'])!.value,
      contactName: this.editForm.get(['contactName'])!.value,
      birthDate: this.editForm.get(['birthDate'])!.value,
      description: this.editForm.get(['description'])!.value,
      attachFileContentType: this.editForm.get(['attachFileContentType'])!.value,
      attachFile: this.editForm.get(['attachFile'])!.value,
      sazeman: this.editForm.get(['sazeman'])!.value,
      semateSazeman: this.editForm.get(['semateSazeman'])!.value,
    };
  }

  protected subscribeToSaveResponse(result: Observable<HttpResponse<IContact>>): void {
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

  trackById(index: number, item: SelectableEntity): any {
    return item.id;
  }
}
