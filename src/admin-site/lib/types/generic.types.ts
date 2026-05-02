export interface PaginationRequest {
    pageIndex?: number;
    pageSize?: number;
}

export interface PaginationResponse<T> {
    items: T[];
    totalItems: number;
    totalPages: number;
    pageIndex: number;
}