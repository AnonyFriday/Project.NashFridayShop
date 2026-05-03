"use client";

import GoBackButton from "@/features/shared/components/Buttons/GoBackButton";
import { EditButton, DeleteButton } from "@/features/shared/components/Buttons/DataTableButtons";
import { APP_ROUTES } from "@/lib/api/routes";
import { useGetCategoryByIdQuery, useDeleteCategoryMutation } from "@/features/categories/category.api";
import { useParams, useRouter } from "next/navigation";
import LoadingSpinner from "@/features/shared/components/LoadingSpinner";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";

export default function CategoryViewPage() {
  const params = useParams<{ id: string }>();
  const router = useRouter();
  const dispatch = useAppDispatch();

  const { data: category, isLoading, error } = useGetCategoryByIdQuery(params.id);

  const [deleteCategory] = useDeleteCategoryMutation();

  const handleDelete = async () => {
    if (!category) return;
    try {
      await deleteCategory(category.id).unwrap();
      dispatch(
        enqueueToast({
          message: "Category deleted successfully.",
          type: ToastType.Success,
        }),
      );
      router.push(APP_ROUTES.CATEGORIES);
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to delete category.",
          type: ToastType.Error,
        }),
      );
    }
  };

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (error || !category) {
    return (
      <div className="p-4">
        <div className="alert alert-error">Failed to load category details.</div>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 p-4 max-w-4xl mx-auto w-full">
      {/* Header */}
      <GoBackButton href={APP_ROUTES.CATEGORIES} title="Category Details" />

      {/* Content Area */}
      <div className="flex flex-col gap-8 bg-base-100 p-8 rounded-box shadow-sm border border-base-200">
        <div>
          <h2 className="text-3xl font-bold text-base-content">{category.name}</h2>
          <div className="text-xs text-base-content/50 mt-2 font-mono bg-base-200 inline-block px-2 py-1 rounded">ID: {category.id}</div>
        </div>

        <div className="divider my-1"></div>

        <div>
          <h3 className="font-semibold text-lg mb-4 uppercase tracking-widest text-base-content/50">Description</h3>
          <p className="text-base-content/80 leading-relaxed text-justify text-lg">
            {category.description || "No description has been provided for this category yet."}
          </p>
        </div>

        <div className="divider my-1"></div>

        {/* Action Buttons */}
        <div className="mt-4 flex gap-3">
          <EditButton href={`${APP_ROUTES.CATEGORIES}/${category.id}/edit`} title="Edit Category" className="btn btn-primary flex-1" />
          <DeleteButton onClick={handleDelete} title="Delete Category" className="btn btn-outline btn-error px-8" />
        </div>
      </div>
    </div>
  );
}
