import { WindForecast } from "./wind-forecast";

export interface AirportWeatherForecast {
    timeOffset: string;
    windForecasts: WindForecast[];
}