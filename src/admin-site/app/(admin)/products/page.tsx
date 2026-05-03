"use client";

import { useState } from "react";
import { useGetProductsQuery, useDeleteProductMutation } from "@/features/products/product.api";
import { useGetCategoriesQuery } from "@/features/categories/category.api";
import DataTable, { ColumnDef } from "@/features/shared/components/DataTable";
import Pagination from "@/features/shared/components/Pagination";
import { ActionGroupInDataTable, ViewButton, EditButton, DeleteButton } from "@/features/shared/components/Buttons/DataTableButtons";
import { GetProducts, ProductStatus } from "@/features/products/product.types";
import { ProductStatusHelper } from "@/features/products/product.helper";
import Image from "next/image";
import ProductStatusBadge from "@/features/products/components/ProductStatusBadge";
import ArchiveStatusBadge from "@/features/shared/components/ArchiveStatusBadge";
import SearchInput from "@/features/shared/components/SearchInput";
import SelectInput, { SelectInputOptions } from "@/features/shared/components/SelectInput";
import ToggleInput from "@/features/shared/components/ToggleInput";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";

export default function ProductsPage() {
  const dispatch = useAppDispatch();
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const initialFilters: GetProducts.Request = {
    searchName: undefined,
    categoryId: undefined,
    status: undefined,
    minPrice: undefined,
    maxPrice: undefined,
    includeDeleted: true,
  };

  const [filters, setFilters] = useState<GetProducts.Request>({
    searchName: undefined,
    categoryId: undefined,
    status: undefined,
    minPrice: undefined,
    maxPrice: undefined,
    includeDeleted: true,
  });

  const resetFilters = () => {
    setFilters(initialFilters);
    setPageIndex(0);
  };

  const { data: categoriesData } = useGetCategoriesQuery({ isAll: true });

  const { data, isLoading, error } = useGetProductsQuery({
    pageIndex,
    pageSize,
    searchName: filters.searchName || undefined,
    categoryId: filters.categoryId || undefined,
    status: (filters.status as ProductStatus) || undefined,
    includeDeleted: filters.includeDeleted,
  });

  const handleFilterChange = (newFilters: GetProducts.Request) => {
    // Only reset page and update state accept the pageSize and pageIndex
    if (
      newFilters.searchName !== filters.searchName ||
      newFilters.categoryId !== filters.categoryId ||
      newFilters.status !== filters.status ||
      newFilters.includeDeleted !== filters.includeDeleted
    ) {
      setFilters(newFilters);
      setPageIndex(0);
    }
  };

  const categoryOptions =
    categoriesData?.items.map<SelectInputOptions>((cat) => ({
      label: cat.name,
      value: cat.id,
    })) || [];

  const statusOptions = ProductStatusHelper.getOptions();

  const [toggleDelete] = useDeleteProductMutation();

  const handleDelete = async (id: string, isDeleted: boolean) => {
    try {
      await toggleDelete(id).unwrap();
      dispatch(
        enqueueToast({
          message: `Product ${isDeleted ? "restored" : "archived"} successfully.`,
          type: ToastType.Success,
        }),
      );
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to update product archived status.",
          type: ToastType.Error,
        }),
      );
    }
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
        <span className={`font-bold ${product.quantity && product.quantity > 0 ? "text-success" : "text-error"}`}>{product.quantity ?? 0}</span>
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
      render: (product) => <ProductStatusBadge status={product.status} size="sm" />,
    },
    {
      key: "isDeleted",
      header: "Archived",
      render: (product) => <ArchiveStatusBadge isDeleted={product.isDeleted} size="sm" />,
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
          <DeleteButton onClick={() => handleDelete(product.id, product.isDeleted)} />
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

      {/* Product Filters */}
      <div className="flex flex-col gap-6">
        <div className="flex flex-wrap items-center gap-4 bg-base-100 p-4 rounded-box border border-base-200 shadow-sm">
          <div className="flex-1 min-w-[200px]">
            <label className="label label-text text-xs font-bold uppercase text-base-content/50">Search Products</label>
            <SearchInput
              placeholder="Search by name..."
              onSearch={(val) => handleFilterChange({ ...filters, searchName: val })}
              initialValue={filters.searchName}
            />
          </div>
          <SelectInput
            label="Category"
            placeholder="All Categories"
            value={filters.categoryId || ""}
            options={categoryOptions}
            onChange={(val) => handleFilterChange({ ...filters, categoryId: val })}
            className="sm:w-48"
          />
          <SelectInput
            label="Stock Status"
            placeholder="All Statuses"
            value={filters.status || ""}
            options={statusOptions}
            onChange={(val) => handleFilterChange({ ...filters, status: (val as ProductStatus) || undefined })}
            className="sm:w-48"
          />

          {/* Archive Toggle and Reset */}
          <div className="flex items-center gap-2 pt-6">
            <ToggleInput
              label="Show Archived"
              checked={!!filters.includeDeleted}
              onChange={(val) => handleFilterChange({ ...filters, includeDeleted: val })}
              className="px-2"
            />

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
