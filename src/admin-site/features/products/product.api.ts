import { baseApiSlice } from "@/lib/api/base.api";
import { GetProducts, GetProductById, UpdateProduct } from "./product.types";

export const productApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getProducts: builder.query<GetProducts.Response, GetProducts.Request>({
            query: (params) => ({
                url: "products",
                params,
            }),
            keepUnusedDataFor: 0,
        }),
        getProductById: builder.query<GetProductById.Response, GetProductById.Request>({
            query: ({ id, includeDeleted = false }) => ({
                url: `products/${id}`,
                params: { IncludeDeleted: includeDeleted },
            }),
            keepUnusedDataFor: 0
        }),
        updateProduct: builder.mutation<UpdateProduct.Response, UpdateProduct.Request>({
            query: ({ id, body, includeDeleted = false }) => ({
                url: `products/${id}`,
                method: 'PUT',
                params: { includeDeleted },
                body,
            }),
        }),
        deleteProduct: builder.mutation<void, string>({
            query: (id) => ({
                url: `products/${id}`,
                method: 'PATCH',
            }),
        }),
    }),
    overrideExisting: false,
});

export const {
    useGetProductsQuery,
    useLazyGetProductsQuery,
    useGetProductByIdQuery,
    useLazyGetProductByIdQuery,
    useUpdateProductMutation,
    useDeleteProductMutation
} = productApiSlice;
