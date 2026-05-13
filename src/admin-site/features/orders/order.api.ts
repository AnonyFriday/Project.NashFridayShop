import { baseApiSlice } from "@/lib/api/base.api";
import { GetOrders, GetOrderById } from "./order.types";

export const orderApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getOrders: builder.query<GetOrders.Response, GetOrders.Request>({
            query: (params) => ({
                url: "admin/orders",
                params,
            }),
            providesTags: ['Order'],
        }),
        getOrderById: builder.query<GetOrderById.Response, string>({
            query: (id) => ({
                url: `admin/orders/${id}`,
            }),
            providesTags: ['Order'],
        }),
    }),
    overrideExisting: false,
});

export const {
    useGetOrdersQuery,
    useLazyGetOrdersQuery,
    useGetOrderByIdQuery,
    useLazyGetOrderByIdQuery,
} = orderApiSlice;
