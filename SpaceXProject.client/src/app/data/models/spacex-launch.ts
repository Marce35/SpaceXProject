import {SpaceXRocket} from "./spacex-rocket";
import {SpaceXCore} from "./spacex-core";

export interface SpaceXLaunch {
  id: string;
  flight_number: number;
  name: string;
  date_utc: string;
  success: boolean | null;
  details: string;
  upcoming: boolean;
  rocket: SpaceXRocket;
  cores: SpaceXCore[];
}
