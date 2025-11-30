import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';

// Material Imports
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import {DbService} from "../../../services/db.service";
import {LaunchService} from "../../../services/launch.service";
import {SpaceXLaunchRow} from "../../../data/models/launch-row";
import {LaunchQueryRequest} from "../../../data/requests/launch-query";
import {SpaceXLaunch} from "../../../data/models/spacex-launch";

// Custom

@Component({
  selector: 'app-launches',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, MatTableModule, MatPaginatorModule,
    MatSortModule, MatInputModule, MatSelectModule, MatCardModule,
    MatButtonModule, MatIconModule, MatMenuModule, MatProgressSpinnerModule,
    MatDialogModule
  ],
  templateUrl: './launches.component.html',
  styleUrls: ['./launches.component.scss']
})
export class LaunchesComponent implements OnInit {
  private launchService = inject(LaunchService);
  private dbService = inject(DbService);
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);

  // Use the Row Model for the DataSource
  displayedColumns: string[] = ['name', 'date', 'rocket', 'success', 'actions'];
  dataSource = new MatTableDataSource<SpaceXLaunchRow>([]);
  totalDocs = 0;
  isLoading = false;
  filterForm: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor() {
    this.filterForm = this.fb.group({
      search: [''],
      type: ['all'],
      sort: ['desc']
    });
  }

  async ngOnInit() {
    // 1. Restore filters from IndexDB BEFORE loading data
    const savedFilters = await this.dbService.getSetting<any>('launch_filters');

    if (savedFilters) {
      // emitEvent: false prevents triggering the valueChanges subscription immediately
      this.filterForm.patchValue(savedFilters, { emitEvent: false });
    }

    // 2. Load Data (uses the restored filters)
    this.loadLaunches();

    // 3. Setup persistence listener
    this.filterForm.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged((prev, curr) => JSON.stringify(prev) === JSON.stringify(curr))
    ).subscribe(async (vals) => {
      // Save to DB whenever filters change
      await this.dbService.setSetting('launch_filters', vals);

      if (this.paginator) this.paginator.pageIndex = 0;
      this.loadLaunches();
    });
  }

  async loadLaunches() {
    this.isLoading = true;

    debugger;
    const request: LaunchQueryRequest = {
      page: this.paginator ? this.paginator.pageIndex + 1 : 1,
      limit: this.paginator ? this.paginator.pageSize : 10,
      search: this.filterForm.value.search || '',
      type: this.filterForm.value.type,
      sort: this.filterForm.value.sort
    };

    const res = await this.launchService.getLaunches(request);

    this.isLoading = false;

    if (res.isSuccess && res.value) {
      this.totalDocs = res.value.totalDocs;

      // MAP RESPONSE TO ROWS
      const rows = res.value.docs.map(item => this.mapToRow(item));
      this.dataSource.data = rows;

    } else {
      console.error('Failed to load launches', res.error);
    }
  }

  // --- MAPPING LOGIC ---
  private mapToRow(item: SpaceXLaunch): SpaceXLaunchRow {
    // 1. Format Date
    let formattedDateString = 'TBD';
    if (item.date_utc) {
      const dateObj = new Date(item.date_utc);
      formattedDateString = dateObj.toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit"
      });
    }

    // 2. Return UI Model
    return {
      id: item.id,
      name: item.name,
      rawDate: item.date_utc,
      formattedDate: formattedDateString, // <--- Used in HTML
      success: item.success,
      upcoming: item.upcoming,
      rocketName: item.rocket?.name || 'Unknown',
      rocketDetails: item.rocket // Store full object for dialog
    };
  }

  onPageChange(event: PageEvent) {
    this.loadLaunches();
  }

  openRocketDetails(row: SpaceXLaunchRow) {
    // if(!row.rocketDetails) return;
    //
    // this.dialog.open(RocketDetailsDialogComponent, {
    //   width: '500px',
    //   data: row.rocketDetails
    // });
  }
}
