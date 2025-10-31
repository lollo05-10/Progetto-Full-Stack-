import { Component } from '@angular/core';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-buttons-component',
  imports: [MatIcon],
  templateUrl: './buttons-component.html',
  styleUrl: './buttons-component.scss',
})
export class ButtonsComponent {
  openDialog() {
    // Logic to open a dialog goes here
  }
}
