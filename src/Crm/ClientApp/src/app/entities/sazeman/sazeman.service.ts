import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SERVER_API_URL } from 'app/app.constants';
import { createRequestOption } from 'app/shared/util/request-util';
import { ISazeman } from 'app/shared/model/sazeman.model';

type EntityResponseType = HttpResponse<ISazeman>;
type EntityArrayResponseType = HttpResponse<ISazeman[]>;

@Injectable({ providedIn: 'root' })
export class SazemanService {
  public resourceUrl = SERVER_API_URL + 'api/sazemen';

  constructor(protected http: HttpClient) {}

  create(sazeman: ISazeman): Observable<EntityResponseType> {
    return this.http.post<ISazeman>(this.resourceUrl, sazeman, { observe: 'response' });
  }

  update(sazeman: ISazeman): Observable<EntityResponseType> {
    return this.http.put<ISazeman>(this.resourceUrl, sazeman, { observe: 'response' });
  }

  find(id: number): Observable<EntityResponseType> {
    return this.http.get<ISazeman>(`${this.resourceUrl}/${id}`, { observe: 'response' });
  }

  query(req?: any): Observable<EntityArrayResponseType> {
    const options = createRequestOption(req);
    return this.http.get<ISazeman[]>(this.resourceUrl, { params: options, observe: 'response' });
  }

  delete(id: number): Observable<HttpResponse<{}>> {
    return this.http.delete(`${this.resourceUrl}/${id}`, { observe: 'response' });
  }
}
