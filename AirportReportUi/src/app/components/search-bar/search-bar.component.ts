import { Component, HostListener, OnInit } from '@angular/core';
import { AirportService } from '../../services/airport.service';
import { catchError, of } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css'],
})
export class SearchBarComponent implements OnInit {
  inputValue = '';
  isButtonDisabled = true;

  errorMessage = '';
  isErrorVisible = false;

  constructor(private airportService: AirportService) { }

  ngOnInit(): void {
    this.hideLoader();
  }

  onGoButtonClick() {
    const searchTerm = (document.querySelector('input') as HTMLInputElement).value;
    this.disableGoButton();
    this.showLoader();

    this.airportService.fetchAirportsFromApi(searchTerm).pipe(
      catchError(error => {
        this.errorMessage = `An error occured: ${error.error}`;
        // alert('An error occurred: ' + error.error);
        this.isErrorVisible = true;
        return of([]);
      })).subscribe(() => {
        this.hideLoader();
        this.enableGoButton();
      });
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter' && !this.isButtonDisabled) {
      this.onGoButtonClick();
    }
  }

  updateButtonState() {
    this.isButtonDisabled = this.inputValue.trim().length === 0;
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
