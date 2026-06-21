import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { OrderService } from '../../core/services/order.service';
import { EmptyState } from '../../shared/components/empty-state/empty-state';

@Component({
  selector: 'app-cart-page',
  imports: [CommonModule, CurrencyPipe, EmptyState, FormsModule, RouterLink],
  templateUrl: './cart-page.html',
  styleUrl: './cart-page.scss'
})
export class CartPage {
  private readonly cartService = inject(CartService);
  private readonly orderService = inject(OrderService);

  protected readonly customerName = signal('');
  protected readonly customerEmail = signal('');
  protected readonly address = signal('');
  protected readonly phone = signal('');
  protected readonly error = signal('');
  protected readonly successMessage = signal('');
  protected readonly cartItems = this.cartService.items;
  protected readonly cartTotal = computed(() => this.cartService.total());

  protected updateQuantity(productId: string, event: Event): void {
    const input = event.target as HTMLInputElement;
    this.cartService.updateQuantity(productId, Number(input.value));
  }

  protected removeFromCart(productId: string): void {
    this.cartService.removeProduct(productId);
  }

  protected checkout(): void {
    this.error.set('');
    this.successMessage.set('');

    const request = {
      customerName: this.customerName(),
      customerEmail: this.customerEmail(),
      address: this.address(),
      phone: this.phone(),
      items: this.cartItems().map((item) => ({
        productId: item.product.id,
        quantity: item.quantity
      }))
    };

    this.orderService.checkout(request).subscribe({
      next: (order) => {
        this.successMessage.set(`Order ${order.id} was created successfully.`);
        this.customerName.set('');
        this.customerEmail.set('');
        this.address.set('');
        this.phone.set('');
        this.cartService.clear();
      },
      error: (response) => {
        this.error.set(response.error || 'Unable to complete checkout.');
      }
    });
  }
}
