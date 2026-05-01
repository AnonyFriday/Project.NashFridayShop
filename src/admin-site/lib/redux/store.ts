import { configureStore } from '@reduxjs/toolkit';
import { baseApiSlice } from '../api/baseApiSlice';

export const makeStore = () => {
  return configureStore({
    reducer: {
      [baseApiSlice.reducerPath]: baseApiSlice.reducer,
    },
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat(baseApiSlice.middleware),
  });
};

// Infer the type of makeStore
export type AppStore = ReturnType<typeof makeStore>;
// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<AppStore['getState']>;
export type AppDispatch = AppStore['dispatch'];
