import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { UserInfo } from "./auth.types";

interface AuthState {
  user: UserInfo | null;
}

const initialState: AuthState = {
  user: null,
};

export const authSlice = createSlice({
  name: "authSlice",
  initialState,
  reducers: {
    setUser: (state, action: PayloadAction<UserInfo | null>) => {
      state.user = action.payload;
    },
    logout: (state) => {
      state.user = null;
    },
  },
});

export const { setUser, logout } = authSlice.actions;
export default authSlice.reducer;
