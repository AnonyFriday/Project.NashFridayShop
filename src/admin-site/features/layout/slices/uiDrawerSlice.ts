import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface UiDrawerState {
  isSidebarOpen: boolean;
}

const initialState: UiDrawerState = {
  isSidebarOpen: false,
};

export const uiDrawerSlice = createSlice({
  name: "uiDrawer",
  initialState,
  reducers: {
    toggleSidebar: (state) => {
      state.isSidebarOpen = !state.isSidebarOpen;
    },
    setSidebarOpen: (state, action: PayloadAction<boolean>) => {
      state.isSidebarOpen = action.payload;
    },
  },
});

export const { toggleSidebar, setSidebarOpen } = uiDrawerSlice.actions;
export default uiDrawerSlice.reducer;
