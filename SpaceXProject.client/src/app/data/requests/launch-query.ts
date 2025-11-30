export interface LaunchQueryRequest {
  page: number;
  limit: number;
  search?: string;
  type: 'all' | 'upcoming' | 'past';
  sort: 'asc' | 'desc';
}
