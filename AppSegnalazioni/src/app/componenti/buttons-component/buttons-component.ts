import { Component } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { RouterLink } from '@angular/router';
import { DialogComponent } from '../dialog-component/dialog-component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-buttons-component',
  imports: [MatIcon, RouterLink],
  templateUrl: './buttons-component.html',
  styleUrl: './buttons-component.scss',
})
export class ButtonsComponent {
  constructor(private dialog: MatDialog) {}

  openDialog() {
    this.dialog.open(DialogComponent, {
      width: '400px',
      disableClose: true, // opzionale: impedisce la chiusura cliccando fuori
    });
  }
}
