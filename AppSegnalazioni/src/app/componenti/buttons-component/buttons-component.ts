import { Component } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { DialogComponent } from '../dialog-component/dialog-component';
import { MatDialog } from '@angular/material/dialog';
import { DataService } from '../../services/dataservice/dataservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-buttons-component',
  imports: [MatIcon],
  templateUrl: './buttons-component.html',
  styleUrl: './buttons-component.scss',
})
export class ButtonsComponent {
  constructor(private dialog: MatDialog, private dataservice: DataService, private router: Router) {}

goToNewReport() {
  if (this.dataservice.isUserLogged()) {
    this.router.navigate(['/new-report']);
  } else {
    alert('Per creare un report devi avere un account. Verrai reindirizzato al login.');
    this.router.navigate(['/login']);
  }
}

  openDialog() {
    this.dialog.open(DialogComponent, {
      width: '400px',
      disableClose: false, // opzionale: impedisce la chiusura cliccando fuori
    });
  }
}
