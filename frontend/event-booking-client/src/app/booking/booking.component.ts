import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventService } from '../event.service';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {
  event: any;
  tickets: any[] = [];
  selectedTickets: { [key: number]: number } = {};
  totalAmount: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eventService: EventService
  ) {}

  ngOnInit() {
    const eventId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadEventAndTickets(eventId);
  }

  loadEventAndTickets(eventId: number) {
    this.eventService.getEventById(eventId).subscribe({
      next: (data: any) => {
        this.event = data;
        this.tickets = [
          { id: 1, type: 'VIP', price: 1500000, available: 50 },
          { id: 2, type: 'Standard', price: 800000, available: 200 },
          { id: 3, type: 'Early Bird', price: 500000, available: 100 }
        ];
      },
      error: (err) => console.error('Error loading event', err)
    });
  }

  updateQuantity(ticketId: number, quantity: number): void {
    if (quantity < 0) quantity = 0;
    const ticket = this.tickets.find(t => t.id === ticketId);
    const max = ticket ? ticket.available : 0;
    if (quantity > max) quantity = max;

    this.selectedTickets[ticketId] = quantity;
    this.calculateTotal();
  }

    onQuantityInput(ticketId: number, event: any): void {
    const value = Number((event.target as HTMLInputElement).value) || 0;
    this.updateQuantity(ticketId, value);
  }

  calculateTotal() {
    this.totalAmount = Object.keys(this.selectedTickets).reduce((sum, key) => {
      const ticketId = Number(key);
      const ticket = this.tickets.find(t => t.id === ticketId);
      return sum + (ticket ? ticket.price * (this.selectedTickets[ticketId] || 0) : 0);
    }, 0);
  }

  getTotalTickets(): number {
    return Object.values(this.selectedTickets).reduce((sum, qty) => sum + qty, 0);
  }

  goBack() {
    this.router.navigate(['/']);
  }

  proceedToPayment() {
    console.log('Proceeding to payment with:', this.selectedTickets);
    alert('Đang chuyển đến trang thanh toán...');
  }
}