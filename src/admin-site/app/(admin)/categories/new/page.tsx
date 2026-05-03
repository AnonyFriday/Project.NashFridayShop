"use client";

import { useRouter } from "next/navigation";
import { useCreateCategoryMutation } from "@/features/categories/category.api";
import CategoryForm, { CategoryFormData } from "@/features/categories/components/CategoryForm";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";
import { APP_ROUTES } from "@/lib/api/routes";
import GoBackButton from "@/features/shared/components/Buttons/GoBackButton";

export default function NewCategoryPage() {
  const router = useRouter();
  const dispatch = useAppDispatch();
  const [createCategory, { isLoading }] = useCreateCategoryMutation();

  const handleSubmit = async (data: CategoryFormData) => {
    try {
      await createCategory(data).unwrap();
      dispatch(
        enqueueToast({
          message: "Category created successfully!",
          type: ToastType.Success,
        }),
      );
      router.push(APP_ROUTES.CATEGORIES);
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to create category.",
          type: ToastType.Error,
        }),
      );
    }
  };

  return (
    <div className="flex flex-col gap-6 p-4 max-w-4xl mx-auto w-full">
      <GoBackButton href={APP_ROUTES.CATEGORIES} title="Add New Category" />

      <CategoryForm onSubmit={handleSubmit} onCancel={() => router.push(APP_ROUTES.CATEGORIES)} isLoading={isLoading} />
    </div>
  );
}
