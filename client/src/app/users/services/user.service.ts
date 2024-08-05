import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";

@Injectable({
    providedIn: 'root',
  })
  export class UserService {
    register(value: any) : Observable<any>  {
      return of(value);
    }
  
    login(value: any) : Observable<any> {
      return of(value);
    }
  
    isAuthenticated() : boolean {
      return false;
    }
  }