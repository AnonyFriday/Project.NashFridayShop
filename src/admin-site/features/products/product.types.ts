import { PaginationRequest, PaginationResponse } from "@/lib/types/generic.types"

// === Shared

export enum ProductStatus {
    InStock = "InStock",
    OutOfStock = "OutOfStock",
    Draft = "Draft",
}

// === APIs

// eslint-disable-next-line @typescript-eslint/no-namespace
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
    }

    // eslint-disable-next-line @typescript-eslint/no-empty-object-type
    export interface Response extends PaginationResponse<Item> { }
}

// eslint-disable-next-line @typescript-eslint/no-namespace
export namespace GetProductById {
    export interface Request {
        id: string;
        includeDeleted?: boolean;
    }

    export type Response = {
        description?: string;
        categoryId?: string;
        id: string
        name: string
        imageUrl: string
        priceUsd: number
        status: ProductStatus
        averageStars: number
    };
}
