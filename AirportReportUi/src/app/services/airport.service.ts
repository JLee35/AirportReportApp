import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Airport } from '../interfaces/airport';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AirportService {

  // private airportsUrl = "app/airports";
  private airportsUrl = "http://localhost:7051";
  private airports: Airport[] = [];

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private router: Router) { }

  public fetchAirportsFromApi(commaSeparatedIds: string): void {
    this.airports = [];
    let airportIds: string[] = AirportService.getAirportIds(commaSeparatedIds);

    let response = this.http.post<Airport[]>(this.airportsUrl, { airportIds }, this.httpOptions);
    response.subscribe(airports => {
      this.airports = airports;
      this.router.navigate(['/airports']);
    });

    // this.http.get<Airport[]>(this.airportsUrl, this.httpOptions).subscribe(airports => {
    //   this.airports = airports.filter(airport => commaSeparatedIds.includes(airport.id));
    // });
  }

  public getAirports(): Airport[] {
    return this.airports;
  }

  public hasAirports(): boolean {
    return this.airports.length > 0;
  }

  public static getAirportIds(ids: string): string[] {
    // Parse the comma-separated list of airport IDs, trimming any whitespace and casting to uppercase.
    return ids.split(',').map(id => id.trim().toUpperCase());
  }
}
