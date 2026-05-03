import { baseApiSlice } from "@/lib/api/base.api";
import { GetCategories, GetCategoryById, CreateCategory, UpdateCategory } from "./category.types";

export const categoryApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getCategories: builder.query<GetCategories.Response, GetCategories.Request>({
            query: (params) => ({
                url: "categories",
                params
            }),
            providesTags: ['Category'],
        }),
        getCategoryById: builder.query<GetCategoryById.Response, string>({
            query: (id) => ({
                url: `categories/${id}`,
            }),
            providesTags: ['Category'],
        }),
        createCategory: builder.mutation<CreateCategory.Response, CreateCategory.Request>({
            query: (body) => ({
                url: "categories",
                method: 'POST',
                body,
            }),
            invalidatesTags: ['Category'],
        }),
        updateCategory: builder.mutation<UpdateCategory.Response, UpdateCategory.Request>({
            query: ({ id, body }) => ({
                url: `categories/${id}`,
                method: 'PUT',
                body,
            }),
            invalidatesTags: ['Category'],
        }),
        deleteCategory: builder.mutation<void, string>({
            query: (id) => ({
                url: `categories/${id}`,
                method: 'DELETE',
            }),
            invalidatesTags: ['Category'],
        }),
    }),
});

export const {
    useGetCategoriesQuery,
    useLazyGetCategoriesQuery,
    useGetCategoryByIdQuery,
    useCreateCategoryMutation,
    useUpdateCategoryMutation,
    useDeleteCategoryMutation,
} = categoryApiSlice;
