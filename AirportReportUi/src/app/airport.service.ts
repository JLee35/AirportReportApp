import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Airport } from './airport';

@Injectable({
  providedIn: 'root'
})
export class AirportService {

  private airportsUrl = "app/airports";
  private airportId = "";


  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  setAirportId(id: string) {
    this.airportId = id;
  }

  getAirports(): Observable<Airport[]> {
    return this.http.get<Airport[]>(this.airportsUrl);
  }

  getAirport(): Observable<Airport> {
    const url = `${this.airportsUrl}/${this.airportId}`;
    return this.http.get<Airport>(url);
  }
}
