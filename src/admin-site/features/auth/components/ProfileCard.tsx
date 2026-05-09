"use client";

import { useAppSelector } from "@/lib/redux/hooks";
import { motion } from "motion/react";

export default function ProfileCard() {
  const user = useAppSelector((state) => state.authSlice.user);

  if (!user) return null;

  const role = user.roles?.[0] || "Admin";
  const initial = (user.email || "?")[0].toUpperCase();

  return (
    <motion.div
      initial={{ opacity: 0, scale: 0.95 }}
      animate={{ opacity: 1, scale: 1 }}
      whileHover={{ y: -2 }}
      className="relative flex flex-col gap-3 rounded-3xl bg-base-300/20 p-5 backdrop-blur-md transition-all hover:bg-base-300/40 border border-white/5">
      {/* Top Section: Avatar & Role */}
      <div className="flex items-start justify-between">
        <div className="relative">
          <div className="avatar placeholder ring-offset-base-100 ring-offset-2 ring-2 ring-primary/30 rounded-2xl overflow-hidden shadow-xl">
            <div className="w-14 rounded-2xl bg-linear-to-br from-primary/80 to-secondary/80 text-primary-content flex items-center justify-center">
              <span className="text-2xl font-black italic tracking-tighter">{initial}</span>
            </div>
          </div>
          <div className="absolute -right-1 -top-1 flex h-4 w-4">
            <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-success opacity-75"></span>
            <span className="relative inline-flex rounded-full h-4 w-4 bg-success border-2 border-base-200"></span>
          </div>
        </div>

        <div className="flex flex-col items-end">
          <span className="text-[10px] font-black uppercase tracking-[0.2em] text-primary/70">Verified</span>
          <div className="mt-1 h-[2px] w-8 bg-linear-to-r from-primary to-transparent" />
        </div>
      </div>

      {/* Middle Section: User Details */}
      <div className="mt-1 space-y-0.5">
        <h3 className="text-lg font-black tracking-tight text-base-content/90 leading-tight">
          {(user.email?.split("@")[0] || "User").toUpperCase()}
        </h3>
        <p className="truncate text-[11px] font-medium text-base-content/40 lowercase tracking-wide">{user.email}</p>
      </div>

      {/* Bottom Section: Status & Badge */}
      <div className="mt-2 flex items-center justify-between border-t border-base-content/5 pt-4">
        <div className="flex items-center gap-1.5">
          <div className="h-1.5 w-1.5 rounded-full bg-primary" />
          <span className="text-[10px] font-bold uppercase tracking-widest text-base-content/50">{role}</span>
        </div>

        <button className="btn btn-ghost btn-xs btn-circle opacity-30 hover:opacity-100">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor" className="w-4 h-4">
            <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 12h15m0 0l-6.75-6.75M19.5 12l-6.75 6.75" />
          </svg>
        </button>
      </div>

      {/* Subtle Bottom Glow */}
      <div className="absolute -bottom-8 -left-8 h-20 w-20 rounded-full bg-secondary/5 blur-3xl" />
    </motion.div>
  );
}
