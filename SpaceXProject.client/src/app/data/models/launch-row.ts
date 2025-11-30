import {SpaceXRocket} from "./spacex-rocket";

export interface SpaceXLaunchRow {
    id: string;
    name: string;
    rawDate: string;
    formattedDate: string;
    success: boolean | null;
    upcoming: boolean;
    rocketName: string;
    rocketDetails: SpaceXRocket;
}
