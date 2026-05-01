"use client";

import { useAppDispatch, useAppSelector } from "@/lib/redux/hooks";
import { setSidebarOpen } from "@/features/layout/slices/uiDrawerSlice";

export default function DrawerCheckbox() {
  const isSidebarOpen = useAppSelector((state) => state.uiDrawer.isSidebarOpen);
  const dispatch = useAppDispatch();
  return (
    <input
      id="admin-drawer"
      type="checkbox"
      className="drawer-toggle"
      checked={isSidebarOpen}
      onChange={(e) => dispatch(setSidebarOpen(e.target.checked))}
    />
  );
}
