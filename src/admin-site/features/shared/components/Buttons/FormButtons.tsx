export interface FormButtonProps {
  title: string;
  className?: string;
  isLoading?: boolean;
  disabled?: boolean;
  onClick?: () => void;
}

export function SubmitButton({ title, className, isLoading, disabled }: FormButtonProps) {
  const baseClassName = "btn btn-primary flex-1";
  return (
    <button
      type="submit"
      className={`${className ?? baseClassName} ${isLoading ? "loading" : ""}`}
      disabled={disabled || isLoading}
    >
      {isLoading ? "Saving..." : title}
    </button>
  );
}

export function CancelButton({ onClick, title, className, disabled }: FormButtonProps) {
  const baseClassName = "btn btn-outline flex-1";
  return (
    <button type="button" className={className ?? baseClassName} onClick={onClick} disabled={disabled}>
      {title}
    </button>
  );
}
