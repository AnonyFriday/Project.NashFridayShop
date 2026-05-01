"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAppDispatch } from "@/lib/redux/hooks";
import { setSidebarOpen } from "@/features/layout/slices/uiDrawerSlice";

const sidebarItems = [
  { label: "Dashboard", href: "/dashboard" },
  { label: "Products", href: "/products" },
  { label: "Orders", href: "/orders" },
  { label: "Customers", href: "/customers" },
];

export default function Sidebar() {
  const pathname = usePathname();
  const dispatch = useAppDispatch();

  return (
    <div className="bg-base-200 min-h-full w-72 p-4 flex flex-col">
      <div className="text-lg font-bold mb-6 px-2">Admin Panel</div>

      <nav className="flex flex-col gap-1">
        {sidebarItems.map((item) => {
          const isActive = pathname === item.href;

          return (
            <Link
              key={item.href}
              href={item.href}
              onClick={() => dispatch(setSidebarOpen(false))}
              className={`
                px-3 py-2 rounded-lg transition
                ${isActive ? "bg-primary text-primary-content font-semibold" : "hover:bg-base-300"}
              `}
            >
              {item.label}
            </Link>
          );
        })}
      </nav>
    </div>
  );
}
