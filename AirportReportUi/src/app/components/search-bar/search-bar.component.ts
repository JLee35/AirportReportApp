import { Component, HostListener, OnInit } from '@angular/core';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css'],
})
export class SearchBarComponent implements OnInit {

  constructor(private airportService: AirportService) { }

  ngOnInit(): void {
  }

  onGoButtonClick() {
    const searchTerm = (document.querySelector('input') as HTMLInputElement).value;
    this.airportService.fetchAirportsFromApi(searchTerm);
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onGoButtonClick();
    }
  }
}
