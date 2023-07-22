import { Component, OnInit } from '@angular/core';
import { Airport } from '../airport';
import { AirportService } from '../airport.service';


@Component({
  selector: 'app-airports',
  templateUrl: './airports.component.html',
  styleUrls: ['./airports.component.css']
})
export class AirportsComponent implements OnInit {

  airports: Airport[] = [];

  constructor(private airportService: AirportService) { }

  ngOnInit(): void {
    console.log("airport component ngOnInit");
    this.getAirports();
  }

  getAirports(): void {
    this.airportService.getAirports()
      .subscribe(airports => this.airports = airports);
  }
}
