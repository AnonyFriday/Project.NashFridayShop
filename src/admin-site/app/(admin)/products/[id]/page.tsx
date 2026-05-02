"use client";

import { use } from "react";
import { useGetProductByIdQuery } from "@/features/products/product.api";
import Link from "next/link";
import Image from "next/image";
import GoBackButton from "@/features/layout/components/Buttons/GoBackButton";
import { DeleteButton, EditButton } from "@/features/layout/components/Buttons/ActionButtons";
import { APP_ROUTES } from "@/lib/api/routes";

export default function ProductViewPage({ params }: { params: Promise<{ id: string }> }) {
  // In Next.js 15+, dynamic params are promises that must be unwrapped with React.use()
  const resolvedParams = use(params);

  const {
    data: product,
    isLoading,
    error,
  } = useGetProductByIdQuery({
    id: resolvedParams.id,
    includeDeleted: false,
  });

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <span className="loading loading-spinner loading-lg text-primary"></span>
      </div>
    );
  }

  if (error || !product) {
    return (
      <div className="p-4">
        <div className="alert alert-error">Failed to load product details.</div>
      </div>
    );
  }

  const isInstock = product.status === "InStock";

  return (
    <div className="flex flex-col gap-6 p-4 max-w-6xl mx-auto w-full">
      {/* Header */}
      <GoBackButton href="/products" title="Product Details" />

      {/* Content Area */}
      <div className="grid grid-cols-1 md:grid-cols-[1fr_1.5fr] gap-8 bg-base-100 p-8 rounded-box shadow-sm border border-base-200">
        {/* Left Side: Image */}
        <div className="flex flex-col">
          <div className="bg-base-200 rounded-box p-4 aspect-square relative overflow-hidden flex items-center justify-center">
            {product.imageUrl ? (
              <Image src={product.imageUrl} alt={product.name} fill className="object-cover rounded-box" />
            ) : (
              <span className="text-base-content/40 font-medium">No Image Available</span>
            )}
          </div>
        </div>

        {/* Right Side: Product Info */}
        <div className="flex flex-col gap-5">
          <div>
            <h2 className="text-3xl font-bold text-base-content">{product.name}</h2>
            <div className="text-xs text-base-content/50 mt-2 font-mono bg-base-200 inline-block px-2 py-1 rounded">
              ID: {product.id}
            </div>
          </div>

          <div className="flex items-center gap-4">
            <span className="text-4xl font-black text-primary">${product.priceUsd.toFixed(2)}</span>
            <div
              className={`badge badge-lg font-medium ${isInstock ? "badge-success text-success-content" : "badge-ghost"}`}
            >
              {product.status}
            </div>
          </div>

          <div className="flex items-center gap-2 text-lg">
            <div className="rating rating-sm">
              <input type="radio" className="mask mask-star-2 bg-warning" readOnly checked />
            </div>
            <span className="font-bold text-base-content">{product.averageStars.toFixed(1)}</span>
            <span className="text-sm text-base-content/60">/ 10</span>
          </div>

          <div className="divider my-1"></div>

          <div className="flex-grow">
            <h3 className="font-semibold text-lg">Description</h3>
            <p className="text-base-content/80 leading-relaxed text-justify">
              {product.description ||
                "No detailed description has been provided for this product yet. When added, it will appear here to help customers understand the item better."}
            </p>
          </div>

          <div className="divider my-1"></div>

          {/* Action Buttons */}
          <div className="mt-8 flex gap-3 border-t border-base-200">
            <EditButton
              href={`${APP_ROUTES.PRODUCTS}/${product.id}/edit`}
              title="Edit Product"
              className="btn btn-primary flex-1"
            />
            <DeleteButton title="Delete" className="btn btn-outline btn-error px-8"></DeleteButton>{" "}
          </div>
        </div>
      </div>
    </div>
  );
}
