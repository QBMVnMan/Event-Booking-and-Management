import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EventService, EventItem } from './event.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrls: ['./app.component.css']
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

   search() {
    if (this.searchTerm.trim()) {
      this.router.navigate(['/'], { 
        queryParams: { search: this.searchTerm.trim() } 
      });
    }
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
