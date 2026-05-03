import { baseApiSlice } from "@/lib/api/base.api";
import { UserInfo } from "./auth.types";

export const authApi = baseApiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getUserInfo: builder.query<UserInfo, void>({
      query: () => ({
        url: "/auth/me",
      }),
      // This is crucial: don't cache auth info for too long, or force refresh on login
      providesTags: ['User'],
    }),
  }),
});

export const { useGetUserInfoQuery } = authApi;
