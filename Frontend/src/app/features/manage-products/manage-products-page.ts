import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Product } from '../../core/models/product';
import { ProductService } from '../../core/services/product.service';
import { EmptyState } from '../../shared/components/empty-state/empty-state';

type ProductForm = Omit<Product, 'id'> & { id?: string };

const emptyForm: ProductForm = {
  name: '',
  description: '',
  price: 0,
  stockQuantity: 0
};

@Component({
  selector: 'app-manage-products-page',
  imports: [CommonModule, CurrencyPipe, EmptyState, FormsModule],
  templateUrl: './manage-products-page.html',
  styleUrl: './manage-products-page.scss'
})
export class ManageProductsPage implements OnInit {
  protected readonly products = signal<Product[]>([]);
  protected readonly form = signal<ProductForm>({ ...emptyForm });
  protected readonly loading = signal(true);
  protected readonly error = signal('');
  protected readonly successMessage = signal('');

  constructor(private readonly productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  protected edit(product: Product): void {
    this.form.set({ ...product });
    this.successMessage.set('');
    this.error.set('');
  }

  protected cancelEdit(): void {
    this.form.set({ ...emptyForm });
  }

  protected save(): void {
    this.error.set('');
    this.successMessage.set('');

    const current = this.form();
    const request = current.id
      ? this.productService.updateProduct(current as Product)
      : this.productService.createProduct({
          name: current.name,
          description: current.description,
          price: Number(current.price),
          stockQuantity: Number(current.stockQuantity)
        });

    request.subscribe({
      next: () => {
        this.successMessage.set(current.id ? 'Product updated.' : 'Product created.');
        this.form.set({ ...emptyForm });
        this.loadProducts();
      },
      error: (response) => this.error.set(response.error || 'Unable to save product.')
    });
  }

  protected delete(product: Product): void {
    this.error.set('');
    this.successMessage.set('');

    this.productService.deleteProduct(product.id).subscribe({
      next: () => {
        this.successMessage.set('Product deleted.');
        this.loadProducts();
      },
      error: (response) => this.error.set(response.error || 'Unable to delete product.')
    });
  }

  protected updateField(field: keyof ProductForm, value: string): void {
    this.form.update((form) => ({
      ...form,
      [field]: field === 'price' || field === 'stockQuantity' ? Number(value) : value
    }));
  }

  private loadProducts(): void {
    this.loading.set(true);

    this.productService.getProductsForManagement().subscribe({
      next: (products) => {
        this.products.set(products);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Unable to load products. Login again if your session expired.');
        this.loading.set(false);
      }
    });
  }
}
