import { baseApiSlice } from "@/lib/api/base.api";
import { GetCustomers } from "./customer.types";
import { APP_ROUTES } from "@/lib/api/routes";

export const customerApi = baseApiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getCustomers: builder.query<GetCustomers.Response, GetCustomers.Request>({
      query: (params) => ({
        url: "admin/customers",
        params,
      }),
      providesTags: ["Customer"],
    }),
  }),
});

export const { useGetCustomersQuery } = customerApi;
