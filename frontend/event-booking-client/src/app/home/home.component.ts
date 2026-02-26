import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventService } from '../event.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  featuredEvents: any[] = [];
  events: any[] = [];
  categories = [
    'Nhạc sống', 'Hội thảo & Workshop', 'Thể thao',
    'Phim & Điện ảnh', 'Kịch & Nghệ thuật', 'Voucher & Khác'
  ];
  selectedCategory: string | null = null;

  responsiveOptions = [
    { breakpoint: '1400px', numVisible: 4, numScroll: 1 },
    { breakpoint: '1024px', numVisible: 3, numScroll: 1 },
    { breakpoint: '768px', numVisible: 2, numScroll: 1 },
    { breakpoint: '560px', numVisible: 1, numScroll: 1 }
  ];

  constructor(private eventService: EventService) {}

  ngOnInit() {
    this.loadFeatured();
    this.loadEvents();
  }

  loadFeatured() {
    this.eventService.getFeatured().subscribe({
      next: (data) => this.featuredEvents = data,
      error: (err) => console.error('Featured events error', err)
    });
  }

  loadEvents() {
    const category = this.selectedCategory ? this.selectedCategory : undefined;
    this.eventService.getEvents(category).subscribe({
      next: (data) => this.events = data,
      error: (err) => console.error('Events error', err)
    });
  }

  selectCategory(cat: string) {
    this.selectedCategory = (this.selectedCategory === cat) ? null : cat;
    this.loadEvents();
  }

  onBuy(ev: any) {
    // placeholder for buy action
    alert('Mua vé cho: ' + (ev?.name || ev?.id));
  }

}
