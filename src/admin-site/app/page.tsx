"use client";

import { APP_ROUTES } from "@/lib/api/routes";
import Logo from "@/features/shared/components/Logo";

export default function Home() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center p-4 bg-cover bg-center bg-no-repeat">
      <div className="absolute inset-0 bg-black/80 backdrop-blur-[2px]"></div>

      <div className="relative z-10 bg-base-100/80 backdrop-blur-xl p-10 rounded-3xl shadow-2xl flex flex-col items-center max-w-md w-full border border-white/10">
        <div className="transform hover:scale-105 transition-transform duration-300">
          <Logo width={520} height={100} />
        </div>

        <div className="text-center mb-10">
          <h1 className="text-3xl font-bold tracking-tight text-base-content mb-2">Admin Portal</h1>
          <p className="text-base-content/60 text-sm">Secure access for NashFriday Store administrators only.</p>
        </div>

        <div className="flex flex-col gap-6 w-full">
          <div className="space-y-4">
            <a
              href={APP_ROUTES.LOGIN}
              className="btn btn-primary btn-lg w-full normal-case text-lg shadow-lg hover:shadow-primary/20 hover:scale-[1.02] active:scale-[0.98] transition-all duration-200 gap-3">
              <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"
                />
              </svg>
              Login with NashFriday
            </a>
          </div>

          <div className="divider text-xs text-base-content/30">SECURE SESSION</div>

          <div className="flex items-center justify-center gap-2 text-xs text-base-content/40">
            <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"
              />
            </svg>
            End-to-end encrypted authentication
          </div>
        </div>
      </div>

      <footer className="relative z-10 mt-8 text-white/40 text-xs">
        &copy; {new Date().getFullYear()} NashFriday Store Management System. All rights reserved.
      </footer>
    </div>
  );
}
