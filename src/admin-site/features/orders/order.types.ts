import { PaginationRequest, PaginationResponse } from "@/lib/types/generic.types"

export enum OrderStatus {
    Pending = "Pending",
    Completed = "Completed",
    Cancelled = "Cancelled",
    Refunded = "Refunded",
    Delivered = "Delivered",
}

export namespace OrderStatus {
    export function toBadgeClassName(status: OrderStatus): string {
        switch (status) {
            case OrderStatus.Pending:
                return "badge-ghost text-base-content/70"
            case OrderStatus.Completed:
                return "badge-success text-white"
            case OrderStatus.Delivered:
                return "badge-info text-white"
            case OrderStatus.Cancelled:
                return "badge-error text-white"
            case OrderStatus.Refunded:
                return "badge-secondary text-white"
            default:
                return "badge-ghost"
        }
    }
}

export enum PaymentStatus {
    Pending = "Pending",
    Paid = "Paid",
    Failed = "Failed",
    Refunded = "Refunded",
}

export namespace PaymentStatus {
    export function toBadgeClassName(status: PaymentStatus): string {
        switch (status) {
            case PaymentStatus.Pending:
                return "badge-ghost text-base-content/70"
            case PaymentStatus.Paid:
                return "badge-success text-white"
            case PaymentStatus.Failed:
                return "badge-error text-white"
            case PaymentStatus.Refunded:
                return "badge-secondary text-white"
            default:
                return "badge-ghost"
        }
    }
}

export namespace GetOrders {
    export interface Request extends PaginationRequest {
        orderStatus?: OrderStatus;
        paymentStatus?: PaymentStatus;
    }

    export interface Item {
        id: string;
        customerFullName: string;
        customerEmail: string;
        deliveryAddress: string;
        phoneNumber: string;
        currency: string;
        totalPriceInUsd: number;
        orderStatus: OrderStatus;
        paymentStatus: PaymentStatus;
        createdAtUtc: string;
    }

    export interface Response extends PaginationResponse<Item> { }
}

export namespace GetOrderById {
    export interface Response {
        id: string;
        customerFullName: string;
        customerEmail: string;
        deliveryAddress: string;
        phoneNumber: string;
        currency: string;
        totalPriceInUsd: number;
        orderStatus: OrderStatus;
        paymentStatus: PaymentStatus;
        createdAtUtc: string;
        items: OrderItemDetail[];
    }

    export interface OrderItemDetail {
        productId: string;
        productName: string;
        categoryId: string;
        categoryName: string;
        quantity: number;
        productUnitPriceInUsd: number;
    }
}
