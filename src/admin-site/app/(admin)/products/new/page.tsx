"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useCreateProductMutation } from "@/features/products/product.api";
import { useGetCategoriesQuery } from "@/features/categories/category.api";
import ProductForm, { ProductFormData } from "@/features/products/components/ProductForm";
import ProductImageUpload from "@/features/products/components/ProductImageUpload";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";
import Link from "next/link";

export default function NewProductPage() {
  const router = useRouter();
  const dispatch = useAppDispatch();
  const [currentStep, setCurrentStep] = useState(1);
  const [createdProductId, setCreatedProductId] = useState<string | null>(null);

  const [createProduct, { isLoading: isCreatingProduct }] = useCreateProductMutation();
  const { data: categoriesData, isLoading: isLoadingCategories } = useGetCategoriesQuery({ isAll: true });

  const handleInfoSubmit = async (data: ProductFormData) => {
    try {
      const response = await createProduct(data).unwrap();
      setCreatedProductId(response.id);
      setCurrentStep(2);
      dispatch(
        enqueueToast({
          message: "Product info saved!",
          type: ToastType.Success,
        }),
      );
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to create product.",
          type: ToastType.Error,
        }),
      );
    }
  };

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
              Basic Information
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
        <h1 className="text-3xl font-extrabold tracking-tight">Create New Product</h1>
        <p className="text-base-content/60">Follow the steps below to list a new product in your store.</p>
      </div>

      {currentStep === 1 && (
        <div className="animate-in fade-in slide-in-from-bottom-4 duration-500">
          <ProductForm
            onSubmit={handleInfoSubmit}
            onCancel={() => router.push("/products")}
            categoriesData={categoriesData?.items || []}
            isLoadingProduct={isCreatingProduct}
            isLoadingCategories={isLoadingCategories}
          />
        </div>
      )}

      {currentStep === 2 && createdProductId && (
        <div className="animate-in fade-in slide-in-from-right-4 duration-500">
          <ProductImageUpload productId={createdProductId} onSuccess={() => setCurrentStep(3)} onSkip={() => setCurrentStep(3)} />
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
              <h2 className="text-3xl font-bold">All Set!</h2>
              <p className="text-base-content/60">Your product has been created and updated successfully.</p>
            </div>
            <div className="flex gap-4 mt-4">
              <Link href={`/products/${createdProductId}`} className="btn btn-primary px-8">
                View Product Details
              </Link>
              <button onClick={() => window.location.reload()} className="btn btn-outline">
                Create Another
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
