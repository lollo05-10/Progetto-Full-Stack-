import { Component } from '@angular/core';
import { MatIconModule } from "@angular/material/icon";
import { ButtonsComponent } from "../buttons-component/buttons-component";
import { ReportCardComponent } from "../report-card-component/report-card-component";
import { MatCardModule } from "@angular/material/card";

@Component({
  selector: 'app-feed-component',
  imports: [MatIconModule, ButtonsComponent, ReportCardComponent, MatCardModule],
  templateUrl: './feed-component.html',
  styleUrl: './feed-component.scss'
})
export class FeedComponent {

}
