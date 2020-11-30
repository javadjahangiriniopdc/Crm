import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SERVER_API_URL } from 'app/app.constants';
import { createRequestOption } from 'app/shared/util/request-util';
import { ISemateSazeman } from 'app/shared/model/semate-sazeman.model';

type EntityResponseType = HttpResponse<ISemateSazeman>;
type EntityArrayResponseType = HttpResponse<ISemateSazeman[]>;

@Injectable({ providedIn: 'root' })
export class SemateSazemanService {
  public resourceUrl = SERVER_API_URL + 'api/semate-sazemen';

  constructor(protected http: HttpClient) {}

  create(semateSazeman: ISemateSazeman): Observable<EntityResponseType> {
    return this.http.post<ISemateSazeman>(this.resourceUrl, semateSazeman, { observe: 'response' });
  }

  update(semateSazeman: ISemateSazeman): Observable<EntityResponseType> {
    return this.http.put<ISemateSazeman>(this.resourceUrl, semateSazeman, { observe: 'response' });
  }

  find(id: number): Observable<EntityResponseType> {
    return this.http.get<ISemateSazeman>(`${this.resourceUrl}/${id}`, { observe: 'response' });
  }

  query(req?: any): Observable<EntityArrayResponseType> {
    const options = createRequestOption(req);
    return this.http.get<ISemateSazeman[]>(this.resourceUrl, { params: options, observe: 'response' });
  }

  delete(id: number): Observable<HttpResponse<{}>> {
    return this.http.delete(`${this.resourceUrl}/${id}`, { observe: 'response' });
  }
}
