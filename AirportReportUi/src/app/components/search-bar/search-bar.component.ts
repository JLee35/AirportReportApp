import { Component, HostListener, OnInit } from '@angular/core';
import { AirportService } from '../../services/airport.service';
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css'],
})
export class SearchBarComponent implements OnInit {
  loading$: Observable<boolean> = new Observable<boolean>();
  private loadingSubject = new Subject<boolean>();

  constructor(private airportService: AirportService) { }

  ngOnInit(): void {
    this.loadingSubject.next(false);
    this.loading$ = this.loadingSubject.asObservable();
  }

  onGoButtonClick() {
    const searchTerm = (document.querySelector('input') as HTMLInputElement).value;
    this.loadingSubject
    this.airportService.fetchAirportsFromApi(searchTerm).subscribe(
      () => {
        this.loadingSubject.next(false);
      },
      (error) => {
        this.loadingSubject.next(false);
        console.log(error);
      }
    )
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onGoButtonClick();
    }
  }
}
