import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { airportsGuard } from './airports.guard';

describe('searchGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => airportsGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
