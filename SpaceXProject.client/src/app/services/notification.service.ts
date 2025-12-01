import {inject, Injectable} from '@angular/core';
import {MatSnackBar, MatSnackBarConfig} from "@angular/material/snack-bar";
import {SNACK_BAR_ERROR_MESSAGE_DURATION} from "../data/constants/constants";

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private snackbar = inject(MatSnackBar)

  showError(message: string, duration: number = SNACK_BAR_ERROR_MESSAGE_DURATION) {
    const config: MatSnackBarConfig = {
      duration: duration,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: ['error-snackbar']
    }

    this.snackbar.open(message, 'Ok', config);
  }
}
