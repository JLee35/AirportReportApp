import { Component, OnInit } from '@angular/core';
import { Airport } from '../../interfaces/airport';
import { AirportService } from '../../services/airport.service';


@Component({
  selector: 'app-airports',
  templateUrl: './airports.component.html',
  styleUrls: ['./airports.component.css']
})
export class AirportsComponent implements OnInit {

  airports: Airport[] = [];

  constructor(private airportService: AirportService) { }

  ngOnInit(): void {
    this.getAirports();
  }

  hasAirports(): boolean {
    return this.airports.length > 0;
  }

  private getAirports(): void {
    this.airports = [];
    this.airports.concat(this.airportService.getAirports());
  }
}
