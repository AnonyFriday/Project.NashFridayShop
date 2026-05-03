"use client";

import { useRouter } from "next/navigation";
import { useCreateProductMutation } from "@/features/products/product.api";
import { useGetCategoriesQuery } from "@/features/categories/category.api";
import ProductForm, { ProductFormData } from "@/features/products/components/ProductForm";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";

export default function NewProductPage() {
  const router = useRouter();
  const dispatch = useAppDispatch();
  const [createProduct, { isLoading: isCreatingProduct }] = useCreateProductMutation();
  const { data: categoriesData, isLoading: isLoadingCategories } = useGetCategoriesQuery({ isAll: true });

  const handleSubmit = async (data: ProductFormData) => {
    try {
      await createProduct(data).unwrap();
      dispatch(
        enqueueToast({
          message: "Product created successfully!",
          type: ToastType.Success,
        }),
      );
      router.push("/products");
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
    <div className="flex flex-col gap-6 p-4">
      <div className="flex items-center gap-4">
        <h1 className="text-2xl font-bold">Add New Product</h1>
      </div>

      <ProductForm
        onSubmit={handleSubmit}
        onCancel={() => router.push("/products")}
        categoriesData={categoriesData?.items || []}
        isLoadingProduct={isCreatingProduct}
        isLoadingCategories={isLoadingCategories}
      />
    </div>
  );
}
