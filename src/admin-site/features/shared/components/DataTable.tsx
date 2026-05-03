import { ReactNode } from "react";
import LoadingSpinner from "./LoadingSpinner";

export interface ColumnDef<T> {
  key: string;
  header: string | ReactNode;
  render?: (row: T) => ReactNode;
}

export interface DataTableProps<T> {
  data: T[];
  columns: ColumnDef<T>[];
  isLoading?: boolean;
  onRowClick?: (row: T) => void;
  emptyMessage?: string;
}

export default function DataTable<T>({ data, columns, isLoading = false, onRowClick, emptyMessage = "No records found." }: DataTableProps<T>) {
  return (
    <div className="flex flex-col w-full gap-4">
      <div className="overflow-x-auto overflow-y-auto w-full bg-base-100 rounded-box shadow border border-base-300 max-h-[70vh]">
        <table className="table w-full table-pin-rows">
          {/* TABLE HEAD */}
          <thead className="bg-base-200 text-base-content sticky top-0 z-10">
            <tr>
              {columns.map((col) => (
                <th key={col.key as string} className="whitespace-nowrap">
                  {col.header}
                </th>
              ))}
            </tr>
          </thead>

          {/* TABLE BODY */}
          <tbody>
            {isLoading ? (
              <tr>
                <td colSpan={columns.length} className="p-0">
                  <LoadingSpinner />
                </td>
              </tr>
            ) : data.length === 0 ? (
              <tr>
                <td colSpan={columns.length} className="text-center py-12 text-base-content/60">
                  {emptyMessage}
                </td>
              </tr>
            ) : (
              data.map((row, rowIndex) => (
                <tr key={rowIndex} className={`hover:bg-base-200 ${onRowClick ? "cursor-pointer" : ""}`} onClick={() => onRowClick && onRowClick(row)}>
                  {columns.map((col) => (
                    <td key={col.key as string}>{col.render ? col.render(row) : (row as Record<string, ReactNode>)[col.key]}</td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
