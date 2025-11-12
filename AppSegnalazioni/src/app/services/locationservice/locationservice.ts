import { Injectable } from '@angular/core';
import L from 'leaflet';
import { AppReport } from '../../models/app-report';
@Injectable({
  providedIn: 'root'
})
export class LocationService {
    sortReportsByDistance(reports: AppReport[]) : Promise<AppReport[]> {
  
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

  calculateReportsDistance(reports: AppReport[], position: GeolocationPosition) : AppReport[] {
  
    const userLatLng = L.latLng(position.coords.latitude, position.coords.longitude);

    const reportsWithDistance: AppReport[] = [];
    for (const report of reports) {
      report.distance = userLatLng.distanceTo(L.latLng(report.latitude,report.longitude));
      reportsWithDistance.push(report);
    }
    return reportsWithDistance;
  }

  sortByDistance(reports: AppReport[]) : AppReport[] {

    return reports.sort((r1,r2)=>{
      if(r1.distance!>r2.distance!) return 1;
      else if(r1.distance!<r2.distance!) return -1;
      else{
        return r1.title.localeCompare(r2.title);
      }
    })
  }
}
