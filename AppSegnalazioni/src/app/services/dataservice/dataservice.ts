import { Injectable } from '@angular/core';
import { FeatureCollection } from 'geojson';
@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor() {}

  getReportsGeoJson(): Promise<FeatureCollection>{
    return fetch('./assets/reports.geojson')
    .then(resp => resp.json())
    .catch(err => console.error(err))
  }

  getReports(): Promise<Report[]> {
    return fetch('./assets/reports.json')
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  getReport(id: Number): Promise<Report> {
    return fetch('./assets/report.json')
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  getCategories(): Promise<string[]> {
    return fetch('./assets/categories.json')
      .then((resp) => resp.json())
      .catch((err) => console.error(err));
  }

  postReport(report: Report){
    console.log(report);
  }
}
