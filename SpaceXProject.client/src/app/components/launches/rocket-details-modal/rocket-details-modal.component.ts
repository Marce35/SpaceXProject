import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import {SpaceXRocket} from "../../../data/models/spacex-rocket";

@Component({
  selector: 'app-rocket-details',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  templateUrl: './rocket-details-modal.component.html',
  styleUrls: ['./rocket-details-modal.component.scss']
})
export class RocketDetailsModal{
  constructor(
    public dialogRef: MatDialogRef<RocketDetailsModal>,
    @Inject(MAT_DIALOG_DATA) public data: SpaceXRocket
  ) {}
}
