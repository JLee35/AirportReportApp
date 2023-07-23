import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { AirportService } from '../services/airport.service';
import { Router } from '@angular/router';

export const airportsGuard: CanActivateFn = (route, state) => {
  // If the request is going to the /airports route, then we need 
  // to ensure it came from the 'onGoButtonClick' method in the
  // SearchBarComponent. If it didn't, then we need to redirect
  // the user to the landing page.

  if (inject(AirportService).hasAirports()) {
    return true;
  }

  return inject(Router).navigate(['/landing']);
};
