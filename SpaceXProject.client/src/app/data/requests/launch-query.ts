import {LaunchStatusFilter} from "../enums/launch-status-filter.enum";
import {SortDirection} from "../enums/sort-direction.enum";

export interface LaunchQueryRequest {
  page: number;
  limit: number;
  search?: string;
  type: LaunchStatusFilter;
  sort: SortDirection;
}
