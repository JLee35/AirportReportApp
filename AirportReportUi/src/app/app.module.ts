import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';

import { HttpClientModule } from '@angular/common/http';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
import { InMemoryDataService } from './services/in-memory-data.service';
import { AirportsComponent } from './components/airports/airports.component';
import { AppRoutingModule } from './app-routing.module';

import { config } from './config';

@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    SearchBarComponent,
    AirportsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,

    // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
    // and returns simulated server responses.
    // A flag in the app config file, src/app/config.ts, determines 
    //whether the app in memory web API is used.
    config.useInMemoryApi ?
      HttpClientInMemoryWebApiModule
        .forRoot(InMemoryDataService, { dataEncapsulation: false }) : [],
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
