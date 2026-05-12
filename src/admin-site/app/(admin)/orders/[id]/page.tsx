"use client";

import { useGetOrderByIdQuery } from "@/features/orders/order.api";
import { useParams, useRouter } from "next/navigation";
import OrderStatusBadge from "@/features/orders/components/OrderStatusBadge";
import PaymentStatusBadge from "@/features/orders/components/PaymentStatusBadge";
import DataTable, { ColumnDef } from "@/features/shared/components/DataTable";
import { GetOrderById } from "@/features/orders/order.types";

export default function OrderDetailPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const { data, isLoading, error } = useGetOrderByIdQuery(id as string);

  if (isLoading) return <div className="flex justify-center p-20"><span className="loading loading-spinner loading-lg"></span></div>;
  if (error || !data) return <div className="alert alert-error">Order not found.</div>;

  const columns: ColumnDef<GetOrderById.OrderItemDetail>[] = [
    {
      key: "productName",
      header: "Product",
      render: (item) => <span className="font-medium">{item.productName}</span>,
    },
    {
      key: "quantity",
      header: "Quantity",
      render: (item) => <span>{item.quantity}</span>,
    },
    {
      key: "productUnitPriceInUsd",
      header: "Unit Price",
      render: (item) => <span>${item.productUnitPriceInUsd.toFixed(2)}</span>,
    },
    {
      key: "total",
      header: "Total",
      render: (item) => <span className="font-bold">${(item.quantity * item.productUnitPriceInUsd).toFixed(2)}</span>,
    },
  ];

  return (
    <div className="flex flex-col gap-6 p-4">
      <div className="flex items-center gap-4">
        <button className="btn btn-ghost btn-sm" onClick={() => router.back()}>
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-5 h-5">
            <path strokeLinecap="round" strokeLinejoin="round" d="M10.5 19.5 3 12m0 0 7.5-7.5M3 12h18" />
          </svg>
          Back
        </button>
        <h1 className="text-2xl font-bold">Order Details</h1>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Customer Information */}
        <div className="card bg-base-100 shadow-sm border border-base-200">
          <div className="card-body">
            <h2 className="card-title text-base-content/50 text-sm uppercase">Customer Information</h2>
            <div className="flex flex-col gap-2 mt-2">
              <div className="flex justify-between">
                <span className="text-base-content/60">Name:</span>
                <span className="font-medium">{data.customerFullName}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-base-content/60">Email:</span>
                <span>{data.customerEmail}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-base-content/60">Phone:</span>
                <span>{data.phoneNumber}</span>
              </div>
              <div className="flex flex-col gap-1 mt-2">
                <span className="text-base-content/60 text-xs">Delivery Address:</span>
                <span className="text-sm bg-base-200 p-2 rounded mt-1">{data.deliveryAddress}</span>
              </div>
            </div>
          </div>
        </div>

        {/* Order Status */}
        <div className="card bg-base-100 shadow-sm border border-base-200">
          <div className="card-body">
            <h2 className="card-title text-base-content/50 text-sm uppercase">Order Summary</h2>
            <div className="flex flex-col gap-4 mt-2">
              <div className="flex justify-between items-center">
                <span className="text-base-content/60">Order Status:</span>
                <OrderStatusBadge status={data.orderStatus} />
              </div>
              <div className="flex justify-between items-center">
                <span className="text-base-content/60">Payment Status:</span>
                <PaymentStatusBadge status={data.paymentStatus} />
              </div>
              <div className="flex justify-between items-center border-t border-base-200 pt-4 mt-2">
                <span className="text-lg font-bold">Total Amount:</span>
                <span className="text-2xl font-black text-primary">${data.totalPriceInUsd.toFixed(2)}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Order Items */}
      <div className="card bg-base-100 shadow-sm border border-base-200 overflow-hidden">
        <div className="px-8 pt-6 pb-2">
          <h2 className="card-title text-base-content/50 text-sm uppercase">Order Items</h2>
        </div>
        <DataTable data={data.items} columns={columns} />
      </div>
    </div>
  );
}
