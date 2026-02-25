import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EventService } from './event.service';

interface EventItem {
  id: string;
  name: string;
  date: string;
  venue?: string;
  poster?: string;
  minPrice?: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Ticketbox-style Demo';

  featuredEvents: EventItem[] = [];
  events: EventItem[] = [];
  categories = ['Nhạc sống','Hội thảo','Thể thao','Phim','Kịch','Voucher'];
  selectedCategory = '';
  lang = 'Tiếng Việt';

  searchTerm = '';

  constructor(private eventService: EventService, private router: Router) {}

  ngOnInit() {
    this.loadFeatured();
    this.loadEvents();
  }

  loadFeatured() {
    this.eventService.getFeatured().subscribe({
      next: (res: EventItem[]) => this.featuredEvents = res,
      error: (err: any) => console.error('Failed to load featured', err)
    });
  }

  loadEvents(category?: string, query?: string) {
    this.eventService.getEvents(category, query).subscribe({
      next: (res: EventItem[]) => this.events = res,
      error: (err: any) => console.error('Failed to load events', err)
    });
  }

  selectCategory(cat: string) {
    this.selectedCategory = cat;
    this.loadEvents(cat, this.searchTerm);
  }

  buyTicket(id: string) {
    // navigate to event detail / booking
    this.router.navigate(['/events', id]);
  }

  login() {
    alert('Đăng nhập (tạm)');
  }
}
