import { baseApiSlice } from "@/lib/api/base.api";
import { GetProducts, GetProductById, UpdateProduct, CreateProduct, UpdateProductImage } from "./product.types";

export const productApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getProducts: builder.query<GetProducts.Response, GetProducts.Request>({
            query: (params) => ({
                url: "admin/products",
                params,
            }),
            providesTags: ['Product'],
        }),
        getProductById: builder.query<GetProductById.Response, GetProductById.Request>({
            query: ({ id, includeDeleted = false }) => ({
                url: `admin/products/${id}`,
                params: { IncludeDeleted: includeDeleted },
            }),
            providesTags: ['Product'],
        }),
        createProduct: builder.mutation<CreateProduct.Response, CreateProduct.Request>({
            query: (body) => ({
                url: "admin/products",
                method: 'POST',
                body,
            }),
            invalidatesTags: ['Product'],
        }),
        updateProduct: builder.mutation<UpdateProduct.Response, UpdateProduct.Request>({
            query: ({ id, body, includeDeleted = false }) => ({
                url: `admin/products/${id}`,
                method: 'PUT',
                params: { includeDeleted },
                body,
            }),
            invalidatesTags: ['Product'],
        }),
        updateProductImage: builder.mutation<UpdateProductImage.Response, UpdateProductImage.Request>({
            query: ({ productId, imageFile, includeDeleted = false }) => {
                const formData = new FormData();
                formData.append('imageFile', imageFile);
                return {
                    url: `admin/products/${productId}/image`,
                    method: 'PATCH',
                    params: { includeDeleted },
                    body: formData,
                };
            },
            invalidatesTags: ['Product'],
        }),
        deleteProduct: builder.mutation<void, string>({
            query: (id) => ({
                url: `admin/products/${id}`,
                method: 'PATCH',
            }),
            invalidatesTags: ['Product'],
        }),
    }),
    overrideExisting: false,
});

export const {
    useGetProductsQuery,
    useLazyGetProductsQuery,
    useGetProductByIdQuery,
    useLazyGetProductByIdQuery,
    useCreateProductMutation,
    useUpdateProductMutation,
    useUpdateProductImageMutation,
    useDeleteProductMutation
} = productApiSlice;
