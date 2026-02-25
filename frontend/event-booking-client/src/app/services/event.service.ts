import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  private apiUrl = environment.apiUrl + '/api/events';

  constructor(private http: HttpClient) { }

  getFeaturedEvents(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/featured`);
  }

  getEvents(query = ''): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}${query}`);
  }

}
