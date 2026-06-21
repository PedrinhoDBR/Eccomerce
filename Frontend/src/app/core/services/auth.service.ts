import { HttpClient } from '@angular/common/http';
import { Injectable, computed, signal } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../models/login';

const tokenKey = 'ecommerce_admin_token';
const userKey = 'ecommerce_admin_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenSignal = signal(localStorage.getItem(tokenKey) ?? '');
  private readonly userSignal = signal(localStorage.getItem(userKey) ?? '');

  readonly token = this.tokenSignal.asReadonly();
  readonly username = this.userSignal.asReadonly();
  readonly isLoggedIn = computed(() => Boolean(this.tokenSignal()));

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest) {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap((response) => {
        localStorage.setItem(tokenKey, response.token);
        localStorage.setItem(userKey, response.username);
        this.tokenSignal.set(response.token);
        this.userSignal.set(response.username);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(tokenKey);
    localStorage.removeItem(userKey);
    this.tokenSignal.set('');
    this.userSignal.set('');
  }
}
