import { ENV_CONFIGS } from "@/lib/config/env";

export const APP_ROUTES = {
  HOME: "/",
  DASHBOARD: "/dashboard",
  PRODUCTS: "/products",
  ORDERS: "/orders",
  CUSTOMERS: "/customers",
  PROFILE: "/profile",
  LOGIN: `${ENV_CONFIGS.bffUrl}/api/auth/login`,
  REGISTER: `${ENV_CONFIGS.bffUrl}/api/auth/register`,
};
