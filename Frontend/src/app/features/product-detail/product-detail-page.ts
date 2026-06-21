import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product';
import { EmptyState } from '../../shared/components/empty-state/empty-state';

@Component({
  selector: 'app-product-detail-page',
  imports: [CommonModule, CurrencyPipe, EmptyState, RouterLink],
  templateUrl: './product-detail-page.html',
  styleUrl: './product-detail-page.scss'
})
export class ProductDetailPage implements OnInit {
  protected readonly product = signal<Product | null>(null);
  protected readonly loading = signal(true);
  protected readonly error = signal('');
  protected readonly successMessage = signal('');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly productService: ProductService,
    private readonly cartService: CartService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.error.set('Product was not found.');
      this.loading.set(false);
      return;
    }

    this.productService.getProduct(id).subscribe({
      next: (product) => {
        this.product.set(product);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Product was not found.');
        this.loading.set(false);
      }
    });
  }

  protected addToCart(product: Product): void {
    this.cartService.addProduct(product);
    this.successMessage.set(`${product.name} was added to your cart.`);
  }
}
