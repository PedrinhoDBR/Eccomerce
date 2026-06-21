import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login-page',
  imports: [FormsModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.scss'
})
export class LoginPage {
  protected readonly username = signal('');
  protected readonly password = signal('');
  protected readonly error = signal('');

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  protected login(): void {
    this.error.set('');

    this.authService.login({ username: this.username(), password: this.password() }).subscribe({
      next: () => this.router.navigateByUrl('/manage/products'),
      error: (response) => this.error.set(response.error || 'Invalid username or password.')
    });
  }
}
