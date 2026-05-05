import { baseApiSlice } from "@/lib/api/base.api";
import { UserInfo } from "./auth.types";
import { APP_ROUTES } from "@/lib/api/routes";

export const authApi = baseApiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getUserInfo: builder.query<UserInfo, void>({
      query: () => ({
        url: "/auth/me",
      }),
      providesTags: ["User"],
    }),

    // Posting only for login redirect
    loginRedirect: builder.mutation<void, void>({
      query: () => ({
        url: "",
      }),
      async onQueryStarted() {
        window.location.href = APP_ROUTES.LOGIN;
      },
    }),

    // Posting only for logout redirect
    logoutRedirect: builder.mutation<void, void>({
      query: () => ({
        url: "",
      }),
      async onQueryStarted() {
        const form = document.createElement("form");
        form.method = "POST";
        form.action = APP_ROUTES.LOGOUT;
        document.body.appendChild(form);
        form.submit();
      },
      invalidatesTags: ["User"],
    }),
  }),
});

export const { useGetUserInfoQuery, useLoginRedirectMutation, useLogoutRedirectMutation } = authApi;
