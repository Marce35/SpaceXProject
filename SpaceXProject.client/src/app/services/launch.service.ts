import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Result} from "../data/models/result-pattern/result";
import {toPromise} from "../core/api-helper";
import {LaunchQueryRequest} from "../data/requests/launch-query";
import {PagedResponse} from "../data/responses/core/paged-response";
import {SpaceXLaunch} from "../data/models/spacex-launch";

@Injectable({
  providedIn: 'root',
})
export class LaunchService {
  private apiUrl = 'https://localhost:7200/api/launches/';

  private http = inject(HttpClient);


  async getLaunches(req: LaunchQueryRequest): Promise<Result<PagedResponse<SpaceXLaunch>>> {
    const queryParams: any = {
      page: req.page,
      limit: req.limit,
      type: req.type,
      sort: req.sort,
    }

    if(req.search && req.search.trim()){
      queryParams.search = req.search.trim()
    }
    const params = new HttpParams({fromObject: queryParams});

    const call = this.http.get<Result<PagedResponse<SpaceXLaunch>>>(this.apiUrl, { params: params });
    return await toPromise(call);
  }
}
