import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HttpResponse } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { of } from 'rxjs';

import { CrmTestModule } from '../../../test.module';
import { SazemanUpdateComponent } from 'app/entities/sazeman/sazeman-update.component';
import { SazemanService } from 'app/entities/sazeman/sazeman.service';
import { Sazeman } from 'app/shared/model/sazeman.model';

describe('Component Tests', () => {
  describe('Sazeman Management Update Component', () => {
    let comp: SazemanUpdateComponent;
    let fixture: ComponentFixture<SazemanUpdateComponent>;
    let service: SazemanService;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [CrmTestModule],
        declarations: [SazemanUpdateComponent],
        providers: [FormBuilder],
      })
        .overrideTemplate(SazemanUpdateComponent, '')
        .compileComponents();

      fixture = TestBed.createComponent(SazemanUpdateComponent);
      comp = fixture.componentInstance;
      service = fixture.debugElement.injector.get(SazemanService);
    });

    describe('save', () => {
      it('Should call update service on save for existing entity', fakeAsync(() => {
        // GIVEN
        const entity = new Sazeman(123);
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
        const entity = new Sazeman();
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
