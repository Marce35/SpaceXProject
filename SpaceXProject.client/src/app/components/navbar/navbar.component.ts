import { Component, OnInit, inject, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../services/auth.service';
import { DbService } from '../../services/db.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule, RouterLink, RouterLinkActive,
    MatToolbarModule, MatButtonModule, MatIconModule,
    MatMenuModule, MatDividerModule
  ],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavBarComponent implements OnInit {
  authService = inject(AuthService);
  dbService = inject(DbService);
  private renderer = inject(Renderer2);

  isDarkTheme = false;

  async ngOnInit() {
    // Check IndexDB for saved theme preference on load
    const theme = await this.dbService.getSetting<string>('theme');
    if (theme === 'dark') {
      this.isDarkTheme = true;
      this.applyTheme(true);
    }
  }

  async toggleTheme() {
    this.isDarkTheme = !this.isDarkTheme;
    this.applyTheme(this.isDarkTheme);
    await this.dbService.setSetting('theme', this.isDarkTheme ? 'dark' : 'light');
  }

  private applyTheme(isDark: boolean) {
    if (isDark) {
      this.renderer.addClass(document.body, 'dark-theme');
    } else {
      this.renderer.removeClass(document.body, 'dark-theme');
    }
  }

  async logout() {
    await this.authService.logout();
  }
}
