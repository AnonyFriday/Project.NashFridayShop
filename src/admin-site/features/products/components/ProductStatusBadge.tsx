import { ProductStatus } from "../product.types";

interface ProductStatusBadgeProps {
  status: ProductStatus;
  className?: string;
  size?: "xs" | "sm" | "md" | "lg";
}

export default function ProductStatusBadge({ status, className = "", size = "md" }: ProductStatusBadgeProps) {
  const badgeClass = ProductStatus.toBadgeClassName(status);
  const sizeClass = size === "md" ? "" : `badge-${size}`;

  return (
    <div className={`badge ${sizeClass} font-medium ${badgeClass} ${className}`}>
      {status}
    </div>
  );
}
