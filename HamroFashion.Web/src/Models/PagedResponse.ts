export interface PagedResponse<T> {
    pageNumber: Number;
    pageSize: number;
    results: T;
    sortBy?: string;
    sortDescending: boolean
}