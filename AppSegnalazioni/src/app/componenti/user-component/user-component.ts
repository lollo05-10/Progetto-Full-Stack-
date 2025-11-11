import { Component, inject } from '@angular/core';
import { MatCard, MatCardHeader, MatCardModule, MatCardContent } from "@angular/material/card";
import { ReportCardComponent } from "../report-card-component/report-card-component";
import { DataService } from '../../services/dataservice/dataservice';
import { User } from '../../models/user';
import { Report } from '../../models/report';

@Component({
  selector: 'app-user-component',
  imports: [
    MatCard, 
    ReportCardComponent, 
    MatCardHeader, 
    MatCardModule, 
    MatCardContent
  ],
  templateUrl: './user-component.html',
  styleUrl: './user-component.scss'
})
export class UserComponent {
  private dataServ = inject(DataService);
  public user: User | null = null;
  public userReports: Report[] = [];
  public isLoading = true;

  constructor(){
    this.loadUserData();
  }

  async loadUserData(){
    try{
      this.isLoading = true;
      this.user = await this.dataServ.getUserById(1); 
       this.userReports = await this.dataServ.getUserReports(1);
    }
    catch(error){
      console.error('Error caricamento dati utente', error);
    }
    finally{
      this.isLoading = false;
    }
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('it-IT');
  }
}