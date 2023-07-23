import { Component, HostListener, OnInit } from '@angular/core';
import { AirportService } from '../../services/airport.service';
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css'],
})
export class SearchBarComponent implements OnInit {

  constructor(private airportService: AirportService) { }

  ngOnInit(): void {
    this.hideLoader();
  }

  onGoButtonClick() {
    const searchTerm = (document.querySelector('input') as HTMLInputElement).value;
    this.disableGoButton();
    this.showLoader();

    this.airportService.fetchAirportsFromApi(searchTerm).subscribe(
      () => {
        this.hideLoader();
        this.enableGoButton();
      },
      (error) => {
        this.hideLoader();
        this.enableGoButton();
        alert(error);
      }
    )
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onGoButtonClick();
    }
  }

  private showLoader() {
    document.getElementById('loadingDiv')!.style.display = 'block';
  }

  private hideLoader() {
    document.getElementById('loadingDiv')!.style.display = 'none';
  }

  private disableGoButton() {
    (document.getElementById('goButton') as HTMLButtonElement).disabled = true;
  }

  private enableGoButton() {
    (document.getElementById('goButton') as HTMLButtonElement).disabled = false;
  }
}
