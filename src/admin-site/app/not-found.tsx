"use client";

import { useRouter } from "next/navigation";
import { APP_ROUTES } from "@/lib/api/routes";
import Logo from "@/features/shared/components/Logo";
import Link from "next/link";

export default function NotFound() {
  const router = useRouter();

  return (
    <main className="min-h-screen flex items-center justify-center p-6 bg-base-100">
      <div className="w-full max-w-md text-center space-y-10">
        {/* Logo */}
        <div className="flex justify-center">
          <Logo width={280} height={56} />
        </div>

        {/* Simple 404 Heading */}
        <div className="space-y-2">
          <h1 className="text-8xl font-black tracking-tighter text-primary">404</h1>
          <h2 className="text-2xl font-bold tracking-tight">Page Not Found</h2>
          <p className="text-base-content/60 leading-relaxed">Sorry, the page you are looking for doesn&apos;t exist or has been moved.</p>
        </div>

        {/* Clean Actions */}
        <div className="flex flex-col gap-3">
          <button onClick={() => router.back()} className="btn btn-primary btn-lg normal-case shadow-lg shadow-primary/10">
            Go back
          </button>
          <Link href={APP_ROUTES.DASHBOARD} className="btn btn-ghost btn-lg normal-case">
            Back to Dashboard
          </Link>
        </div>

        {/* Subtle Footer */}
        <div className="pt-10 border-t border-base-content/5">
          <p className="text-xs text-base-content/30 uppercase tracking-widest">NashFriday Store Management</p>
        </div>
      </div>
    </main>
  );
}
