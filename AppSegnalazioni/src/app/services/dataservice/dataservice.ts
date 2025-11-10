import { Injectable } from '@angular/core';
import { FeatureCollection } from 'geojson';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
}
