import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

export interface EventItem {
  id: string;
  name: string;
  date: string;
  venue?: string;
  poster?: string;
  minPrice?: number;
}

@Injectable({ providedIn: 'root' })
export class EventService {
  // Simple base - adjust to your API gateway/proxy
  private base = '/api/events';

  constructor(private http: HttpClient) {}

  getFeatured(): Observable<EventItem[]> {
    // if backend has /featured endpoint
    return this.http.get<EventItem[]>(`${this.base}/featured`);
  }

  getEvents(category?: string, query?: string): Observable<EventItem[]> {
    let params = new HttpParams();
    if (category) params = params.set('category', category);
    if (query) params = params.set('q', query);
    return this.http.get<EventItem[]>(this.base, { params });
  }

  // Fallback local sample (not used by default)
  sampleEvents(): Observable<EventItem[]> {
    const sample: EventItem[] = [
      { id: '1', name: 'Concert A', date: new Date().toISOString(), venue: 'Hanoi', poster: '', minPrice: 100000 },
      { id: '2', name: 'Football Match', date: new Date().toISOString(), venue: 'HCMC', poster: '', minPrice: 200000 }
    ];
    return of(sample);
  }
}
