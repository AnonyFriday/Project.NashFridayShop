import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";
import { ENV_CONFIGS } from "@/lib/config/env";

export function proxy(request: NextRequest) {
  const sessionCookie = request.cookies.get(ENV_CONFIGS.sessionCookieName);
  const identiyServerCookie = request.cookies.get(ENV_CONFIGS.identityServerCookieName);

  const { pathname } = request.nextUrl;

  // logged in
  if (pathname === "/" && sessionCookie && identiyServerCookie) {
    return NextResponse.redirect(new URL("/dashboard", request.url));
  }

  // not logged in, access other routes
  if (pathname !== "/" && (!sessionCookie || !identiyServerCookie)) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: [
    "/",
    "/dashboard/:path*",
    "/customers/:path*",
    "/categories/:path*",
    "/products/:path*",
    "/orders/:path*",
  ],
};
