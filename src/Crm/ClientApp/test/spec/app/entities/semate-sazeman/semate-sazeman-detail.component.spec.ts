import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { CrmTestModule } from '../../../test.module';
import { SemateSazemanDetailComponent } from 'app/entities/semate-sazeman/semate-sazeman-detail.component';
import { SemateSazeman } from 'app/shared/model/semate-sazeman.model';

describe('Component Tests', () => {
  describe('SemateSazeman Management Detail Component', () => {
    let comp: SemateSazemanDetailComponent;
    let fixture: ComponentFixture<SemateSazemanDetailComponent>;
    const route = ({ data: of({ semateSazeman: new SemateSazeman(123) }) } as any) as ActivatedRoute;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [CrmTestModule],
        declarations: [SemateSazemanDetailComponent],
        providers: [{ provide: ActivatedRoute, useValue: route }],
      })
        .overrideTemplate(SemateSazemanDetailComponent, '')
        .compileComponents();
      fixture = TestBed.createComponent(SemateSazemanDetailComponent);
      comp = fixture.componentInstance;
    });

    describe('OnInit', () => {
      it('Should load semateSazeman on init', () => {
        // WHEN
        comp.ngOnInit();

        // THEN
        expect(comp.semateSazeman).toEqual(jasmine.objectContaining({ id: 123 }));
      });
    });
  });
});
