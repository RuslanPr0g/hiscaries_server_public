import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";

@Injectable({
    providedIn: 'root',
  })
  export class UserService {
    register(value: any) : Observable<any>  {
      return of(5);
    }
  
    login(value: any) : Observable<any> {
      return of(5);
    }
  
    isAuthenticated() : boolean {
      return false;
    }
  }