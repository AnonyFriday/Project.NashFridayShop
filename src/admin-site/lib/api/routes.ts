import { ENV_CONFIGS } from "../config/env";

export const APP_ROUTES = {
  HOME: "/",
  DASHBOARD: "/dashboard",
  PRODUCTS: "/products",
  ORDERS: "/orders",
  CUSTOMERS: "/customers",
  CATEGORIES: "/categories",
  PROFILE: "/profile",
  LOGIN: `${ENV_CONFIGS.bffAuthUrl}/login`,
  LOGOUT: `${ENV_CONFIGS.bffAuthUrl}/logout`,
};
