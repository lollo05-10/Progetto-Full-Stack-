import { Injectable } from '@angular/core';
import L from 'leaflet';
import { Report } from '../../models/report';


declare global {
  interface Report {
    distance?: number;
  }
}

@Injectable({
  providedIn: 'root'
})
export class LocationService {
    sortReportsByDistance(reports: Report[]) : Promise<Report[]> {
  
    return this.getPosition()
    .then((pos) => this.calculateReportsDistance(reports, pos))
    .then((reports) => this.sortByDistance(reports))
    .catch((err) => {console.error(err); return reports});
  }

  getPosition(): Promise<GeolocationPosition> {

    return new Promise<GeolocationPosition>((Resolve, Reject) => {
      return navigator.geolocation.getCurrentPosition(Resolve, Reject);
    })
  }

  calculateReportsDistance(reports: Report[], position: GeolocationPosition) : Report[] {
  
    const userLatLng = L.latLng(position.coords.latitude, position.coords.longitude);

    const reportsWithDistance: Report[] = [];
    for (const report of reports) {
      report.distance = userLatLng.distanceTo(L.latLng(report.lat,report.lng));
      reportsWithDistance.push(report);
    }
    return reportsWithDistance;
  }

  sortByDistance(reports: Report[]) : Report[] {

    return reports.sort((r1,r2)=>{
      if(r1.distance!>r2.distance!) return 1;
      else if(r1.distance!<r2.distance!) return -1;
      else{
        return r1.title.localeCompare(r2.title);
      }
    })
  }
}
