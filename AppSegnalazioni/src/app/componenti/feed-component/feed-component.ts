import { Component } from '@angular/core';
import { MatIconModule } from "@angular/material/icon";
import { ButtonsComponent } from "../buttons-component/buttons-component";

@Component({
  selector: 'app-feed-component',
  imports: [MatIconModule, ButtonsComponent],
  templateUrl: './feed-component.html',
  styleUrl: './feed-component.scss'
})
export class FeedComponent {

}
