import { Component } from '@angular/core';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent {
  pageTitle = 'Going somewhere?';
  pageSubtitle = 'Search for airports by ICAO separated by spaces';
}
