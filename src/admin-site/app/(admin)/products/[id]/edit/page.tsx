"use client";

import { useRouter } from "next/navigation";
import { useGetProductByIdQuery, useUpdateProductMutation } from "@/features/products/product.api";
import GoBackButton from "@/features/shared/components/Buttons/GoBackButton";
import { APP_ROUTES } from "@/lib/api/routes";
import ProductForm, { ProductFormData } from "@/features/products/components/ProductForm";
import { useGetCategoriesQuery } from "@/features/categories/category.api";
import { useParams } from "next/navigation";

export default function EditProductPage() {
  const params = useParams<{ id: string }>();
  const router = useRouter();

  const {
    data: product,
    isLoading: isFetching,
    error,
  } = useGetProductByIdQuery({
    id: params.id,
  });

  const { data: categoriesData, isLoading: isLoadingCategories } = useGetCategoriesQuery({
    isAll: true,
  });

  const [updateProduct, { isLoading: isUpdating }] = useUpdateProductMutation();

  const onCancel = () => {
    router.back();
  };

  const onSubmit = async (data: ProductFormData) => {
    await updateProduct({
      id: params.id,
      body: data,
    }).unwrap();
  };

  if (isFetching) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <span className="loading loading-spinner loading-lg text-primary"></span>
      </div>
    );
  }

  if (error || !product) {
    return (
      <div className="p-4">
        <div className="alert alert-error">Failed to load product for editing.</div>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 p-4 max-w-4xl mx-auto w-full">
      <GoBackButton href={`${APP_ROUTES.PRODUCTS}/${params.id}`} title="Edit Product" />
      <ProductForm initialProduct={product} isLoading={isUpdating} isLoadingCategories={isLoadingCategories} categoriesData={categoriesData?.items ?? []} onSubmit={onSubmit} onCancel={onCancel} />
    </div>
  );
}
