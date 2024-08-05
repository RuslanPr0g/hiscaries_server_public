import { createSelector } from "@ngrx/store";
import { UserStateModel } from "./user-state.model";

export const username = (state: UserStateModel) => state.Username;

export const selectUsername = createSelector(
    username,
    (state: string | undefined) => state
);