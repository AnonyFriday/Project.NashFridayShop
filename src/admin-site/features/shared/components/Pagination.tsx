export interface PaginationProps {
  pageIndex: number;
  totalPages: number;
  pageSize?: number;
  totalItems?: number;
  onPageIndexChange: (newIndex: number) => void;
  onPageSizeChange?: (newSize: number) => void;
}

export default function Pagination({
  pageIndex,
  totalPages,
  pageSize = 10,
  onPageIndexChange,
  onPageSizeChange,
}: PaginationProps) {
  // Ignore pagination if totalPages is 0 or undefined
  if (totalPages <= 0) return null;

  return (
    <div className="flex justify-between items-center">
      <div className="text-sm text-base-content/70">
        {onPageSizeChange && (
          <div className="flex items-center gap-2">
            <span>Show</span>
            <select
              className="select select-bordered select-sm text-sm"
              value={pageSize}
              onChange={(e) => onPageSizeChange(Number(e.target.value))}
            >
              {[5, 10, 20, 50].map((size) => (
                <option key={size} value={size}>
                  {size}
                </option>
              ))}
            </select>
            <span>items</span>
          </div>
        )}
      </div>

      <div className="join">
        <button
          className="join-item btn btn-sm"
          disabled={pageIndex === 0}
          onClick={() => onPageIndexChange(pageIndex - 1)}
        >
          «
        </button>
        <button className="join-item btn btn-sm pointer-events-none">
          Page {pageIndex + 1} of {totalPages}
        </button>
        <button
          className="join-item btn btn-sm"
          disabled={pageIndex === totalPages - 1}
          onClick={() => onPageIndexChange(pageIndex + 1)}
        >
          »
        </button>
      </div>
    </div>
  );
}
