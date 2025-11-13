import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ButtonsComponent } from '../buttons-component/buttons-component';
import { ReportCardComponent } from '../report-card-component/report-card-component';
import { MatCardModule } from '@angular/material/card';
import { DataService } from '../../services/dataservice/dataservice';
import { Observable } from 'rxjs';
import { LocationService } from '../../services/locationservice/locationservice';
import { AppReport } from '../../models/app-report';

@Component({
  selector: 'app-feed-component',
  imports: [
    CommonModule,
    MatIconModule,
    ButtonsComponent,
    ReportCardComponent,
    MatCardModule,
  ],
  templateUrl: './feed-component.html',
  styleUrl: './feed-component.scss',
})
export class FeedComponent implements OnInit {
  reportsFiltrati$!: Observable<AppReport[]>;

  private locationServ = inject(LocationService);

  constructor(private dataServ: DataService) {
    this.reportsFiltrati$ = this.dataServ.reportsFiltrati$;
  }

  async ngOnInit() {
    // carica i report dal file GeoJSON
    await this.dataServ.caricaReportsDaGeoJson();
  }

  trackById(index: number, report: AppReport): number {
    return report.id;
  }
}
