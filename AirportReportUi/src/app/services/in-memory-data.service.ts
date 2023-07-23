import { Injectable } from '@angular/core';
import { InMemoryDbService } from 'angular-in-memory-web-api';
import { Airport } from '../interfaces/airport'

@Injectable({
  providedIn: 'root'
})
export class InMemoryDataService implements InMemoryDbService {

  createDb() {
    const airports = [
      {
        identifier: "KJFK",
        name: "John F. Kennedy International Airport",
        latitude: "N47°37.14'",
        longitude: "W117°32.11'",
        weather: {
          temperatureInF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "SW",
          windForecasts: [
            {
              timeOffset: "0h:33m",
              windSpeedMph: "10",
              isWindVariable: false,
              windDirectionDegrees: "020"
            },
            {
              timeOffset: "1h:33m",
              windSpeedMph: "15",
              isWindVariable: false,
              windDirectionDegrees: "025"
            }
          ]
        },
        runways: [
          {
            name: "01",
            magneticHeading: "010",
            isBestRunway: false
          },
          {
            name: "19",
            magneticHeading: "190",
            isBestRunway: true
          }
        ]
      },
      {
        identifier: "KSFO",
        name: "San Francisco International Airport",
        latitude: "N47°37.14'",
        longitude: "W117°32.11'",
        weather: {
          temperatureInF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "N",
          windForecasts: [
            {
              timeOffset: "0h:33m",
              windSpeedMph: "10",
              isWindVariable: false,
              windDirectionDegrees: "020"
            },
            {
              timeOffset: "1h:33m",
              windSpeedMph: "15",
              isWindVariable: false,
              windDirectionDegrees: "025"
            }
          ]
        },
        runways: [
          {
            name: "01",
            magneticHeading: "010",
            isBestRunway: false
          },
          {
            name: "19",
            magneticHeading: "190",
            isBestRunway: true
          }
        ]
      },
      {
        identifier: "KSEA",
        name: "Seattle-Tacoma International Airport",
        latitude: "N47°37.14'",
        longitude: "W117°32.11'",
        weather: {
          temperatureInF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "NE",
          windForecasts: [
            {
              timeOffset: "0h:33m",
              windSpeedMph: "10",
              isWindVariable: false,
              windDirectionDegrees: "020"
            },
            {
              timeOffset: "1h:33m",
              windSpeedMph: "15",
              isWindVariable: false,
              windDirectionDegrees: "025"
            }
          ]
        },
        runways: [
          {
            name: "01",
            magneticHeading: "010",
            isBestRunway: false
          },
          {
            name: "19",
            magneticHeading: "190",
            isBestRunway: true
          }
        ]
      }
    ];

    return { airports };
  }
}
