import { Routes } from '@angular/router';
import { adminGuard } from './core/guards/admin.guard';
import { CartPage } from './features/cart/cart-page';
import { LoginPage } from './features/login/login-page';
import { ManageProductsPage } from './features/manage-products/manage-products-page';
import { OrdersPage } from './features/orders/orders-page';
import { ProductDetailPage } from './features/product-detail/product-detail-page';
import { ShopPage } from './features/shop/shop-page';

export const routes: Routes = [
  { path: '', component: ShopPage },
  { path: 'products/:id', component: ProductDetailPage },
  { path: 'cart', component: CartPage },
  { path: 'login', component: LoginPage },
  { path: 'manage/products', component: ManageProductsPage, canActivate: [adminGuard] },
  { path: 'orders', component: OrdersPage, canActivate: [adminGuard] },
  { path: '**', redirectTo: '' }
];
