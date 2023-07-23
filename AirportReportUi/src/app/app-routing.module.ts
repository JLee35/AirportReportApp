import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AirportsComponent } from './components/airports/airports.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { airportsGuard } from './guards/airports.guard';

const routes: Routes = [
  { path: '', redirectTo: '/landing', pathMatch: 'full' },
  { path: 'landing', component: LandingPageComponent },
  { path: 'airports', component: AirportsComponent, canActivate: [airportsGuard] }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
