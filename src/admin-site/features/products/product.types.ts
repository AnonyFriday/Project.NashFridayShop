import { PaginationRequest, PaginationResponse } from "@/lib/types/generic.types"

// === Shared

export enum ProductStatus {
    InStock = "InStock",
    OutOfStock = "OutOfStock",
    Discontinued = "Discontinued",
}

export namespace ProductStatus {
    export function toBadgeClassName(status: ProductStatus): string {
        switch (status) {
            case ProductStatus.InStock:
                return "badge badge-success text-white"
            case ProductStatus.OutOfStock:
                return "badge badge-error text-white"
            case ProductStatus.Discontinued:
                return "badge badge-warning text-white"
            default:
                return "badge badge-ghost"
        }
    }
}

// === APIs

export namespace GetProducts {

    export interface Request extends PaginationRequest {
        categoryId?: string
        searchName?: string
        minPrice?: number
        maxPrice?: number
        status?: ProductStatus
        includeDeleted?: boolean
    }

    export interface Item {
        id: string
        name: string
        imageUrl: string
        priceUsd: number
        status: ProductStatus
        averageStars: number
        quantity: number
        isDeleted: boolean
    }

    export interface Response extends PaginationResponse<Item> { }
}

export namespace GetProductById {
    export interface Request {
        id: string;
        includeDeleted?: boolean;
    }

    export type Response = {
        description: string;
        categoryId: string;
        categoryName?: string;
        id: string
        name: string
        imageUrl: string
        priceUsd: number
        status: ProductStatus
        averageStars: number
        quantity: number
    };
}

export namespace UpdateProduct {
    export interface Request {
        id: string;
        includeDeleted?: boolean;
        body: {
            categoryId: string;
            name: string;
            description: string;
            priceUsd: number;
            quantity: number;
            status: ProductStatus;
        }
    }

    export interface Response {
        id: string;
        categoryId: string;
        name: string;
        description: string;
        priceUsd: number;
        imageUrl: string;
        quantity: number;
        status: ProductStatus;
        updatedAtUtc: string;
    }
}

export namespace CreateProduct {
    export interface Request {
        categoryId: string;
        name: string;
        description: string;
        priceUsd: number;
        quantity: number;
        status: ProductStatus;
    }

    export interface Response {
        id: string;
        categoryId: string;
        name: string;
        description: string;
        priceUsd: number;
        imageUrl: string;
        quantity: number;
        status: ProductStatus;
        createdAtUtc: string;
    }
}
