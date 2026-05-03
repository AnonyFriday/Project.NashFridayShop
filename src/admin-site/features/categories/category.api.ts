import { baseApiSlice } from "@/lib/api/base.api";
import { GetCategories, GetCategoryById, CreateCategory, UpdateCategory } from "./category.types";
import { APP_ROUTES } from "@/lib/api/routes";

export const categoryApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getCategories: builder.query<GetCategories.Response, GetCategories.Request>({
            query: (params) => ({
                url: APP_ROUTES.CATEGORIES,
                params
            }),
            providesTags: ['Category'],
        }),
        getCategoryById: builder.query<GetCategoryById.Response, string>({
            query: (id) => ({
                url: `${APP_ROUTES.CATEGORIES}/${id}`,
            }),
            providesTags: ['Category'],
        }),
        createCategory: builder.mutation<CreateCategory.Response, CreateCategory.Request>({
            query: (body) => ({
                url: APP_ROUTES.CATEGORIES,
                method: 'POST',
                body,
            }),
            invalidatesTags: ['Category'],
        }),
        updateCategory: builder.mutation<UpdateCategory.Response, UpdateCategory.Request>({
            query: ({ id, body }) => ({
                url: `${APP_ROUTES.CATEGORIES}/${id}`,
                method: 'PUT',
                body,
            }),
            invalidatesTags: ['Category'],
        }),
        deleteCategory: builder.mutation<void, string>({
            query: (id) => ({
                url: `${APP_ROUTES.CATEGORIES}/${id}`,
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
