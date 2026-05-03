"use client";

import ToastItem from "./ToastItem";
import { useAppSelector } from "@/lib/redux/hooks";

export default function ToastContainer() {
  const toasts = useAppSelector((state) => state.toastSlice.toasts);

  if (toasts.length <= 0) return null;

  return (
    <div className="toast toast-top toast-end z-[100] space-y-2">
      {toasts.map((toast) => (
        <ToastItem key={toast.id} toast={toast}></ToastItem>
      ))}
    </div>
  );
}
