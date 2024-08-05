import { createReducer, on } from "@ngrx/store";
import { InitialUserStateModel } from "./user-state.model";
import { updateUserInfo } from "./user.actions";

export const userFeatureKey = 'user';

export const userReducer = createReducer(
    InitialUserStateModel,
    on(updateUserInfo, (state, user) => {
        return { ...state, Username: user.Username };
    }),
  );