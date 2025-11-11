import { Injectable } from '@angular/core';
import { FeatureCollection } from 'geojson';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { Report } from '../../models/report';


export interface AppReport {
  id: number;
  description: string;
  title: string;
  date: string;
  categories: string[];
  images: string[];
  latitude: number;
  longitude: number;
  distance?: number;
}

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private http: HttpClient) {}

  private apiUrl = 'http://localhost:5077/api/reports'; 
  //cambiare il port con quello che vi appare

  getReportsGeoJson(): Promise<FeatureCollection>{
    return fetch('./assets/report.geojson')
    .then(resp => resp.json())
    .catch(err => console.error(err))
  }

  getReports(): Observable<AppReport[]> {
    return this.http.get<AppReport[]>(this.apiUrl);
  }

  getReport(id: number): Observable<AppReport> {
    return this.http.get<AppReport>(`${this.apiUrl}/${id}`);
  }

  getCategories(): Promise<string[]> {
    return fetch('./assets/categories.json')
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  /*
  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/categories`);
  }
    */

  postReport(report: AppReport): Observable<AppReport> {
    return this.http.post<AppReport>(this.apiUrl, report);
  }


 // data-service.ts
registerUser(userData: any): Promise<any> {
  return fetch('http://localhost:5077/api/user', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      username: userData.username,
      password: userData.password,
      gender: userData.gender,
      dob: userData.dob,
      isAdmin: false
    })
  })
  .then(resp => {
    if (!resp.ok) {
      throw new Error('Errore durante la registrazione');
    }
    return resp.json();
  })
  .catch(err => {
    console.error('Errore registerUser:', err);
    throw err;
  });
}

  // Simula l'ID dell'utente loggato (per ora fisso)

  getCurrentUserId(): Promise<number> {
  return Promise.resolve(1);
}

getUserById(userId: number): Promise<User> {
  return fetch(`http://localhost:5077/api/user/${userId}`)
    .then(resp => resp.json())
    .catch(err => console.error(err));
}

  

getUserReports(userId: number): Promise<Report[]> {
  return fetch(`/api/user/${userId}/reports`)
    .then(resp => resp.json())
    .then((reports: Report[]) => reports) 
    .catch(err => {
      console.error(err);
      return []; 
    });


}

}