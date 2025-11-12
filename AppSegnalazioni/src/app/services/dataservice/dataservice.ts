import { FeatureCollection } from 'geojson';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';
import { User } from '../../models/user';
import { Injectable } from '@angular/core';
import { FilterService } from '../filterservice/filter-service';
import { AppReport } from '../../models/app-report';

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
      this.filterService.categorieSelezionate$,
    ]).pipe(
      map(([reports, filtro]) => {
        if (!filtro || filtro.length === 0) return reports;
        return reports.filter((r) =>
          r.categories.some((c) => filtro.includes(c))
        );
      })
    );
  }

getReportsFromGeoJson(): Promise<AppReport[]> {
  return fetch('./assets/report.geojson')
    .then(resp => resp.json())
    .then((geojson: any) => {
      return geojson.features.map((f: any, index: number) => ({
        id: index + 1, // puoi generare un id progressivo
        title: f.properties.title,
        description: f.properties.description,
        categories: [f.properties.category], // array per compatibilità filter
        images: f.properties.image ? [f.properties.image] : [],
        latitude: f.geometry.coordinates[1], // GeoJSON: [lng, lat]
        longitude: f.geometry.coordinates[0],
      }));
    })
    .catch(err => {
      console.error(err);
      return [];
    });
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
    return fetch('https://localhost:7189/api/User', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        username: userData.username,
        gender: userData.gender,
        dob: userData.dob,
        isAdmin: false,
      }),
    })
      .then((resp) => {
        if (!resp.ok) {
          throw new Error('Errore durante la registrazione');
        }
        return resp.json();
      })
      .catch((err) => {
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
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  getUserReports(userId: number): Promise<AppReport[]> {
    return fetch(`/api/user/${userId}/reports`)
      .then((resp) => resp.json())
      .then((reports: AppReport[]) => reports)
      .catch((err) => {
        console.error(err);
        return [];
      });
  }

  async caricaReportsDaGeoJson() {
    const reports = await this.getReportsFromGeoJson();
    this.reports$.next(reports); 
  }

}
