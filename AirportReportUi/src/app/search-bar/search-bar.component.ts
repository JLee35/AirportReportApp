import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css'],
})
export class SearchBarComponent {
  onGoButtonClick() {
    const searchTerm = (document.querySelector('input') as HTMLInputElement).value;
    console.log(`Performing serach for: ${searchTerm}`);
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onGoButtonClick();
    }
  }
}
