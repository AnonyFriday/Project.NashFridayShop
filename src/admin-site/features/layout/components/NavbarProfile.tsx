import Link from "next/link";
import { APP_ROUTES } from "@/lib/api/routes";

export default function AuthComponent() {
  return (
    <>
      <div className="flex items-center gap-3">
        <p className="bg-primary text-primary-content px-3 py-1 rounded-full text-sm font-semibold hidden sm:block">
          Admin
        </p>
        <div className="dropdown dropdown-end">
          <div tabIndex={0} role="button" className="btn btn-ghost btn-circle avatar border border-base-300">
            <div className="w-9 rounded-full bg-base-300 flex items-center justify-center">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={1.5}
                stroke="currentColor"
                className="w-6 h-6 text-base-content/70"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M15.75 6a3.75 3.75 0 1 1-7.5 0 3.75 3.75 0 0 1 7.5 0ZM4.501 20.118a7.5 7.5 0 0 1 14.998 0A17.933 17.933 0 0 1 12 21.75c-2.676 0-5.216-.584-7.499-1.632Z"
                />
              </svg>
            </div>
          </div>
          <ul
            tabIndex={0}
            className="menu menu-sm dropdown-content bg-base-100 rounded-box z-[1] mt-3 w-52 p-2 shadow border border-base-200"
          >
            <li>
              <Link href={APP_ROUTES.PROFILE}>Profile</Link>
            </li>
            <li>
              <button className="text-error">Logout</button>
            </li>
          </ul>
        </div>
      </div>
    </>
  );
}
