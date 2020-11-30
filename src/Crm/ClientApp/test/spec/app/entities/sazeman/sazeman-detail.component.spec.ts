import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { CrmTestModule } from '../../../test.module';
import { SazemanDetailComponent } from 'app/entities/sazeman/sazeman-detail.component';
import { Sazeman } from 'app/shared/model/sazeman.model';

describe('Component Tests', () => {
  describe('Sazeman Management Detail Component', () => {
    let comp: SazemanDetailComponent;
    let fixture: ComponentFixture<SazemanDetailComponent>;
    const route = ({ data: of({ sazeman: new Sazeman(123) }) } as any) as ActivatedRoute;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [CrmTestModule],
        declarations: [SazemanDetailComponent],
        providers: [{ provide: ActivatedRoute, useValue: route }],
      })
        .overrideTemplate(SazemanDetailComponent, '')
        .compileComponents();
      fixture = TestBed.createComponent(SazemanDetailComponent);
      comp = fixture.componentInstance;
    });

    describe('OnInit', () => {
      it('Should load sazeman on init', () => {
        // WHEN
        comp.ngOnInit();

        // THEN
        expect(comp.sazeman).toEqual(jasmine.objectContaining({ id: 123 }));
      });
    });
  });
});
