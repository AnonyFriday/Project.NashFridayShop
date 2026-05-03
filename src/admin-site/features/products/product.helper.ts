import { SelectInputOptions } from "@/features/shared/components/SelectInput";
import { ProductStatus } from "./product.types";

export const ProductStatusHelper = {
  toBadgeClassName(status: ProductStatus): string {
    switch (status) {
      case ProductStatus.InStock:
        return "badge badge-success text-white";
      case ProductStatus.OutOfStock:
        return "badge badge-error text-white";
      case ProductStatus.Discontinued:
        return "badge badge-warning text-white";
      default:
        return "badge badge-ghost";
    }
  },

  getOptions(): SelectInputOptions[] {
    return Object.values(ProductStatus)
      .filter((v) => typeof v === "string")
      .map((status) => ({
        label: status as string,
        value: status as string,
      }));
  },
};
