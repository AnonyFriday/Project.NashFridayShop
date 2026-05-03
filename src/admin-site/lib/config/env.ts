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
    firebaseApiKey: getEnv('NEXT_PUBLIC_FIREBASE_API_KEY', process.env.NEXT_PUBLIC_FIREBASE_API_KEY),
    firebaseAuthDomain: getEnv('NEXT_PUBLIC_FIREBASE_AUTH_DOMAIN', process.env.NEXT_PUBLIC_FIREBASE_AUTH_DOMAIN),
    firebaseMessagingSenderId: getEnv('NEXT_PUBLIC_FIREBASE_MESSAGING_SENDER_ID', process.env.NEXT_PUBLIC_FIREBASE_MESSAGING_SENDER_ID),
    firebaseAppId: getEnv('NEXT_PUBLIC_FIREBASE_APP_ID', process.env.NEXT_PUBLIC_FIREBASE_APP_ID),
    firebaseProjectId: getEnv('NEXT_PUBLIC_FIREBASE_PROJECT_ID', process.env.NEXT_PUBLIC_FIREBASE_PROJECT_ID),
    firebaseStorageBucket: getEnv('NEXT_PUBLIC_FIREBASE_STORAGE_BUCKET', process.env.NEXT_PUBLIC_FIREBASE_STORAGE_BUCKET),
    sessionCookieName: getEnv('NEXT_PUBLIC_SESSION_COOKIE_NAME', process.env.NEXT_PUBLIC_SESSION_COOKIE_NAME),
    identityServerCookieName: getEnv('NEXT_PUBLIC_IDENTITYSERVER_COOKIE_NAME', process.env.NEXT_PUBLIC_IDENTITYSERVER_COOKIE_NAME),
}