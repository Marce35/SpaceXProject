import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {ApiHealthCheckService} from "../../services/api-health-check.service";

@Component({
  selector: 'app-out-of-service',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './out-of-service.component.html',
  styleUrls: ['./out-of-service.component.scss']
})
export class OutOfServiceComponent implements OnInit, OnDestroy {
  private apiHealthCheckService = inject(ApiHealthCheckService);
  private router = inject(Router);

  private intervalId: any;
  isRetrying = false;

  ngOnInit() {
    // Start polling every 5 seconds
    this.intervalId = setInterval(() => this.checkSystemStatus(), 5000);
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  async checkSystemStatus() {
    this.isRetrying = true;
    const isAlive = await this.apiHealthCheckService.checkApiHealth();
    this.isRetrying = false;

    if (isAlive) {
      this.router.navigate(['/']);
    }
  }

  // Manual retry button
  manualRetry() {
    this.checkSystemStatus();
  }
}
