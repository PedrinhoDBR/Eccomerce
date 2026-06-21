import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EmptyState } from '../../shared/components/empty-state/empty-state';
import { CartService } from '../../core/services/cart.service';
import { OrderService } from '../../core/services/order.service';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product';

@Component({
  selector: 'app-shop-page',
  imports: [CommonModule, CurrencyPipe, EmptyState, FormsModule, RouterLink],
  templateUrl: './shop-page.html',
  styleUrl: './shop-page.scss'
})
export class ShopPage implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly orderService = inject(OrderService);
  private readonly cartService = inject(CartService);

  protected readonly products = signal<Product[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal('');
  protected readonly customerName = signal('');
  protected readonly customerEmail = signal('');
  protected readonly successMessage = signal('');
  protected readonly cartItems = this.cartService.items;
  protected readonly cartTotal = computed(() => this.cartService.total());

  ngOnInit(): void {
    this.loadProducts();
  }

  protected addToCart(product: Product): void {
    this.successMessage.set('');
    this.cartService.addProduct(product);
  }

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
        this.cartService.clear();
        this.loadProducts();
      },
      error: (response) => {
        this.error.set(response.error || 'Unable to complete checkout.');
      }
    });
  }

  private loadProducts(): void {
    this.loading.set(true);

    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products.set(products);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Unable to load products. Check whether the backend is running.');
        this.loading.set(false);
      }
    });
  }
}
