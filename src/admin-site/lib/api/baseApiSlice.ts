import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { envConfig } from '../config/env';

export const baseApiSlice = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: envConfig.bffUrl,
    credentials: 'include', // send bff cookies
  }),
  tagTypes: ['Category', 'Product', 'Order', 'Customer'],
  endpoints: (builder) => ({
  }),
});
