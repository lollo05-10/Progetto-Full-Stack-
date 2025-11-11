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

  private apiUrl = 'http://localhost:5077/api/reports'; 
  //cambiare il port con quello che vi appare

  private reports$ = new BehaviorSubject<AppReport[]>([]);

  reportsFiltrati$!: Observable<AppReport[]>;

  // Stream filtrato che unisce i report e il filtro attivo
  constructor(private http: HttpClient, private filterService: FilterService) {

    this.reportsFiltrati$ = combineLatest([
      this.reports$,
      this.filterService.categorieSelezionate$
    ]).pipe(
      map(([reports, filtro]) => {
        if (!filtro || filtro.length === 0) return reports;
        return reports.filter(r => r.categories.some(c => filtro.includes(c)));
      })
    );
  }

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
