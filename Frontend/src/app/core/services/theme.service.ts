import { DOCUMENT } from '@angular/common';
import { Inject, Injectable, signal } from '@angular/core';

export type ThemeMode = 'light' | 'dark';

const themeKey = 'ecommerce_theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly modeSignal = signal<ThemeMode>(this.loadInitialMode());

  readonly mode = this.modeSignal.asReadonly();

  constructor(@Inject(DOCUMENT) private readonly document: Document) {
    this.applyTheme(this.modeSignal());
  }

  setMode(mode: ThemeMode): void {
    localStorage.setItem(themeKey, mode);
    this.modeSignal.set(mode);
    this.applyTheme(mode);
  }

  toggle(): void {
    this.setMode(this.modeSignal() === 'light' ? 'dark' : 'light');
  }

  private loadInitialMode(): ThemeMode {
    const savedMode = localStorage.getItem(themeKey);

    if (savedMode === 'light' || savedMode === 'dark') {
      return savedMode;
    }

    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }

  private applyTheme(mode: ThemeMode): void {
    this.document.documentElement.dataset['theme'] = mode;
  }
}
