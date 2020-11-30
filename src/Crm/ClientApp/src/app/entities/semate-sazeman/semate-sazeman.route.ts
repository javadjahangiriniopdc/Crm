import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, Routes, Router } from '@angular/router';
import { Observable, of, EMPTY } from 'rxjs';
import { flatMap } from 'rxjs/operators';

import { Authority } from 'app/shared/constants/authority.constants';
import { UserRouteAccessService } from 'app/core/auth/user-route-access-service';
import { ISemateSazeman, SemateSazeman } from 'app/shared/model/semate-sazeman.model';
import { SemateSazemanService } from './semate-sazeman.service';
import { SemateSazemanComponent } from './semate-sazeman.component';
import { SemateSazemanDetailComponent } from './semate-sazeman-detail.component';
import { SemateSazemanUpdateComponent } from './semate-sazeman-update.component';

@Injectable({ providedIn: 'root' })
export class SemateSazemanResolve implements Resolve<ISemateSazeman> {
  constructor(private service: SemateSazemanService, private router: Router) {}

  resolve(route: ActivatedRouteSnapshot): Observable<ISemateSazeman> | Observable<never> {
    const id = route.params['id'];
    if (id) {
      return this.service.find(id).pipe(
        flatMap((semateSazeman: HttpResponse<SemateSazeman>) => {
          if (semateSazeman.body) {
            return of(semateSazeman.body);
          } else {
            this.router.navigate(['404']);
            return EMPTY;
          }
        })
      );
    }
    return of(new SemateSazeman());
  }
}

export const semateSazemanRoute: Routes = [
  {
    path: '',
    component: SemateSazemanComponent,
    data: {
      authorities: [Authority.USER],
      defaultSort: 'id,asc',
      pageTitle: 'crmApp.semateSazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ':id/view',
    component: SemateSazemanDetailComponent,
    resolve: {
      semateSazeman: SemateSazemanResolve,
    },
    data: {
      authorities: [Authority.USER],
      pageTitle: 'crmApp.semateSazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: 'new',
    component: SemateSazemanUpdateComponent,
    resolve: {
      semateSazeman: SemateSazemanResolve,
    },
    data: {
      authorities: [Authority.USER],
      pageTitle: 'crmApp.semateSazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ':id/edit',
    component: SemateSazemanUpdateComponent,
    resolve: {
      semateSazeman: SemateSazemanResolve,
    },
    data: {
      authorities: [Authority.USER],
      pageTitle: 'crmApp.semateSazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
];
