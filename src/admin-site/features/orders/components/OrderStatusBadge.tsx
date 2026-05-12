import { OrderStatus } from "../order.types";

interface OrderStatusBadgeProps {
  status: OrderStatus;
  className?: string;
  size?: "xs" | "sm" | "md" | "lg";
}

export default function OrderStatusBadge({ status, className = "", size = "md" }: OrderStatusBadgeProps) {
  const badgeClass = OrderStatus.toBadgeClassName(status);
  const sizeClass = size === "md" ? "" : `badge-${size}`;

  return <div className={`badge ${sizeClass} font-medium ${badgeClass} py-4 px-4 ${className}`}>{status}</div>;
}
