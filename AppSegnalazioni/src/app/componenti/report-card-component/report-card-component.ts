import { Component, input, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { MatIcon } from "@angular/material/icon";
import { AppReport } from '../../models/app-report';
import { DataService } from '../../services/dataservice/dataservice';

@Component({
  selector: 'app-report-card',
  imports: [MatCardModule, MatButtonModule, RouterLink, MatIcon],
  templateUrl: './report-card-component.html',
  styleUrl: './report-card-component.scss'
})
export class ReportCardComponent {
  private dataServ = inject(DataService);
  
  public userReport = input<AppReport>();

 get reportImage(): string {
  const images = this.userReport()?.images;
  if (!images || images.length === 0) {
    return './assets/no-image.jpg';
  }
  const firstImage = images[0];
  // Usa path se disponibile, altrimenti base64 (retrocompatibilità)
  if (firstImage.path) {
    return `https://localhost:7189${firstImage.path}`;
  }
  return firstImage.base64 || './assets/no-image.jpg';
}

}
