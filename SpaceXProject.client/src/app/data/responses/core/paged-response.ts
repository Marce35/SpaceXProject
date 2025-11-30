export interface PagedResponse<T> {
    docs: T[];
    totalDocs: number;
    page: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPrevPage: boolean;
}
