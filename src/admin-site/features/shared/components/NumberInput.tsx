import { useEffect, useState } from "react";

interface NumberInputProps {
  label?: string;
  initialValue?: number;
  onNumberChange: (value: number | undefined) => void;
  placeholder?: string;
  className?: string;
  prefix?: string;
}

export default function NumberInput({ label, initialValue, onNumberChange: onChange, placeholder = "0.00", className = "", prefix }: NumberInputProps) {
  const [value, setValue] = useState<number | string | undefined>(initialValue);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (value && Number(value) < 0) {
        onChange(0);
      } else if (value === "" || isNaN(Number(value)) || value === undefined) {
        onChange(undefined);
      } else {
        onChange(Number(value));
      }
    }, 1000);

    return () => clearTimeout(timer);
  }, [value, onChange]);

  useEffect(() => {
    setValue(initialValue);
  }, [initialValue]);

  return (
    <div className={`form-control w-full ${className}`}>
      {label && <label className="label label-text text-xs font-bold uppercase text-base-content/50">{label}</label>}
      <div className="relative">
        {prefix && (
          <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <span className="text-base-content/40 text-sm">{prefix}</span>
          </div>
        )}
        <input
          type="number"
          step="0.01"
          min={0}
          className={`input input-bordered w-full ${prefix ? "pl-7" : ""}`}
          placeholder={placeholder}
          value={value ?? ""}
          onChange={(e) => setValue(e.target.value)}
        />
      </div>
    </div>
  );
}
