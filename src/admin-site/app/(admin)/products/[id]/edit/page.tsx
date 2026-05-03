"use client";

import { useRouter } from "next/navigation";
import { useGetProductByIdQuery, useUpdateProductMutation } from "@/features/products/product.api";
import GoBackButton from "@/features/shared/components/Buttons/GoBackButton";
import { APP_ROUTES } from "@/lib/api/routes";
import ProductForm, { ProductFormData } from "@/features/products/components/ProductForm";
import { useGetCategoriesQuery } from "@/features/categories/category.api";
import { useParams } from "next/navigation";
import { useAppDispatch } from "@/lib/redux/hooks";
import { ToastType, enqueueToast } from "@/features/shared/toast.slice";
import LoadingSpinner from "@/features/shared/components/LoadingSpinner";

export default function EditProductPage() {
  const dispatch = useAppDispatch();
  const params = useParams<{ id: string }>();
  const router = useRouter();

  const { data: product, isLoading: isLoadingProduct, error } = useGetProductByIdQuery({ id: params.id, includeDeleted: true });

  const { data: categoriesData, isLoading: isLoadingCategories } = useGetCategoriesQuery({
    isAll: true,
  });

  const [updateProduct, { isLoading: isUpdatingProduct }] = useUpdateProductMutation();

  const onCancel = () => {
    router.back();
  };

  const onSubmit = async (data: ProductFormData) => {
    try {
      const result = await updateProduct({
        id: params.id,
        body: data,
      }).unwrap();

      dispatch(enqueueToast({ message: `Product "${result.name}" updated successfully.`, type: ToastType.Success }));
      if (!isUpdatingProduct) {
        router.push(`${APP_ROUTES.PRODUCTS}/${params.id}`);
      }
    } catch (err) {
      console.error(err);
      dispatch(enqueueToast({ message: "Product updated failed.", type: ToastType.Error }));
    }
  };

  if (isLoadingProduct) {
    return <LoadingSpinner />;
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
      <GoBackButton href={`${APP_ROUTES.PRODUCTS}`} title="Edit Product" />
      <ProductForm
        initialProduct={product}
        isLoadingProduct={isUpdatingProduct || isLoadingProduct}
        isLoadingCategories={isLoadingCategories}
        categoriesData={categoriesData?.items ?? []}
        onSubmit={onSubmit}
        onCancel={onCancel}
      />
    </div>
  );
}
