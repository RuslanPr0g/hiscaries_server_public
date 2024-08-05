import { inject } from "@angular/core";
import { CanActivateFn, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";

export const authGuard: CanActivateFn = (
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) => {
        if (localStorage.getItem('currentUser')) {
            return true;
        }

        inject(Router).navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
  }