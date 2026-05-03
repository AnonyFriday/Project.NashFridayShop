"use client";

import { useRouter, useParams } from "next/navigation";
import { useGetCategoryByIdQuery, useUpdateCategoryMutation } from "@/features/categories/category.api";
import GoBackButton from "@/features/shared/components/Buttons/GoBackButton";
import { APP_ROUTES } from "@/lib/api/routes";
import CategoryForm, { CategoryFormData } from "@/features/categories/components/CategoryForm";
import { useAppDispatch } from "@/lib/redux/hooks";
import { ToastType, enqueueToast } from "@/features/shared/toast.slice";
import LoadingSpinner from "@/features/shared/components/LoadingSpinner";

export default function EditCategoryPage() {
  const dispatch = useAppDispatch();
  const params = useParams<{ id: string }>();
  const router = useRouter();

  const { data: category, isLoading: isLoadingCategory, error } = useGetCategoryByIdQuery(params.id);
  const [updateCategory, { isLoading: isUpdatingCategory }] = useUpdateCategoryMutation();

  const onCancel = () => {
    router.back();
  };

  const onSubmit = async (data: CategoryFormData) => {
    try {
      await updateCategory({
        id: params.id,
        body: data,
      }).unwrap();

      dispatch(
        enqueueToast({
          message: "Category updated successfully.",
          type: ToastType.Success,
        }),
      );
      router.push(`${APP_ROUTES.CATEGORIES}/${params.id}`);
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to update category.",
          type: ToastType.Error,
        }),
      );
    }
  };

  if (isLoadingCategory) {
    return <LoadingSpinner />;
  }

  if (error || !category) {
    return (
      <div className="p-4">
        <div className="alert alert-error">Failed to load category for editing.</div>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 p-4 max-w-4xl mx-auto w-full">
      <GoBackButton href={`${APP_ROUTES.CATEGORIES}`} title="Edit Category" />
      <CategoryForm initialCategory={category} isLoading={isUpdatingCategory || isLoadingCategory} onSubmit={onSubmit} onCancel={onCancel} />
    </div>
  );
}
