import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EmptyState } from '../../shared/components/empty-state/empty-state';
import { CartService } from '../../core/services/cart.service';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product';

@Component({
  selector: 'app-shop-page',
  imports: [CommonModule, CurrencyPipe, EmptyState, RouterLink],
  templateUrl: './shop-page.html',
  styleUrl: './shop-page.scss'
})
export class ShopPage implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly cartService = inject(CartService);

  protected readonly products = signal<Product[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal('');
  protected readonly successMessage = signal('');

  ngOnInit(): void {
    this.loadProducts();
  }

  protected addToCart(product: Product): void {
    this.successMessage.set('');
    this.cartService.addProduct(product);
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
