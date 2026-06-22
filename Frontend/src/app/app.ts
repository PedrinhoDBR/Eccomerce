import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { CartService } from './core/services/cart.service';
import { ThemeMode, ThemeService } from './core/services/theme.service';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  constructor(
    protected readonly authService: AuthService,
    protected readonly cartService: CartService,
    protected readonly themeService: ThemeService
  ) {}

  protected logout(): void {
    this.authService.logout();
  }

  protected setTheme(mode: ThemeMode): void {
    this.themeService.setMode(mode);
  }
}
