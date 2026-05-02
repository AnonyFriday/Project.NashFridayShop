import { baseApiSlice } from "@/lib/api/base.api";
import { GetProducts, GetProductById } from "./product.types";

export const productApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getProducts: builder.query<GetProducts.Response, GetProducts.Request>({
            query: (params) => ({
                url: "products",
                params,
            }),
            providesTags: ['Product'],
        }),
        getProductById: builder.query<GetProductById.Response, GetProductById.Request>({
            query: ({ id, includeDeleted = false }) => ({
                url: `products/${id}`,
                params: { IncludeDeleted: includeDeleted },
            }),
            providesTags: ['Product']
        }),
    }),
    overrideExisting: false,
});

export const {
    useGetProductsQuery,
    useLazyGetProductsQuery,
    useGetProductByIdQuery,
    useLazyGetProductByIdQuery
} = productApiSlice;
