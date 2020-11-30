import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, Routes, Router } from '@angular/router';
import { Observable, of, EMPTY } from 'rxjs';
import { flatMap } from 'rxjs/operators';

import { Authority } from 'app/shared/constants/authority.constants';
import { UserRouteAccessService } from 'app/core/auth/user-route-access-service';
import { ISazeman, Sazeman } from 'app/shared/model/sazeman.model';
import { SazemanService } from './sazeman.service';
import { SazemanComponent } from './sazeman.component';
import { SazemanDetailComponent } from './sazeman-detail.component';
import { SazemanUpdateComponent } from './sazeman-update.component';

@Injectable({ providedIn: 'root' })
export class SazemanResolve implements Resolve<ISazeman> {
  constructor(private service: SazemanService, private router: Router) {}

  resolve(route: ActivatedRouteSnapshot): Observable<ISazeman> | Observable<never> {
    const id = route.params['id'];
    if (id) {
      return this.service.find(id).pipe(
        flatMap((sazeman: HttpResponse<Sazeman>) => {
          if (sazeman.body) {
            return of(sazeman.body);
          } else {
            this.router.navigate(['404']);
            return EMPTY;
          }
        })
      );
    }
    return of(new Sazeman());
  }
}

export const sazemanRoute: Routes = [
  {
    path: '',
    component: SazemanComponent,
    data: {
      authorities: [Authority.USER],
      defaultSort: 'id,asc',
      pageTitle: 'crmApp.sazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ':id/view',
    component: SazemanDetailComponent,
    resolve: {
      sazeman: SazemanResolve,
    },
    data: {
      authorities: [Authority.USER],
      pageTitle: 'crmApp.sazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: 'new',
    component: SazemanUpdateComponent,
    resolve: {
      sazeman: SazemanResolve,
    },
    data: {
      authorities: [Authority.USER],
      pageTitle: 'crmApp.sazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ':id/edit',
    component: SazemanUpdateComponent,
    resolve: {
      sazeman: SazemanResolve,
    },
    data: {
      authorities: [Authority.USER],
      pageTitle: 'crmApp.sazeman.home.title',
    },
    canActivate: [UserRouteAccessService],
  },
];
