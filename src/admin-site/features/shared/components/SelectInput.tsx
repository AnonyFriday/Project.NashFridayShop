export interface SelectInputOptions {
  label: string;
  value: string;
}

interface SelectInputProps {
  label?: string;
  value: string;
  options: SelectInputOptions[];
  onChange: (value: string) => void;
  placeholder?: string;
  className?: string;
}

export default function SelectInput({ label, value, options, onChange, placeholder = "Select...", className = "" }: SelectInputProps) {
  return (
    <div className={`form-control w-full ${className}`}>
      {label && <label className="label label-text text-xs font-bold uppercase text-base-content/50">{label}</label>}
      <select className="select select-bordered w-full" value={value} onChange={(e) => onChange(e.target.value)}>
        <option value="">{placeholder}</option>
        {options.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>
    </div>
  );
}
