import Link from "next/link";
import AuthComponent from "@/features/layout/components/NavbarProfile";
import Logo from "@/features/layout/components/Logo";
import { APP_ROUTES } from "@/lib/api/routes";

export default function Navbar() {
  return (
    <div className="navbar bg-base-100 shadow-sm border-b border-base-300 w-full h-14 z-10">
      {/* LEFT - LOGO */}
      <div className="flex-1 px-2 mx-2">
        <Link href={APP_ROUTES.DASHBOARD} className="flex items-center gap-2">
          <Logo width={120} height={40} />
        </Link>
      </div>

      {/* RIGHT - PROFILE */}
      <div className="flex-none gap-4 pr-2">
        <AuthComponent />
      </div>
    </div>
  );
}
