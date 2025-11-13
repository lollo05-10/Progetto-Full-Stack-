import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../services/dataservice/dataservice';
import { AppReport } from '../../models/app-report';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-detail',
  imports: [CommonModule, MatCardModule],
  templateUrl: './detail-component.html',
  styleUrl: './detail-component.scss'
})
export class DetailComponent {
  private route = inject(ActivatedRoute);
  private dataServ = inject(DataService);
  
  public report: AppReport | null = null;
  public isLoading = true;

  constructor() {
    this.loadReport();
  }

  async loadReport() {
    try {
      const id = Number(this.route.snapshot.paramMap.get('id'));
      this.report = await this.dataServ.getReport(id);
    } catch (error) {
      console.error('Errore caricamento report:', error);
    } finally {
      this.isLoading = false;
    }
  }
}