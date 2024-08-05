import { createAction, props } from "@ngrx/store";

export const updateUserInfo = createAction(
    '[User] Update user data',
    props<{ Username: string; }>()
  );
