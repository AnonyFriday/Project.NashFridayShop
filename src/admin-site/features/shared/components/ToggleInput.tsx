interface ToggleInputProps {
  label: string;
  checked: boolean;
  onChange: (checked: boolean) => void;
  className?: string;
}

export default function ToggleInput({ label, checked, onChange, className = "" }: ToggleInputProps) {
  return (
    <div className={`form-control ${className}`}>
      <label className="label cursor-pointer gap-2">
        <span className="label-text text-xs font-bold uppercase text-base-content/50">{label}</span>
        <input type="checkbox" className="toggle toggle-primary toggle-sm" checked={checked} onChange={(e) => onChange(e.target.checked)} />
      </label>
    </div>
  );
}
