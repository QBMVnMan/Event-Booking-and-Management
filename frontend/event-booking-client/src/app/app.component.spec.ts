import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  @Component({ selector: 'app-event-card', template: '' })
  class StubEventCardComponent {}

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent, StubEventCardComponent],
      imports: [HttpClientTestingModule, FormsModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should load featured and events from the API', () => {
    const mockFeatured = [ { id: 'f1', name: 'Feat', date: '2026-02-25' } ];
    const mockEvents = [ { id: 'e1', name: 'Event', date: '2026-02-25' } ];

    component.ngOnInit();

    const reqF = httpMock.expectOne('/api/events/featured');
    expect(reqF.request.method).toEqual('GET');
    reqF.flush(mockFeatured);

    const reqE = httpMock.expectOne('/api/events');
    expect(reqE.request.method).toEqual('GET');
    reqE.flush(mockEvents);

    expect(component.featuredEvents).toEqual(mockFeatured);
    expect(component.events).toEqual(mockEvents);
  });
});