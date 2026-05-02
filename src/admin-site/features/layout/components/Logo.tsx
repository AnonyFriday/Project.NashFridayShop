import Image from "next/image";

interface LogoProps {
  width?: number;
  height?: number;
  className?: string;
  priority?: boolean;
}

export default function Logo({ width = 120, height = 40, className = "", priority = true }: LogoProps) {
  return (
    <Image
      src="/assets/images/nashfridayshop_logo.svg"
      alt="NashFridayShop Logo"
      width={width}
      height={height}
      className={className}
      priority={priority}
      quality={100}
      style={{ width: "auto", height: "auto", maxWidth: width }}
    />
  );
}
