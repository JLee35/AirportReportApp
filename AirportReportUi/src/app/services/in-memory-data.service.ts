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
          temperatureF: "32",
          relativeHumidity: "23",
          cloudCoverage: "Skies few at 10000",
          visibilitySm: "10",
          windSpeedMph: "6",
          windDirection: "005",
          weatherForecast: {
            timeOffset: "1h:30m"
          }
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
