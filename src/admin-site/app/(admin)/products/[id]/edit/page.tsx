"use client";

import { useState } from "react";
import { useRouter, useParams } from "next/navigation";
import { useGetProductByIdQuery, useUpdateProductMutation } from "@/features/products/product.api";
import { useGetCategoriesQuery } from "@/features/categories/category.api";
import ProductForm, { ProductFormData } from "@/features/products/components/ProductForm";
import ProductImageUpload from "@/features/products/components/ProductImageUpload";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";
import Link from "next/link";
import LoadingSpinner from "@/features/shared/components/LoadingSpinner";
import { APP_ROUTES } from "@/lib/api/routes";

export default function EditProductPage() {
  const router = useRouter();
  const params = useParams<{ id: string }>();
  const dispatch = useAppDispatch();
  const [currentStep, setCurrentStep] = useState(1);

  const {
    data: product,
    isLoading: isLoadingProduct,
    error,
  } = useGetProductByIdQuery({
    id: params.id,
    includeDeleted: true,
  });

  const { data: categoriesData, isLoading: isLoadingCategories } = useGetCategoriesQuery({ isAll: true });
  const [updateProduct, { isLoading: isUpdatingProduct }] = useUpdateProductMutation();

  const handleInfoSubmit = async (data: ProductFormData) => {
    try {
      await updateProduct({
        id: params.id,
        body: data,
        includeDeleted: true,
      }).unwrap();

      setCurrentStep(2);
      dispatch(
        enqueueToast({
          message: "Product information updated!",
          type: ToastType.Success,
        }),
      );
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to update product.",
          type: ToastType.Error,
        }),
      );
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
    <div className="flex flex-col gap-8 p-6 max-w-4xl mx-auto">
      {/* Breadcrumbs */}
      <div className="breadcrumbs text-sm bg-base-100 p-4 rounded-lg shadow-sm border border-base-200">
        <ul>
          <li className={currentStep >= 1 ? "text-primary font-bold" : "text-base-content/40"}>
            <span className="flex items-center gap-2">
              <span
                className={`w-5 h-5 flex items-center justify-center rounded-full text-[10px] ${currentStep >= 1 ? "bg-primary text-primary-content" : "bg-base-300"}`}>
                1
              </span>
              Update Info
            </span>
          </li>
          <li className={currentStep >= 2 ? "text-primary font-bold" : "text-base-content/40"}>
            <span className="flex items-center gap-2">
              <span
                className={`w-5 h-5 flex items-center justify-center rounded-full text-[10px] ${currentStep >= 2 ? "bg-primary text-primary-content" : "bg-base-300"}`}>
                2
              </span>
              Product Image
            </span>
          </li>
          <li className={currentStep >= 3 ? "text-primary font-bold" : "text-base-content/40"}>
            <span className="flex items-center gap-2">
              <span
                className={`w-5 h-5 flex items-center justify-center rounded-full text-[10px] ${currentStep >= 3 ? "bg-primary text-primary-content" : "bg-base-300"}`}>
                3
              </span>
              Finished
            </span>
          </li>
        </ul>
      </div>

      <div className="flex flex-col gap-2">
        <h1 className="text-3xl font-extrabold tracking-tight">Edit Product</h1>
        <p className="text-base-content/60">Modify the product details using the steps below.</p>
      </div>

      {currentStep === 1 && (
        <div className="animate-in fade-in slide-in-from-bottom-4 duration-500">
          <ProductForm
            initialProduct={product}
            onSubmit={handleInfoSubmit}
            onCancel={() => router.push(`${APP_ROUTES.PRODUCTS}/${params.id}`)}
            categoriesData={categoriesData?.items || []}
            isLoadingProduct={isUpdatingProduct}
            isLoadingCategories={isLoadingCategories}
          />
        </div>
      )}

      {currentStep === 2 && (
        <div className="animate-in fade-in slide-in-from-right-4 duration-500">
          <ProductImageUpload initialImageUrl={product.imageUrl} productId={params.id} onSuccess={() => setCurrentStep(3)} onSkip={() => setCurrentStep(3)} />
        </div>
      )}

      {currentStep === 3 && (
        <div className="card bg-base-100 shadow-xl border border-base-200 animate-in zoom-in-95 duration-500">
          <div className="card-body items-center text-center py-12 gap-6">
            <div className="w-20 h-20 bg-success/20 text-success rounded-full flex items-center justify-center mb-2">
              <svg xmlns="http://www.w3.org/2000/svg" className="h-10 w-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M5 13l4 4L19 7" />
              </svg>
            </div>
            <div className="space-y-2">
              <h2 className="text-3xl font-bold">Updated!</h2>
              <p className="text-base-content/60">The product has been updated successfully.</p>
            </div>
            <div className="flex gap-4 mt-4">
              <Link href={`${APP_ROUTES.PRODUCTS}/${params.id}`} className="btn btn-primary px-8">
                View Changes
              </Link>
              <Link href={APP_ROUTES.PRODUCTS} className="btn btn-outline">
                Back to List
              </Link>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
