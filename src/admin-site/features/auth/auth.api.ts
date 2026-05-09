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
    loginRedirect: builder.mutation<void, string | undefined>({
      query: () => ({
        url: "",
      }),
      async onQueryStarted(returnUrl) {
        const url = returnUrl ? `${APP_ROUTES.LOGIN}?returnUrl=${encodeURIComponent(returnUrl)}` : APP_ROUTES.LOGIN;
        window.location.href = url;
      },
    }),

    // Posting only for logout redirect
    logoutRedirect: builder.mutation<void, string | undefined>({
      query: () => ({
        url: "",
      }),
      async onQueryStarted(returnUrl) {
        const url = returnUrl ? `${APP_ROUTES.LOGOUT}?returnUrl=${encodeURIComponent(returnUrl)}` : APP_ROUTES.LOGOUT;
        const form = document.createElement("form");
        form.method = "POST";
        form.action = url;
        document.body.appendChild(form);
        form.submit();
      },
      invalidatesTags: ["User"],
    }),
  }),
});

export const { useGetUserInfoQuery, useLoginRedirectMutation, useLogoutRedirectMutation } = authApi;
