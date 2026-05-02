"use client";

import { useState } from "react";
import { useGetProductsQuery } from "@/features/products/product.api";
import DataTable, { ColumnDef } from "@/features/layout/components/DataTable";
import Pagination from "@/features/layout/components/Pagination";
import {
  ActionGroupInDataTable,
  ViewButton,
  EditButton,
  DeleteButton,
} from "@/features/layout/components/Buttons/DataTableButtons";
import { GetProducts, ProductStatus } from "@/features/products/product.types";
import Image from "next/image";

export default function ProductsPage() {
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize, setPageSize] = useState(10);

  const { data, isLoading, error } = useGetProductsQuery({
    pageIndex,
    pageSize,
  });

  const handleDelete = (id: string) => {
    console.log("Delete product", id);
  };

  const columns: ColumnDef<GetProducts.Item>[] = [
    {
      key: "imageUrl",
      header: "Image",
      render: (product) => (
        <div className="avatar">
          <div className="w-12 h-12 rounded-lg bg-base-200">
            {product.imageUrl ? (
              <Image src={product.imageUrl} alt={product.name} width={48} height={48} className="object-cover" />
            ) : (
              <span className="flex items-center justify-center w-full h-full text-base-content/40">No Img</span>
            )}
          </div>
        </div>
      ),
    },
    {
      key: "name",
      header: "Product Name",
      render: (product) => <span className="font-medium">{product.name}</span>,
    },
    {
      key: "quantity",
      header: "Stock",
      render: (product) => (
        <span className={`font-bold ${product.quantity && product.quantity > 0 ? "text-success" : "text-error"}`}>
          {product.quantity ?? 0}
        </span>
      ),
    },
    {
      key: "priceUsd",
      header: "Price",
      render: (product) => <span className="font-semibold text-primary">${product.priceUsd.toFixed(2)}</span>,
    },
    {
      key: "status",
      header: "Status",
      render: (product) => {
        return <div className={ProductStatus.toBadgeClassName(product.status)}>{product.status.toUpperCase()}</div>;
      },
    },
    {
      key: "averageStars",
      header: "Rating",
      render: (product) => (
        <div className="flex items-center gap-1">
          <span className="text-warning">★</span>
          <span>{product.averageStars.toFixed(1)}</span>
        </div>
      ),
    },
    {
      key: "actions",
      header: "Actions",
      render: (product) => (
        <ActionGroupInDataTable>
          <ViewButton href={`/products/${product.id}`} />
          <EditButton href={`/products/${product.id}/edit`} />
          <DeleteButton onClick={() => handleDelete(product.id)} />
        </ActionGroupInDataTable>
      ),
    },
  ];

  if (error) {
    return <div className="alert alert-error">Failed to load products.</div>;
  }

  return (
    <div className="flex flex-col gap-6 p-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Products</h1>
        <button className="btn btn-primary">Add Product</button>
      </div>

      <div className="flex flex-col gap-4">
        <DataTable data={data?.items || []} columns={columns} isLoading={isLoading} />

        {data?.totalPages !== undefined && (
          <Pagination
            pageIndex={pageIndex}
            totalPages={data.totalPages}
            pageSize={pageSize}
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
