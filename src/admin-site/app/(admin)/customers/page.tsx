"use client";

import { useState } from "react";
import { useGetCustomersQuery } from "@/features/customers/customer.api";
import DataTable, { ColumnDef } from "@/features/shared/components/DataTable";
import Pagination from "@/features/shared/components/Pagination";
import { ActionGroupInDataTable, ViewButton, DeleteButton } from "@/features/shared/components/Buttons/DataTableButtons";
import { GetCustomers } from "@/features/customers/customer.types";
import SearchInput from "@/features/shared/components/SearchInput";
import ToggleInput from "@/features/shared/components/ToggleInput";
import ArchiveStatusBadge from "@/features/shared/components/ArchiveStatusBadge";
import { CustomerCreatedAtHelper } from "@/features/customers/customer.helper";

export default function CustomersPage() {
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [searchName, setSearchName] = useState("");
  const [includeDeleted, setIncludeDeleted] = useState(true);

  const { data, isLoading, error } = useGetCustomersQuery({
    pageIndex,
    pageSize,
    searchName: searchName || undefined,
    includeDeleted,
  });

  const columns: ColumnDef<GetCustomers.Item>[] = [
    {
      key: "fullName",
      header: "Full Name",
      render: (customer) => <span className="font-medium text-base-content">{customer.fullName}</span>,
    },
    {
      key: "email",
      header: "Email",
      render: (customer) => <span className="text-sm text-base-content/70">{customer.email}</span>,
    },
    {
      key: "address",
      header: "Address",
      render: (customer) => (
        <div className="min-w-[200px]">
          <span className="text-sm text-base-content/70 line-clamp-2 break-words">{customer.address}</span>
        </div>
      ),
    },
    {
      key: "createdAtUtc",
      header: "Joined At",
      render: (customer) => <span className="text-sm text-base-content/60">{CustomerCreatedAtHelper.formatDate(customer.createdAtUtc)}</span>,
    },
    {
      key: "isDeleted",
      header: "Status",
      render: (customer) => <ArchiveStatusBadge isDeleted={customer.isDeleted} size="sm" />,
    },
    {
      key: "actions",
      header: "Actions",
      render: () => (
        <ActionGroupInDataTable>
          <div className="tooltip" data-tip="Under development">
            <ViewButton onClick={() => {}} className="opacity-50 cursor-not-allowed" />
          </div>
          <div className="tooltip" data-tip="Under development">
            <DeleteButton onClick={() => {}} className="opacity-50 cursor-not-allowed" />
          </div>
        </ActionGroupInDataTable>
      ),
    },
  ];

  if (error) {
    return <div className="alert alert-error">Failed to load customers.</div>;
  }

  return (
    <div className="flex flex-col gap-6 p-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Customers</h1>
      </div>

      {/* Filters */}
      <div className="flex flex-col gap-6">
        <div className="flex flex-wrap items-center gap-4 bg-base-100 p-4 rounded-box border border-base-200 shadow-sm">
          <div className="flex-1 min-w-[200px]">
            <label className="label label-text text-xs font-bold uppercase text-base-content/50">Search Customers</label>
            <SearchInput
              placeholder="Search by name, username or email..."
              onSearch={(val) => {
                setSearchName(val);
                setPageIndex(0);
              }}
              initialValue={searchName}
            />
          </div>

          <div className="flex items-center gap-2 pt-6">
            <ToggleInput
              label="Show Archived"
              checked={includeDeleted}
              onChange={(val) => {
                setIncludeDeleted(val);
                setPageIndex(0);
              }}
              className="px-2"
            />
          </div>
        </div>
      </div>

      {/* Data Table */}
      <DataTable columns={columns} data={data?.items || []} isLoading={isLoading} />

      {data?.totalPages !== undefined && (
        <Pagination
          pageIndex={pageIndex}
          pageSize={pageSize}
          totalPages={data.totalPages}
          totalItems={data.totalItems}
          onPageIndexChange={(newPageIndex) => setPageIndex(newPageIndex)}
          onPageSizeChange={(newPageSize) => {
            setPageSize(newPageSize);
            setPageIndex(0);
          }}
        />
      )}
    </div>
  );
}
