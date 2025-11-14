import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';
import { AppReport, AppReportPost, ImageDTO } from '../../models/app-report';
import { FilterService } from '../filterservice/filter-service';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private APIPort = 5077; // Modificate la variabile con la vostra porta
  private apiUrl = `https://localhost:${this.APIPort}/api/Report `;
  private reports$ = new BehaviorSubject<AppReport[]>([]);
  reportsFiltrati$!: Observable<AppReport[]>;

  constructor(private http: HttpClient, private filterService: FilterService) {
    // Stream filtrato che unisce i report e il filtro attivo
    this.reportsFiltrati$ = combineLatest([
      this.reports$,
      this.filterService.categorieSelezionate$,
    ]).pipe(
      map(([reports, filtro]) => {
        if (!filtro || filtro.length === 0) return reports;
        return reports.filter((r) =>
          (r.categoryNames ?? []).some((c) => filtro.includes(c))
        );
      })
    );
  }

  /** Carica i report da GeoJSON e li converte in AppReport */
  getReportsFromGeoJson(): Promise<AppReport[]> {
    return fetch('./assets/report.geojson')
      .then((resp) => resp.json())
      .then((geojson: any) => {
        return geojson.features.map(
          (f: any, index: number) =>
            ({
              id: index + 1,
              title: f.properties.title ?? '',
              description: f.properties.description ?? '',
              userId: f.properties.userId ?? 0,
              reportDate: f.properties.date ?? new Date().toISOString(),
              categoryNames: f.properties.category
                ? [f.properties.category]
                : ['Others'],
              images: f.properties.image
                ? [{ base64: f.properties.image }]
                : ([] as ImageDTO[]),
              latitude: f.geometry?.coordinates?.[1] ?? 0,
              longitude: f.geometry?.coordinates?.[0] ?? 0,
            } as AppReport)
        );
      })
      .catch((err) => {
        console.error('Errore caricamento GeoJSON:', err);
        return [];
      });
  }

  /** Ottieni tutti i report dal backend */
  getReports(): Observable<AppReport[]> {
    return this.http.get<AppReport[]>(this.apiUrl);
  }

  /** Ottieni singolo report */
  getReport(id: number): Promise<AppReport> {
    return fetch(`${this.apiUrl}/${id}`)
      .then((resp) => resp.json())
      .catch((err) => {
        console.error('Errore getReport:', err);
        throw err;
      });
  }

  /** Ottieni categorie */
  getCategories(): Promise<string[]> {
    return fetch('./assets/categories.json')
      .then((resp) => resp.json())
      .catch((err) => {
        console.error('Errore getCategories:', err);
        return [];
      });
  }

  /** Salva un report sul backend */
  postReport(report: AppReportPost): Observable<AppReportPost> {
    console.log('Dati inviati al backend:', report);
    return this.http.post<AppReportPost>(this.apiUrl, report);
  }

  /** Registrazione utente */
  registerUser(userData: any): Promise<any> {
    return fetch(`https://localhost:${this.APIPort}/api/User`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        username: userData.username,
        gender: userData.gender,
        dob: userData.dob,
        isAdmin: false,
      }),
    })
      .then((resp) => {
        if (!resp.ok) throw new Error('Errore durante la registrazione');
        return resp.json();
      })
      .catch((err) => {
        console.error('Errore registerUser:', err);
        throw err;
      });
  }

  isUserLogged(): boolean {
    return localStorage.getItem('userId') !== null;
  }

  /** Ottieni utente per id */
  getUserById(userId: number): Promise<User> {
    return fetch(`http://localhost:${this.APIPort}/api/user/${userId}`)
      .then((resp) => resp.json())
      .catch((err) => {
        console.error('Errore getUserById:', err);
        throw err;
      });
  }

  /** Ottieni report di un utente */
  getUserReports(userId: number): Promise<AppReport[]> {
    return fetch(`http://localhost:${this.APIPort}/api/user/${userId}/reports`)
      .then((resp) => resp.json())
      .then((reports: AppReport[]) => reports)
      .catch((err) => {
        console.error('Errore getUserReports:', err);
        return [];
      });
  }

  /** Carica i report da GeoJSON e aggiorna il BehaviorSubject */
  async caricaReportsDaGeoJson() {
    const reports = await this.getReportsFromGeoJson();
    this.reports$.next(reports);
  }
}
