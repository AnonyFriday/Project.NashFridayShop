// Wrapper function to check config key
const getEnv = (key: string, value: string | undefined): string => {
    if (!value) {
        throw new Error(`MISSING ENVIRONMENT VARIABLE: ${key}`);
    }

    return value;
}

// Export environment variables
export const ENV_CONFIGS = {
    bffUrl: getEnv('NEXT_PUBLIC_BFF_URL', process.env.NEXT_PUBLIC_BFF_URL),
    bffAuthUrl: getEnv('NEXT_PUBLIC_BFF_AUTH_URL', process.env.NEXT_PUBLIC_BFF_AUTH_URL),
    sessionCookieName: getEnv('NEXT_PUBLIC_SESSION_COOKIE_NAME', process.env.NEXT_PUBLIC_SESSION_COOKIE_NAME),
    identityServerCookieName: getEnv('NEXT_PUBLIC_IDENTITYSERVER_COOKIE_NAME', process.env.NEXT_PUBLIC_IDENTITYSERVER_COOKIE_NAME),
}