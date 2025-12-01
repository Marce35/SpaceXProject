import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment.development";
import {HttpClient, HttpContext} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {SKIP_ERROR_HANDLING} from "../core/http-context.tokens";

@Injectable({
  providedIn: 'root',
})
export class ApiHealthCheckService {
  private apiUrl = environment.apiUrl.replace('/api', '');

  private http = inject(HttpClient);

  public async checkApiHealth(): Promise<boolean> {
    try{
      await firstValueFrom(
        this.http.get(`${this.apiUrl}/health`, {
          responseType: "text",
          context: new HttpContext().set(SKIP_ERROR_HANDLING, true)
        })
      );
      return true;
    } catch {
      return false;
    }
  }
}
