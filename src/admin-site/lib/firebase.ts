import { initializeApp, getApps } from "firebase/app";
import { getStorage } from "firebase/storage";
import { ENV_CONFIGS } from "./config/env";

const firebaseConfig = {
  apiKey: ENV_CONFIGS.firebaseApiKey,
  authDomain: ENV_CONFIGS.firebaseAuthDomain,
  projectId: ENV_CONFIGS.firebaseProjectId,
  storageBucket: ENV_CONFIGS.firebaseStorageBucket,
  messagingSenderId: ENV_CONFIGS.firebaseMessagingSenderId,
  appId: ENV_CONFIGS.firebaseAppId
};

// Initialize Firebase
const app = getApps().length === 0 ? initializeApp(firebaseConfig) : getApps()[0];
const storage = getStorage(app);

export { app, storage };
