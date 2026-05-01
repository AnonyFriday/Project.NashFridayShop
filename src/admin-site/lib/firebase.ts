import { initializeApp, getApps } from "firebase/app";
import { getStorage } from "firebase/storage";
import { envConfig } from "./config/env";

const firebaseConfig = {
  apiKey: envConfig.firebaseApiKey,
  authDomain: envConfig.firebaseAuthDomain,
  projectId: envConfig.firebaseProjectId,
  storageBucket: envConfig.firebaseStorageBucket,
  messagingSenderId: envConfig.firebaseMessagingSenderId,
  appId: envConfig.firebaseAppId
};

// Initialize Firebase
const app = getApps().length === 0 ? initializeApp(firebaseConfig) : getApps()[0];
const storage = getStorage(app);

export { app, storage };
