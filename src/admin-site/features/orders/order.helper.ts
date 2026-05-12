import { SelectInputOptions } from "@/features/shared/components/SelectInput";
import { OrderStatus, PaymentStatus } from "./order.types";

export const OrderHelper = {
  getOrderStatusOptions(): SelectInputOptions[] {
    return Object.values(OrderStatus)
      .filter((v) => typeof v === "string")
      .map((status) => ({
        label: status as string,
        value: status as string,
      }));
  },

  getPaymentStatusOptions(): SelectInputOptions[] {
    return Object.values(PaymentStatus)
      .filter((v) => typeof v === "string")
      .map((status) => ({
        label: status as string,
        value: status as string,
      }));
  },
};
