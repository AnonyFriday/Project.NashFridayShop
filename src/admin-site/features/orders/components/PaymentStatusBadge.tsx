import { PaymentStatus } from "../order.types";

interface PaymentStatusBadgeProps {
  status: PaymentStatus;
  className?: string;
  size?: "xs" | "sm" | "md" | "lg";
}

export default function PaymentStatusBadge({ status, className = "", size = "md" }: PaymentStatusBadgeProps) {
  const badgeClass = PaymentStatus.toBadgeClassName(status);
  const sizeClass = size === "md" ? "" : `badge-${size}`;

  return <div className={`badge ${sizeClass} font-medium ${badgeClass} py-4 px-4 ${className}`}>{status}</div>;
}
