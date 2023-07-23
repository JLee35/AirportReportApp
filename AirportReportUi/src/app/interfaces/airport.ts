import { AirportWeather } from "./airport-weather";
import { Runway } from "./runway";

export interface Airport {
    id: string;
    name: string;
    latitude: string;
    longitude: string;
    weather: AirportWeather;
    runways: Runway[];
}