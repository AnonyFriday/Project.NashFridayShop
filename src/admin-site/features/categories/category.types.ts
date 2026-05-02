import { PaginationRequest, PaginationResponse } from "@/lib/types/generic.types"

export namespace GetCategories {
    export interface Request extends PaginationRequest {
        searchName?: string;
        isAll?: boolean;
    }

    export interface Item {
        id: string;
        name: string;
        description: string;
    }

    export interface Response extends PaginationResponse<Item> { }
}
