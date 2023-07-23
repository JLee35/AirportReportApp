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
        id: "KJFK",
        name: "John F. Kennedy International Airport",
        latitude: "40.6413",
        longitude: "-73.7781",
        weather: {
          temperatureInF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "005",
          windForecasts: [
            {
              timeOffset: "0h:33m",
              windSpeedKts: "10",
              isWindVariable: false,
              windDirectionDegrees: "020"
            },
            {
              timeOffset: "1h:33m",
              windSpeedKts: "15",
              isWindVariable: false,
              windDirectionDegrees: "025"
            }
          ]
        },
        runways: [
          {
            name: "1",
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
        id: "KSFO",
        name: "San Francisco International Airport",
        latitude: "40.6413",
        longitude: "-73.7781",
        weather: {
          temperatureInF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "005",
          windForecasts: [
            {
              timeOffset: "0h:33m",
              windSpeedKts: "10",
              isWindVariable: false,
              windDirectionDegrees: "020"
            },
            {
              timeOffset: "1h:33m",
              windSpeedKts: "15",
              isWindVariable: false,
              windDirectionDegrees: "025"
            }
          ]
        },
        runways: [
          {
            name: "1",
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
        id: "KSEA",
        name: "Seattle-Tacoma International Airport",
        latitude: "40.6413",
        longitude: "-73.7781",
        weather: {
          temperatureInF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "005",
          windForecasts: [
            {
              timeOffset: "0h:33m",
              windSpeedKts: "10",
              isWindVariable: false,
              windDirectionDegrees: "020"
            },
            {
              timeOffset: "1h:33m",
              windSpeedKts: "15",
              isWindVariable: false,
              windDirectionDegrees: "025"
            }
          ]
        },
        runways: [
          {
            name: "1",
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
