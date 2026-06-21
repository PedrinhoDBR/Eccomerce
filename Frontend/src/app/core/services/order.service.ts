import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CheckoutRequest } from '../models/checkout';
import { Order } from '../models/order';

@Injectable({ providedIn: 'root' })
export class OrderService {
  constructor(private readonly http: HttpClient) {}

  checkout(request: CheckoutRequest): Observable<Order> {
    return this.http.post<Order>(`${environment.apiUrl}/cart/checkout`, request);
  }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${environment.apiUrl}/orders`);
  }
}
