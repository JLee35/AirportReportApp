import { Component, OnInit } from '@angular/core';
import { Airport } from '../../interfaces/airport';
import { AirportService } from '../../services/airport.service';
import { Location } from '@angular/common';
import { DecimalPipe } from '@angular/common';


@Component({
  selector: 'app-airports',
  templateUrl: './airports.component.html',
  styleUrls: ['./airports.component.css'],
  providers: [DecimalPipe]
})
export class AirportsComponent implements OnInit {

  airports: Airport[] = [];

  constructor(
    private airportService: AirportService,
    private location: Location,
    private decimalPipe: DecimalPipe) { }

  ngOnInit(): void {
    this.airports = [];
    this.getAirports();
  }

  hasAirports(): boolean {
    return this.airports.length > 0;
  }

  private getAirports(): void {
    this.airports = this.airportService.getAirports();
  }

  goBack(): void {
    this.location.back();
  }
}
