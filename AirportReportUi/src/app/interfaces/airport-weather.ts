import { WindForecast } from "./wind-forecast";

export interface AirportWeather {
    temperatureF: string;
    relativeHumidity: string;
    cloudCoverage: string;
    visibilitySm: string;
    windSpeedMph: string;
    windDirection: string;

    windForecasts: WindForecast[];
}