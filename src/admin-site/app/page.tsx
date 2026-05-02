import { APP_ROUTES } from "@/lib/api/routes";
import Logo from "@/features/shared/components/Logo";
import { redirect } from "next/navigation";

export default function Home() {
  // Will implement authentication later
  const isAuthenticated = true;

  if (isAuthenticated) {
    redirect(APP_ROUTES.DASHBOARD);
  }

  return (
    <div className="min-h-screen bg-base-200 flex flex-col items-center justify-center p-4">
      <div className="bg-base-100 p-8 rounded-2xl shadow-xl flex flex-col items-center max-w-sm w-full">
        <Logo width={200} height={60} />

        <div className="flex flex-col gap-4 w-full">
          <a href={APP_ROUTES.LOGIN} className="btn btn-primary w-full">
            Login
          </a>
          <a href={APP_ROUTES.REGISTER} className="btn btn-outline w-full">
            Sign Up
          </a>
        </div>
      </div>
    </div>
  );
}
