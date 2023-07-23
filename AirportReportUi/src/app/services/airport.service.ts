import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Airport } from '../interfaces/airport';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AirportService {

  private airportsUrl = "app/airports";
  private airportId = "";
  private airport: Airport | undefined;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private router: Router) { }

  setAirportId(id: string) {
    this.airportId = id;
    this.setAirport(id);
  }

  setAirport(id: string) {
    const url = `${this.airportsUrl}/${id}`;
    let response = this.http.get<Airport>(url);
    response.subscribe((airport) => {
      this.airport = airport;
      this.router.navigate(['/airports']);
    });
  }

  getAirports(): Observable<Airport[]> {
    return this.http.get<Airport[]>(this.airportsUrl);
  }

  getAirport(): Airport {
    return this.airport as Airport;
  }

  hasAirport(): boolean {
    return this.airport != null;
  }

}
