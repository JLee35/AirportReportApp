import { TestBed } from '@angular/core/testing';

import { AirportService } from './airport.service';

describe('AirportService', () => {
  let service: AirportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AirportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // Test getAirportIds().
  it('should return an array of uppercase airport IDs', () => {
    // With no whitespace.
    const ids1 = 'JFK, LAX, SFO';
    const expected = ['JFK', 'LAX', 'SFO'];
    expect(AirportService.getAirportIds(ids1)).toEqual(expected);

    // With whitespace.
    const ids2 = ' JFK,  LAX , SFO    ';
    expect(AirportService.getAirportIds(ids2)).toEqual(expected);

    // With lowercase.
    const ids3 = 'jfk, lax, sfo';
    expect(AirportService.getAirportIds(ids3)).toEqual(expected);

    // Empty.
    const ids4 = '';
    expect(AirportService.getAirportIds(ids4)).toEqual([]);

  });
});
