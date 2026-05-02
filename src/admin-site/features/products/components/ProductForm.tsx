"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { ProductStatus } from "../product.types";
import { SubmitButton, CancelButton } from "@/features/layout/components/Buttons/FormButtons";
import { GetCategories } from "@/features/categories/category.types";

export const productSchema = z.object({
  name: z.coerce.string().min(1, "Product name is required.").max(255, "Product name must not exceed 255 characters."),
  description: z.string().min(1, "Product description is required.").max(1000, "Product description must not exceed 1000 characters.").optional(),
  priceUsd: z.number({ message: "Price must be a number." }).gt(0, "Price must be greater than 0."),
  imageUrl: z.string().url("Must be a valid URL.").min(1, "Image URL is required."),
  quantity: z.number({ message: "Quantity must be a number." }).int().min(0, "Quantity must be greater than or equal to 0."),
  status: z.enum([ProductStatus.InStock, ProductStatus.OutOfStock, ProductStatus.Discontinued]),
  categoryId: z.string().uuid("Invalid Category ID.").min(1, "Category Id is required."),
});

export type ProductFormData = z.infer<typeof productSchema>;

interface ProductFormProps {
  initialProduct?: ProductFormData;
  categoriesData: GetCategories.Item[];
  isLoading?: boolean;
  isLoadingCategories?: boolean;
  onSubmit: (data: ProductFormData) => void;
  onCancel: () => void;
}

export default function ProductForm({ initialProduct, categoriesData, onSubmit, onCancel, isLoading, isLoadingCategories }: ProductFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(productSchema),
    values: initialProduct,
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="bg-base-100 p-8 rounded-box shadow-sm border border-base-200 flex flex-col gap-6">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Name */}
        <div className="form-control w-full">
          <label className="label font-semibold">Product Name</label>
          <input type="text" {...register("name")} className={`input input-bordered w-full ${errors.name ? "input-error" : ""}`} />
          {errors.name && <span className="label-text-alt text-error mt-1">{errors.name.message}</span>}
        </div>

        {/* Category */}
        <div className="form-control w-full">
          <label className="label font-semibold">Category</label>
          <select {...register("categoryId")} className={`select select-bordered w-full ${errors.categoryId ? "select-error" : ""}`} disabled={isLoadingCategories}>
            <option value="" disabled>
              Select a category
            </option>
            {categoriesData?.map((cat) => (
              <option key={cat.id} value={cat.id}>
                {cat.name}
              </option>
            ))}
          </select>
          {errors.categoryId && <span className="label-text-alt text-error mt-1">{errors.categoryId.message}</span>}
        </div>

        {/* Price */}
        <div className="form-control w-full">
          <label className="label font-semibold">Price (USD)</label>
          <input type="number" step="0.01" {...register("priceUsd", { valueAsNumber: true })} className={`input input-bordered w-full ${errors.priceUsd ? "input-error" : ""}`} />
          {errors.priceUsd && <span className="label-text-alt text-error mt-1">{errors.priceUsd.message}</span>}
        </div>

        {/* Quantity */}
        <div className="form-control w-full">
          <label className="label font-semibold">Quantity</label>
          <input type="number" {...register("quantity", { valueAsNumber: true })} className={`input input-bordered w-full ${errors.quantity ? "input-error" : ""}`} />
          {errors.quantity && <span className="label-text-alt text-error mt-1">{errors.quantity.message}</span>}
        </div>

        {/* Status */}
        <div className="form-control w-full">
          <label className="label font-semibold">Status</label>
          <select {...register("status")} className={`select select-bordered w-full ${errors.status ? "select-error" : ""}`}>
            {Object.values(ProductStatus)
              .filter((v) => typeof v === "string")
              .map((status) => (
                <option key={status} value={status}>
                  {status}
                </option>
              ))}
          </select>
          {errors.status && <span className="label-text-alt text-error mt-1">{errors.status.message}</span>}
        </div>

        {/* Image URL */}
        <div className="form-control w-full">
          <label className="label font-semibold">Image URL</label>
          <input type="text" {...register("imageUrl")} className={`input input-bordered w-full ${errors.imageUrl ? "input-error" : ""}`} />
          {errors.imageUrl && <span className="label-text-alt text-error mt-1">{errors.imageUrl.message}</span>}
        </div>
      </div>

      {/* Description */}
      <div className="form-control w-full">
        <label className="label font-semibold">Description</label>
        <textarea {...register("description")} className={`textarea textarea-bordered h-32 w-full ${errors.description ? "textarea-error" : ""}`}></textarea>
        {errors.description && <span className="label-text-alt text-error mt-1">{errors.description.message}</span>}
      </div>

      <div className="flex gap-4 mt-4">
        <SubmitButton title="Save Changes" isLoading={isLoading} />
        <CancelButton title="Cancel" onClick={onCancel} />
      </div>
    </form>
  );
}
