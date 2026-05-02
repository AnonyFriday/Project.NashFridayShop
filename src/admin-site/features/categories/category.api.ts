import { baseApiSlice } from "@/lib/api/base.api";
import { GetCategories } from "./category.types";

export const categoryApiSlice = baseApiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getCategories: builder.query<GetCategories.Response, GetCategories.Request>({
            query: (params) => ({
                url: "categories",
                params
            }),
        }),
    }),
});

export const {
    useGetCategoriesQuery,
    useLazyGetCategoriesQuery,
} = categoryApiSlice;
