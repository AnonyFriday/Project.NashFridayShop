import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  reactCompiler: true,

  images: {
    qualities: [75, 85, 100],
    remotePatterns: [
      { protocol: "https", hostname: "images.unsplash.com" },
      { protocol: "https", hostname: "cdn.yoursite.com" },
      {
        protocol: "https",
        hostname: "*",
      },
    ],
  }
};

export default nextConfig;
