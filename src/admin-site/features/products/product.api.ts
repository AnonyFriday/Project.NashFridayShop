import { baseApiSlice } from "@/lib/api/base.api";
import { GetProducts } from "./product.types";

export const productApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getProducts: builder.query<GetProducts.Response, GetProducts.Request>({
            query: (params) => ({
                url: "products",
                params,
            }),
            providesTags: ['Product'],
        }),
    }),
    overrideExisting: false,
});

export const { useGetProductsQuery, useLazyGetProductsQuery } = productApiSlice;
