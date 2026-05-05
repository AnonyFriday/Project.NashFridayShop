"use client";

import Sidebar from "@/features/shared/components/Sidebar";
import Navbar from "@/features/shared/components/Navbar";
import FloatingDrawerButton from "@/features/shared/components/Buttons/FloatingDrawerButton";
import DrawerCheckbox from "@/features/shared/components/DrawerCheckbox";
import { useAppDispatch } from "@/lib/redux/hooks";
import LoadingSpinner from "@/features/shared/components/LoadingSpinner";
import { useEffect } from "react";
import { setUser } from "@/features/auth/auth.slice";
import { useGetUserInfoQuery } from "@/features/auth/auth.api";

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  const { data: user, isLoading, isSuccess, isError } = useGetUserInfoQuery();
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (isSuccess && user) {
      dispatch(setUser(user));
    }
    if (isError) {
      dispatch(setUser(null));
    }
  }, [user, isSuccess, isError, dispatch]);

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <LoadingSpinner></LoadingSpinner>
      </div>
    );
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
