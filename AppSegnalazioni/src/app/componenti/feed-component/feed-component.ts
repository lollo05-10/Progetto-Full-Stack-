import { Component } from '@angular/core';
import { MatIconModule } from "@angular/material/icon";
import { ButtonsComponent } from "../buttons-component/buttons-component";
import { ReportCardComponent } from "../report-card-component/report-card-component";
import { MatCardModule } from "@angular/material/card";
import { AppReport, DataService } from '../../services/dataservice/dataservice';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-feed-component',
  imports: [MatIconModule, ButtonsComponent, ReportCardComponent, MatCardModule],
  templateUrl: './feed-component.html',
  styleUrl: './feed-component.scss'
})
export class FeedComponent {
  reportsFiltrati$!: Observable<AppReport[]>;
  // il ! serve per dire a typescript che do il valore dopo
  
  constructor(private dataServ: DataService) {
    this.reportsFiltrati$ = this.dataServ.reportsFiltrati$;
  }
}
