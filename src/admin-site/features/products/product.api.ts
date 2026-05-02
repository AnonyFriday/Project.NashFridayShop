import { baseApiSlice } from "@/lib/api/base.api";
import { GetProducts, GetProductById, UpdateProduct } from "./product.types";

export const productApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getProducts: builder.query<GetProducts.Response, GetProducts.Request>({
            query: (params) => ({
                url: "products",
                params,
            }),

        }),
        getProductById: builder.query<GetProductById.Response, GetProductById.Request>({
            query: ({ id, includeDeleted = false }) => ({
                url: `products/${id}`,
                params: { IncludeDeleted: includeDeleted },
            }),

        }),
        updateProduct: builder.mutation<UpdateProduct.Response, UpdateProduct.Request>({
            query: ({ id, body }) => ({
                url: `products/${id}`,
                method: 'PUT',
                body,
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
    useUpdateProductMutation
} = productApiSlice;
