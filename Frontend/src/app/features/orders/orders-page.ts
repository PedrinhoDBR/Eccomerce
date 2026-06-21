import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EmptyState } from '../../shared/components/empty-state/empty-state';
import { Order } from '../../core/models/order';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-orders-page',
  imports: [CommonModule, CurrencyPipe, DatePipe, EmptyState, RouterLink],
  templateUrl: './orders-page.html',
  styleUrl: './orders-page.scss'
})
export class OrdersPage implements OnInit {
  protected readonly orders = signal<Order[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal('');

  constructor(private readonly orderService: OrderService) {}

  ngOnInit(): void {
    this.orderService.getOrders().subscribe({
      next: (orders) => {
        this.orders.set(orders);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Unable to load orders. Check whether the backend is running.');
        this.loading.set(false);
      }
    });
  }
}
