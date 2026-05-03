"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { SubmitButton, CancelButton } from "@/features/shared/components/Buttons/FormButtons";

export const categorySchema = z.object({
  name: z.coerce.string().min(1, "Category name is required.").max(100, "Category name must not exceed 100 characters."),
  description: z.string().min(1, "Category description is required.").max(300, "Category description must not exceed 300 characters."),
});

export type CategoryFormData = z.infer<typeof categorySchema>;

interface CategoryFormProps {
  initialCategory?: CategoryFormData;
  isLoading?: boolean;
  onSubmit: (data: CategoryFormData) => void;
  onCancel: () => void;
}

export default function CategoryForm({ initialCategory, isLoading, onSubmit, onCancel }: Readonly<CategoryFormProps>) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(categorySchema),
    values: initialCategory,
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="bg-base-100 p-8 rounded-box shadow-sm border border-base-200 flex flex-col gap-6">
      {/* Name */}
      <div className="form-control w-full">
        <label className="label font-semibold">Category Name</label>
        <input
          type="text"
          {...register("name")}
          className={`input input-bordered w-full ${errors.name ? "input-error" : ""}`}
          placeholder="e.g. Electronics, Home & Garden"
        />
        {errors.name && <span className="label-text-alt text-error mt-1">{errors.name.message}</span>}
      </div>

      {/* Description */}
      <div className="form-control w-full">
        <label className="label font-semibold">Description</label>
        <textarea
          {...register("description")}
          className={`textarea textarea-bordered h-32 w-full ${errors.description ? "textarea-error" : ""}`}
          placeholder="Describe what this category is about..."></textarea>
        {errors.description && <span className="label-text-alt text-error mt-1">{errors.description.message}</span>}
      </div>

      <div className="flex gap-4 mt-4">
        <SubmitButton title="Save Category" isLoading={isLoading} />
        <CancelButton title="Cancel" onClick={onCancel} />
      </div>
    </form>
  );
}
