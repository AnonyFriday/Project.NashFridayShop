import { useState, useEffect } from "react";

interface SearchInputProps {
  placeholder?: string;
  onSearch: (value: string) => void;
  initialValue?: string;
  className?: string;
  debounceMs?: number;
}

export default function SearchInput({ placeholder = "Search...", onSearch, initialValue = "", className = "", debounceMs = 500 }: SearchInputProps) {
  const [value, setValue] = useState(initialValue);

  // apply debouce here for delaying the search
  useEffect(() => {
    const timer = setTimeout(() => {
      onSearch(value.trim());
    }, debounceMs);

    return () => clearTimeout(timer);
  }, [value, onSearch, debounceMs]);

  return (
    <div className={`relative w-full max-w-sm ${className}`}>
      <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="w-5 h-5 text-base-content/40">
          <path strokeLinecap="round" strokeLinejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
        </svg>
      </div>
      <input type="text" className="input input-bordered w-full pl-10" placeholder={placeholder} value={value} onChange={(e) => setValue(e.target.value)} />
      {value && (
        <button className="absolute inset-y-0 right-0 pr-3 flex items-center text-base-content/40 hover:text-base-content" onClick={() => setValue("")}>
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5">
            <path strokeLinecap="round" strokeLinejoin="round" d="M6 18 18 6M6 6l12 12" />
          </svg>
        </button>
      )}
    </div>
  );
}
