import { Component, HostListener } from '@angular/core';
import { AirportService } from '../airport.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css'],
})
export class SearchBarComponent {

  constructor(private airportService: AirportService, private router: Router) { }

  onGoButtonClick() {
    const searchTerm = (document.querySelector('input') as HTMLInputElement).value;
    this.airportService.setAirportId(searchTerm);
    this.router.navigate(['/airports']);
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onGoButtonClick();
    }
  }
}
