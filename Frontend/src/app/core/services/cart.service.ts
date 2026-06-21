import { Injectable, signal } from '@angular/core';
import { CartItem } from '../models/cart-item';
import { Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly itemsSignal = signal<CartItem[]>([]);

  readonly items = this.itemsSignal.asReadonly();

  addProduct(product: Product): void {
    this.itemsSignal.update((items) => {
      const existingItem = items.find((item) => item.product.id === product.id);

      if (!existingItem) {
        return [...items, { product, quantity: 1 }];
      }

      return items.map((item) =>
        item.product.id === product.id
          ? { ...item, quantity: Math.min(item.quantity + 1, product.stockQuantity) }
          : item
      );
    });
  }

  updateQuantity(productId: string, quantity: number): void {
    this.itemsSignal.update((items) =>
      items
        .map((item) =>
          item.product.id === productId
            ? { ...item, quantity: Math.max(1, Math.min(quantity, item.product.stockQuantity)) }
            : item
        )
        .filter((item) => item.quantity > 0)
    );
  }

  removeProduct(productId: string): void {
    this.itemsSignal.update((items) => items.filter((item) => item.product.id !== productId));
  }

  clear(): void {
    this.itemsSignal.set([]);
  }

  total(): number {
    return this.items().reduce((sum, item) => sum + item.product.price * item.quantity, 0);
  }
}
