import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private readonly http: HttpClient) {}

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${environment.apiUrl}/products`);
  }

  getProductsForManagement(): Observable<Product[]> {
    return this.http.get<Product[]>(`${environment.apiUrl}/products/admin`);
  }

  getProduct(id: string): Observable<Product> {
    return this.http.get<Product>(`${environment.apiUrl}/products/${id}`);
  }

  createProduct(product: Omit<Product, 'id'>): Observable<Product> {
    return this.http.post<Product>(`${environment.apiUrl}/products`, product);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(`${environment.apiUrl}/products/${product.id}`, {
      name: product.name,
      description: product.description,
      price: product.price,
      stockQuantity: product.stockQuantity
    });
  }

  deleteProduct(productId: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/products/${productId}`);
  }
}
