import { AirportWeatherForecast } from "./airport-weather-forecast";

export interface AirportWeather {
    temperatureF: string;
    relativeHumidity: string;
    cloudCoverage: string;
    visibilitySm: string;
    windSpeedMph: string;
    windDirection: string;

    weatherForecast: AirportWeatherForecast;
}