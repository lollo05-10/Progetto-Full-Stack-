import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { MatIcon } from "@angular/material/icon";
import { AppReport } from '../../services/dataservice/dataservice';

@Component({
  selector: 'app-report-card',
  imports: [MatCardModule, MatButtonModule, RouterLink, MatIcon],
  templateUrl: './report-card-component.html',
  styleUrl: './report-card-component.scss'
})
export class ReportCardComponent {

 public report= input<AppReport>();
}
