import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AirportsComponent } from './airports.component';

describe('AirportsComponent', () => {
  let component: AirportsComponent;
  let fixture: ComponentFixture<AirportsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AirportsComponent]
    });
    fixture = TestBed.createComponent(AirportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
