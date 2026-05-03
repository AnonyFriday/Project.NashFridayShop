"use client";

import { useState } from "react";
import Link from "next/link";
import { useGetCategoriesQuery, useDeleteCategoryMutation } from "@/features/categories/category.api";
import DataTable, { ColumnDef } from "@/features/shared/components/DataTable";
import Pagination from "@/features/shared/components/Pagination";
import { ActionGroupInDataTable, EditButton, DeleteButton, ViewButton } from "@/features/shared/components/Buttons/DataTableButtons";
import { GetCategories } from "@/features/categories/category.types";
import SearchInput from "@/features/shared/components/SearchInput";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";
import { APP_ROUTES } from "@/lib/api/routes";

export default function CategoriesPage() {
  const dispatch = useAppDispatch();
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [searchName, setSearchName] = useState("");

  const { data, isLoading, error } = useGetCategoriesQuery({
    pageIndex,
    pageSize,
    searchName: searchName || undefined,
  });

  const [deleteCategory] = useDeleteCategoryMutation();

  const handleDelete = async (id: string) => {
    try {
      await deleteCategory(id).unwrap();
      dispatch(
        enqueueToast({
          message: "Category deleted successfully.",
          type: ToastType.Success,
        }),
      );
    } catch (error) {
      console.error(error);
      dispatch(
        enqueueToast({
          message: "Failed to delete category.",
          type: ToastType.Error,
        }),
      );
    }
  };

  const columns: ColumnDef<GetCategories.Item>[] = [
    {
      key: "name",
      header: "Category Name",
      render: (category) => <span className="font-medium text-base-content">{category.name}</span>,
    },
    {
      key: "description",
      header: "Description",
      render: (category) => <span className="text-sm text-base-content/70 line-clamp-1">{category.description}</span>,
    },
    {
      key: "actions",
      header: "Actions",
      render: (category) => (
        <ActionGroupInDataTable>
          <ViewButton href={`${APP_ROUTES.CATEGORIES}/${category.id}`} />
          <EditButton href={`${APP_ROUTES.CATEGORIES}/${category.id}/edit`} />
          <DeleteButton onClick={() => handleDelete(category.id)} />
        </ActionGroupInDataTable>
      ),
    },
  ];

  if (error) {
    return <div className="alert alert-error">Failed to load categories.</div>;
  }

  return (
    <div className="flex flex-col gap-6 p-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Categories</h1>
        <Link href="/categories/new" className="btn btn-primary">
          Add Category
        </Link>
      </div>

      {/* Filters */}
      <div className="flex flex-col gap-6">
        <div className="flex flex-wrap items-center gap-4 bg-base-100 p-4 rounded-box border border-base-200 shadow-sm">
          <div className="flex-1 min-w-[200px]">
            <label className="label label-text text-xs font-bold uppercase text-base-content/50">Search Categories</label>
            <SearchInput
              placeholder="Search by name..."
              onSearch={(val) => {
                setSearchName(val);
                setPageIndex(0);
              }}
              initialValue={searchName}
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
