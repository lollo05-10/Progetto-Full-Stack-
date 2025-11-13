import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { MatIcon } from "@angular/material/icon";
import { AppReport } from '../../models/app-report';

@Component({
  selector: 'app-report-card',
  imports: [MatCardModule, MatButtonModule, RouterLink, MatIcon],
  templateUrl: './report-card-component.html',
  styleUrl: './report-card-component.scss'
})
export class ReportCardComponent {

 public userReport= input<AppReport>();

 get reportImage(): string {
  const images = this.userReport()?.images;
  return images && images.length > 0 ? images[0].base64 : './assets/no-image.jpg';
}

}
