import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

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

  private handleError<T>(operation = 'operation', fallback: T) {
    return (err: any): Observable<T> => {
      // proxy will log connection errors before the HTTP client even sees them,
      // but we still surface a warning and fall back to in-memory data so the
      // UI doesn't completely break when the backend isn't running.
      console.warn(`EventService.${operation} failed, returning fallback data:`, err?.message || err);
      return of(fallback);
    };
  }

  getFeatured(): Observable<EventItem[]> {
    // if backend has /featured endpoint
    return this.http.get<EventItem[]>(`${this.base}/featured`)
      .pipe(catchError(this.handleError('getFeatured', [])));
  }

  getEvents(category?: string, query?: string): Observable<EventItem[]> {
    let params = new HttpParams();
    if (category) params = params.set('category', category);
    if (query) params = params.set('q', query);
    return this.http.get<EventItem[]>(this.base, { params })
      .pipe(catchError(this.handleError('getEvents', [])));
  }

  getEventById(id: number) {
  return this.http.get<any>(`/api/events/${id}`);
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
