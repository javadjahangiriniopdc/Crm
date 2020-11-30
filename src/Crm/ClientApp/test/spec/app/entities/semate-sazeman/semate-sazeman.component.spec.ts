import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { HttpHeaders, HttpResponse } from '@angular/common/http';
import { ActivatedRoute, convertToParamMap } from '@angular/router';

import { CrmTestModule } from '../../../test.module';
import { SemateSazemanComponent } from 'app/entities/semate-sazeman/semate-sazeman.component';
import { SemateSazemanService } from 'app/entities/semate-sazeman/semate-sazeman.service';
import { SemateSazeman } from 'app/shared/model/semate-sazeman.model';

describe('Component Tests', () => {
  describe('SemateSazeman Management Component', () => {
    let comp: SemateSazemanComponent;
    let fixture: ComponentFixture<SemateSazemanComponent>;
    let service: SemateSazemanService;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [CrmTestModule],
        declarations: [SemateSazemanComponent],
        providers: [
          {
            provide: ActivatedRoute,
            useValue: {
              data: of({
                defaultSort: 'id,asc',
              }),
              queryParamMap: of(
                convertToParamMap({
                  page: '1',
                  size: '1',
                  sort: 'id,desc',
                })
              ),
            },
          },
        ],
      })
        .overrideTemplate(SemateSazemanComponent, '')
        .compileComponents();

      fixture = TestBed.createComponent(SemateSazemanComponent);
      comp = fixture.componentInstance;
      service = fixture.debugElement.injector.get(SemateSazemanService);
    });

    it('Should call load all on init', () => {
      // GIVEN
      const headers = new HttpHeaders().append('link', 'link;link');
      spyOn(service, 'query').and.returnValue(
        of(
          new HttpResponse({
            body: [new SemateSazeman(123)],
            headers,
          })
        )
      );

      // WHEN
      comp.ngOnInit();

      // THEN
      expect(service.query).toHaveBeenCalled();
      expect(comp.semateSazemen && comp.semateSazemen[0]).toEqual(jasmine.objectContaining({ id: 123 }));
    });

    it('should load a page', () => {
      // GIVEN
      const headers = new HttpHeaders().append('link', 'link;link');
      spyOn(service, 'query').and.returnValue(
        of(
          new HttpResponse({
            body: [new SemateSazeman(123)],
            headers,
          })
        )
      );

      // WHEN
      comp.loadPage(1);

      // THEN
      expect(service.query).toHaveBeenCalled();
      expect(comp.semateSazemen && comp.semateSazemen[0]).toEqual(jasmine.objectContaining({ id: 123 }));
    });

    it('should calculate the sort attribute for an id', () => {
      // WHEN
      comp.ngOnInit();
      const result = comp.sort();

      // THEN
      expect(result).toEqual(['id,desc']);
    });

    it('should calculate the sort attribute for a non-id attribute', () => {
      // INIT
      comp.ngOnInit();

      // GIVEN
      comp.predicate = 'name';

      // WHEN
      const result = comp.sort();

      // THEN
      expect(result).toEqual(['name,desc', 'id']);
    });
  });
});
