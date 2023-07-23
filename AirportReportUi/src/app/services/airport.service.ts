import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Airport } from '../interfaces/airport';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AirportService {

  // private airportsUrl = "app/airports";
  private airportsUrl = "https://localhost:7051/Airport/multiple";
  private airports: Airport[] = [];

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private router: Router) { }

  public fetchAirportsFromApi(commaSeparatedIds: string): void {
    this.airports = [];
    let airportIds: string = AirportService.getAirportIdsAsSanitizedJsonString(commaSeparatedIds);

    let response = this.http.post<Airport[]>(this.airportsUrl, airportIds, this.httpOptions);
    response.subscribe(airports => {
      this.airports = airports;
      this.router.navigate(['/airports']);
    });

    // this.http.get<Airport>("https://localhost:7051/Airport/KGEG", this.httpOptions).subscribe(airport => {
    //   this.airports.push(airport);
    //   this.router.navigate(['/airports']);
    // });
  }

  public getAirports(): Airport[] {
    return this.airports;
  }

  public hasAirports(): boolean {
    return this.airports.length > 0;
  }

  public static formatIdsToJSONString(ids: string[]): string {
    const formattedIds = ids.map(id => `"${id}"`).join(',');
    return `[${formattedIds}]`;
  }

  private static getAirportIdsAsSanitizedJsonString(ids: string): string {
    let airportIds: string[] = AirportService.sanitizeIds(ids.split(','));
    return AirportService.formatIdsToJSONString(airportIds);
  }

  private static sanitizeIds(ids: string[]): string[] {
    // Remove any empty strings.
    let sanitizedIds = ids.filter(id => id.length > 0);

    // Trim whitespace.
    sanitizedIds = sanitizedIds.map(id => id.trim());

    // Cast to uppercase.
    sanitizedIds = sanitizedIds.map(id => id.toUpperCase());

    // Remove any duplicates.
    sanitizedIds = sanitizedIds.filter((id, index) => sanitizedIds.indexOf(id) === index);

    return sanitizedIds;
  }

}
