import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EventService, EventItem } from '../event.service';

interface TicketType {
  id: string;
  name: string;
  price: number;
  available: number;
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {
  event?: EventItem;
  ticketTypes: TicketType[] = [
    { id: '1', name: 'Vé thường', price: 150000, available: 50 },
    { id: '2', name: 'Vé VIP', price: 300000, available: 20 },
    { id: '3', name: 'Vé hạng A', price: 500000, available: 10 }
  ];
  selectedTickets: { [key: string]: number } = {};
  totalAmount = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eventService: EventService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.eventService.getEvents().subscribe((list: EventItem[]) => {
        this.event = list.find(e => e.id === id);
      });
    }
  }

  updateQuantity(ticketId: number, quantity: number): void {
  if (quantity < 0) quantity = 0;
  if (quantity > this.getMaxAvailable(ticketId)) {
    quantity = this.getMaxAvailable(ticketId);
  }
  this.selectedTickets[ticketId] = quantity;
  this.calculateTotal();
}

  calculateTotal() {
    this.totalAmount = 0;
    for (const ticketId in this.selectedTickets) {
      const quantity = this.selectedTickets[ticketId];
      const ticket = this.ticketTypes.find(t => t.id === ticketId);
      if (ticket) {
        this.totalAmount += ticket.price * quantity;
      }
    }
  }

  getTotalTickets(): number {
    return Object.values(this.selectedTickets).reduce((sum, qty) => sum + qty, 0);
  }

  proceedToPayment() {
    if (this.getTotalTickets() === 0) {
      alert('Vui lòng chọn ít nhất một vé');
      return;
    }
    // Simulate booking
    alert(`Đặt vé thành công! Tổng tiền: ${this.totalAmount.toLocaleString()} ₫`);
    this.router.navigate(['/']);
  }

  goBack() {
    this.router.navigate(['/events', this.event?.id]);
  }
}