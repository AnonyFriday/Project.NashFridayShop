"use client";

import { useState } from "react";
import { useGetOrdersQuery } from "@/features/orders/order.api";
import DataTable, { ColumnDef } from "@/features/shared/components/DataTable";
import Pagination from "@/features/shared/components/Pagination";
import { ActionGroupInDataTable, ViewButton } from "@/features/shared/components/Buttons/DataTableButtons";
import { GetOrders, OrderStatus, PaymentStatus } from "@/features/orders/order.types";
import { OrderHelper } from "@/features/orders/order.helper";
import OrderStatusBadge from "@/features/orders/components/OrderStatusBadge";
import PaymentStatusBadge from "@/features/orders/components/PaymentStatusBadge";
import SelectInput from "@/features/shared/components/SelectInput";

export default function OrdersPage() {
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [filters, setFilters] = useState<GetOrders.Request>({
    orderStatus: undefined,
    paymentStatus: undefined,
  });

  const resetFilters = () => {
    setFilters({
      orderStatus: undefined,
      paymentStatus: undefined,
    });
    setPageIndex(0);
  };

  const { data, isLoading, error } = useGetOrdersQuery({
    pageIndex,
    pageSize,
    orderStatus: filters.orderStatus || undefined,
    paymentStatus: filters.paymentStatus || undefined,
  });

  const handleFilterChange = (newFilters: GetOrders.Request) => {
    setFilters(newFilters);
    setPageIndex(0);
  };

  const orderStatusOptions = OrderHelper.getOrderStatusOptions();
  const paymentStatusOptions = OrderHelper.getPaymentStatusOptions();

  const columns: ColumnDef<GetOrders.Item>[] = [
    {
      key: "id",
      header: "Order ID",
      render: (order) => <span className="font-mono text-xs text-base-content/60">{order.id.split("-")[0]}...</span>,
    },
    {
      key: "customerFullName",
      header: "Customer",
      render: (order) => (
        <div className="flex flex-col">
          <span className="font-medium">{order.customerFullName}</span>
          <span className="text-xs text-base-content/50">{order.customerEmail}</span>
        </div>
      ),
    },
    {
      key: "totalPriceInUsd",
      header: "Total",
      render: (order) => (
        <span className="font-semibold text-primary">
          ${order.totalPriceInUsd.toFixed(2)}
        </span>
      ),
    },
    {
      key: "orderStatus",
      header: "Order Status",
      render: (order) => <OrderStatusBadge status={order.orderStatus} size="sm" />,
    },
    {
      key: "paymentStatus",
      header: "Payment Status",
      render: (order) => <PaymentStatusBadge status={order.paymentStatus} size="sm" />,
    },
    {
      key: "createdAtUtc",
      header: "Date",
      render: (order) => (
        <span className="text-xs">{new Date(order.createdAtUtc).toLocaleDateString()}</span>
      ),
    },
    {
      key: "actions",
      header: "Actions",
      render: (order) => (
        <ActionGroupInDataTable>
          <ViewButton href={`/orders/${order.id}`} />
        </ActionGroupInDataTable>
      ),
    },
  ];

  if (error) {
    return <div className="alert alert-error">Failed to load orders.</div>;
  }

  return (
    <div className="flex flex-col gap-6 p-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Orders</h1>
      </div>

      <div className="flex flex-col gap-6">
        <div className="flex flex-wrap items-center gap-4 bg-base-100 p-4 rounded-box border border-base-200 shadow-sm">
          <SelectInput
            label="Order Status"
            placeholder="All Statuses"
            value={filters.orderStatus || ""}
            options={orderStatusOptions}
            onChange={(val) => handleFilterChange({ ...filters, orderStatus: (val as OrderStatus) || undefined })}
            className="sm:w-64"
          />
          <SelectInput
            label="Payment Status"
            placeholder="All Statuses"
            value={filters.paymentStatus || ""}
            options={paymentStatusOptions}
            onChange={(val) => handleFilterChange({ ...filters, paymentStatus: (val as PaymentStatus) || undefined })}
            className="sm:w-64"
          />

          <div className="flex items-center gap-2 pt-6">
            <button className="btn btn-ghost btn-sm h-12 text-base-content/50 hover:text-error flex items-center gap-2" onClick={resetFilters}>
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0 3.181 3.183a8.25 8.25 0 0 0 13.803-3.7M4.031 9.865a8.25 8.25 0 0 1 13.803-3.7l3.181 3.182m0-4.991v4.99"
                />
              </svg>
              <span>Reset</span>
            </button>
          </div>
        </div>

        <DataTable data={data?.items || []} columns={columns} isLoading={isLoading} />

        {data?.totalPages !== undefined && (
          <Pagination
            pageIndex={pageIndex}
            pageSize={pageSize}
            totalPages={data.totalPages}
            totalItems={data.totalItems}
            onPageIndexChange={(newPage) => setPageIndex(newPage)}
            onPageSizeChange={(newSize) => {
              setPageSize(newSize);
              setPageIndex(0);
            }}
          />
        )}
      </div>
    </div>
  );
}
