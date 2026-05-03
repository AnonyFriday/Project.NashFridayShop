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
export namespace CreateCategory {
    export interface Request {
        name: string;
        description: string;
    }

    export interface Response {
        id: string;
        name: string;
        description: string;
    }
}

export namespace UpdateCategory {
    export interface Request {
        id: string;
        body: {
            name: string;
            description: string;
        };
    }

    export interface Response {
        id: string;
        name: string;
        description: string;
    }
}

export namespace GetCategoryById {
    export interface Request {
        id: string;
    }

    export interface Response {
        id: string;
        name: string;
        description: string;
    }
}
