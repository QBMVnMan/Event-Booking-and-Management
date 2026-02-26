import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  // Use a relative path so the dev server proxy (src/proxy.conf.js) forwards requests.
  private base = '/api/events';

  constructor(private http: HttpClient) { }

  getFeaturedEvents(): Observable<any[]> {
    return this.http.get<any[]>(`${this.base}/featured`);
  }

  getEvents(query = ''): Observable<any[]> {
    return this.http.get<any[]>(`${this.base}${query}`);
  }

}
