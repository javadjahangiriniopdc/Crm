import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HttpResponse } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { of } from 'rxjs';

import { CrmTestModule } from '../../../test.module';
import { SemateSazemanUpdateComponent } from 'app/entities/semate-sazeman/semate-sazeman-update.component';
import { SemateSazemanService } from 'app/entities/semate-sazeman/semate-sazeman.service';
import { SemateSazeman } from 'app/shared/model/semate-sazeman.model';

describe('Component Tests', () => {
  describe('SemateSazeman Management Update Component', () => {
    let comp: SemateSazemanUpdateComponent;
    let fixture: ComponentFixture<SemateSazemanUpdateComponent>;
    let service: SemateSazemanService;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [CrmTestModule],
        declarations: [SemateSazemanUpdateComponent],
        providers: [FormBuilder],
      })
        .overrideTemplate(SemateSazemanUpdateComponent, '')
        .compileComponents();

      fixture = TestBed.createComponent(SemateSazemanUpdateComponent);
      comp = fixture.componentInstance;
      service = fixture.debugElement.injector.get(SemateSazemanService);
    });

    describe('save', () => {
      it('Should call update service on save for existing entity', fakeAsync(() => {
        // GIVEN
        const entity = new SemateSazeman(123);
        spyOn(service, 'update').and.returnValue(of(new HttpResponse({ body: entity })));
        comp.updateForm(entity);
        // WHEN
        comp.save();
        tick(); // simulate async

        // THEN
        expect(service.update).toHaveBeenCalledWith(entity);
        expect(comp.isSaving).toEqual(false);
      }));

      it('Should call create service on save for new entity', fakeAsync(() => {
        // GIVEN
        const entity = new SemateSazeman();
        spyOn(service, 'create').and.returnValue(of(new HttpResponse({ body: entity })));
        comp.updateForm(entity);
        // WHEN
        comp.save();
        tick(); // simulate async

        // THEN
        expect(service.create).toHaveBeenCalledWith(entity);
        expect(comp.isSaving).toEqual(false);
      }));
    });
  });
});
