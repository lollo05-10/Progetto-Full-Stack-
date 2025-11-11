import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";
import { ButtonsComponent } from "../buttons-component/buttons-component";
import { ReportCardComponent } from "../report-card-component/report-card-component";
import { MatCardModule } from "@angular/material/card";
import { AppReport, DataService } from '../../services/dataservice/dataservice';
import { Observable } from 'rxjs';
import { LocationService } from '../../services/locationservice/locationservice';

@Component({
  selector: 'app-feed-component',
  imports: [MatIconModule, ButtonsComponent, ReportCardComponent, MatCardModule, AsyncPipe],
  templateUrl: './feed-component.html',
  styleUrl: './feed-component.scss'
})
export class FeedComponent {
  reportsFiltrati$!: Observable<AppReport[]>;
  // il ! serve per dire a typescript che do il valore dopo
  
  constructor(private dataServ: DataService) {
    this.reportsFiltrati$ = this.dataServ.reportsFiltrati$;
  }

  private locationServ = inject(LocationService);
  public reports: AppReport[] = [];


}
