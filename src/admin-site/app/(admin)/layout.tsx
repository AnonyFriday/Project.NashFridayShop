"use client";

import Sidebar from "@/features/shared/components/Sidebar";
import Navbar from "@/features/shared/components/Navbar";
import FloatingDrawerButton from "@/features/shared/components/Buttons/FloatingDrawerButton";
import DrawerCheckbox from "@/features/shared/components/DrawerCheckbox";
import { useAppDispatch, useAppSelector } from "@/lib/redux/hooks";
import LoadingSpinner from "@/features/shared/components/LoadingSpinner";
import { useEffect } from "react";
import { setUser } from "@/features/auth/auth.slice";
import { useRouter } from "next/navigation";
import { APP_ROUTES } from "@/lib/api/routes";
import { useGetUserInfoQuery } from "@/features/auth/auth.api";

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  const { data: userData, isLoading, isSuccess, isError } = useGetUserInfoQuery();
  const user = useAppSelector((state) => state.authSlice.user);
  const dispatch = useAppDispatch();
  const router = useRouter();

  useEffect(() => {
    if (isSuccess && userData) {
      dispatch(setUser(userData));
    }
    if (isError) {
      dispatch(setUser(null));
      router.push(APP_ROUTES.HOME);
    }
  }, [userData, isSuccess, isError, dispatch, router]);

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <LoadingSpinner />
      </div>
    );
  }

  // guard check user in store to make sure some component using user not having errors
  if (!user && !isLoading) {
    return null;
  }

  return (
    <div className="drawer md:drawer-open relative">
      {/* Content */}
      <DrawerCheckbox />
      <FloatingDrawerButton />

      <div className="drawer-content flex flex-col">
        <Navbar />

        {/* Toggle for open sidebar */}

        <main className="flex-1 p-6 bg-base-100">{children}</main>
      </div>

      {/* Sidebar */}
      <div className="drawer-side">
        <label htmlFor="admin-drawer" className="drawer-overlay"></label>

        <Sidebar />
      </div>
    </div>
  );
}
