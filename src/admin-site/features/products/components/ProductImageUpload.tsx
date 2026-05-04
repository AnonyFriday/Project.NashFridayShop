"use client";

import Image from "next/image";
import React, { useState } from "react";
import { useUpdateProductImageMutation } from "../product.api";
import { useAppDispatch } from "@/lib/redux/hooks";
import { enqueueToast, ToastType } from "@/features/shared/toast.slice";

interface ProductImageUploadProps {
  productId: string;
  onSuccess: () => void;
  onSkip: () => void;
}

export const ProductImageUpload = ({ productId, onSuccess, onSkip }: ProductImageUploadProps) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [updateImage, { isLoading }] = useUpdateProductImageMutation();
  const dispatch = useAppDispatch();

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setSelectedFile(file);
      setPreviewUrl(URL.createObjectURL(file)); // create the preview url based on the submitted image
    }
  };

  const handleUpload = async () => {
    if (!selectedFile) return;

    try {
      await updateImage({ productId, imageFile: selectedFile }).unwrap();
      dispatch(
        enqueueToast({
          message: "Image uploaded successfully!",
          type: ToastType.Success,
        }),
      );
      onSuccess();
    } catch (err) {
      console.error(err);
      dispatch(
        enqueueToast({
          message: "Failed to upload image.",
          type: ToastType.Error,
        }),
      );
    }
  };

  return (
    <div className="card bg-base-100 shadow-xl border border-base-200">
      <div className="card-body gap-6 items-center text-center">
        <h2 className="card-title text-2xl">Step 2: Upload Product Image</h2>
        <p className="text-base-content/70">Almost there! Now add a great picture for your product.</p>

        <div className="flex flex-col items-center gap-4 w-full max-w-md">
          {previewUrl ? (
            <div className="relative w-full aspect-square max-w-300px rounded-xl overflow-hidden border-2 border-primary border-dashed p-1">
              <Image src={previewUrl} alt="Preview" fill className="object-cover rounded-lg" />
            </div>
          ) : (
            <div className="w-full aspect-square max-w-300px rounded-xl border-2 border-base-300 border-dashed flex flex-col items-center justify-center gap-2 bg-base-200/50">
              <svg xmlns="http://www.w3.org/2000/svg" className="h-12 w-12 text-base-content/30" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                />
              </svg>
              <span className="text-sm text-base-content/50">No image selected</span>
            </div>
          )}

          <input
            id="id-upload-image"
            type="file"
            className="file-input file-input-bordered file-input-primary w-full"
            onChange={handleFileChange}
            accept="image/*"
          />
        </div>

        <div className="card-actions justify-center gap-4 mt-4">
          <button className="btn btn-ghost" onClick={onSkip} disabled={isLoading} type="button">
            Skip for now
          </button>
          <button className="btn btn-primary px-8" onClick={handleUpload} disabled={!selectedFile || isLoading} type="button">
            {isLoading ? (
              <>
                <span className="loading loading-spinner"></span>
                Uploading...
              </>
            ) : (
              "Upload & Continue"
            )}
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductImageUpload;
