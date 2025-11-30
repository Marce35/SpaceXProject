export interface ApiError{
  code: string;
  messages: string[];
}

export enum ResultStatus{
  Success = 'Success',
  Failure = 'Failure',
  UnAuthorized = 'UnAuthorized',
  EmailAlreadyExists = 'EmailAlreadyExists',
  ValidationFailed = 'ValidationFailed',
  NotFound = 'NotFound',
  Exception = 'Exception'
}

export interface Result<T = void>{
  isSuccess: boolean;
  status: ResultStatus;
  value?: T;
  error?: ApiError;
}
