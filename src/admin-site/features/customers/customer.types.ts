export namespace GetCustomers {
  export interface Request {
    searchName?: string;
    pageIndex: number;
    pageSize: number;
    includeDeleted?: boolean;
  }

  export interface Item {
    id: string;
    fullName: string;
    userName: string;
    email: string;
    address: string;
    isDeleted: boolean;
    createdAtUtc: string;
  }

  export interface Response {
    items: Item[];
    totalItems: number;
    totalPages: number;
    pageIndex: number;
  }
}
