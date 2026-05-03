interface LoadingSpinnerProps {
  minHeight?: string;
  className?: string;
}

export default function LoadingSpinner({ minHeight = "min-h-[60vh]", className = "" }: LoadingSpinnerProps) {
  return (
    <div className={`flex items-center justify-center ${minHeight} ${className}`}>
      <span className="loading loading-infinity loading-lg text-primary"></span>
    </div>
  );
}
