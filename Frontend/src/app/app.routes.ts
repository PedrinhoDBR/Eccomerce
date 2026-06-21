import { Routes } from '@angular/router';
import { OrdersPage } from './features/orders/orders-page';
import { ShopPage } from './features/shop/shop-page';

export const routes: Routes = [
  { path: '', component: ShopPage },
  { path: 'orders', component: OrdersPage },
  { path: '**', redirectTo: '' }
];
