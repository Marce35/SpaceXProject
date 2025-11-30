import {firstValueFrom, Observable} from "rxjs";
import {Result, ResultStatus} from "../data/models/result-pattern/result";

export async function toPromise<T>(observable: Observable<Result<T>>): Promise<Result<T>> {
  try{
    const response = await firstValueFrom(observable);
    return response;
  } catch(err : any){
    return{
      isSuccess: false,
      status: ResultStatus.Exception,
      error: {
        code: 'Network Error',
        messages: [err.message || 'Network error occured']
      }
    } as Result<T>
  }
}
