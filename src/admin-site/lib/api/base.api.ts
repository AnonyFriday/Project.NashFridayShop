import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { ENV_CONFIGS } from '../config/env';

export const baseApiSlice = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: ENV_CONFIGS.bffUrl,
    credentials: 'include', // send bff cookies
  }),
  tagTypes: ['Category', 'Product', 'Order', 'Customer', 'User'],
  endpoints: () => ({
  }),
});
