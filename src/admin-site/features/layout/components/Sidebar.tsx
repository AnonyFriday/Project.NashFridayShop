"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAppDispatch } from "@/lib/redux/hooks";
import { setSidebarOpen } from "@/features/layout/drawer.slice";

import { APP_ROUTES } from "@/lib/api/routes";

const sidebarItems = [
  { label: "Dashboard", href: APP_ROUTES.DASHBOARD },
  { label: "Products", href: APP_ROUTES.PRODUCTS },
  { label: "Categories", href: APP_ROUTES.CATEGORIES },
  { label: "Orders", href: APP_ROUTES.ORDERS },
  { label: "Customers", href: APP_ROUTES.CUSTOMERS },
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
