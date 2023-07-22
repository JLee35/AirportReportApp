import { Injectable } from '@angular/core';
import { InMemoryDbService } from 'angular-in-memory-web-api';
import { Airport } from './airport'

@Injectable({
  providedIn: 'root'
})
export class InMemoryDataService implements InMemoryDbService {

  createDb() {
    const airports = [
      { id: "KJFK", name: "John F. Kennedy International Airport", latitude: "40.6413", longitude: "-73.7781" },
      { id: "KLGA", name: "LaGuardia Airport", latitude: "40.7769", longitude: "-73.8740" },
      { id: "KEWR", name: "Newark Liberty International Airport", latitude: "40.6895", longitude: "-74.1745" },
      { id: "KTEB", name: "Teterboro Airport", latitude: "40.8506", longitude: "-74.0608" },
      { id: "KHPN", name: "Westchester County Airport", latitude: "41.0669", longitude: "-73.7076" },
      { id: "KISP", name: "Long Island MacArthur Airport", latitude: "40.7953", longitude: "-73.1002" },
    ]

    return { airports };
  }
}
