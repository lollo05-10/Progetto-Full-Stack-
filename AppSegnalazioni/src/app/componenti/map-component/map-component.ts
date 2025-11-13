import { Component } from '@angular/core';
import * as L from 'leaflet';
import { MatIconModule } from '@angular/material/icon';
import { ButtonsComponent } from '../buttons-component/buttons-component';
import { DataService } from '../../services/dataservice/dataservice';
import { Observable } from 'rxjs';
import { AppReport } from '../../models/app-report';

@Component({
  selector: 'app-map-component',
  imports: [MatIconModule, ButtonsComponent],
  templateUrl: './map-component.html',
  styleUrl: './map-component.scss',
})
export class MapComponent {
  private map!: L.Map;
  private markersLayer!: L.LayerGroup;
  reportsFiltrati$!: Observable<AppReport[]>;

  constructor(private dataServ: DataService) {
    this.reportsFiltrati$ = this.dataServ.reportsFiltrati$;
  }

  ngAfterViewInit() {
    this.setupMap();
  }

  async setupMap() {
    this.map = L.map('map').setView([44.406144, 8.9494], 13);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '&copy; OpenStreetMap contributors',
    }).addTo(this.map);

    this.markersLayer = L.layerGroup().addTo(this.map);

    // Carica i dati GeoJSON nel DataService
    await this.dataServ.caricaReportsDaGeoJson();

    this.reportsFiltrati$.subscribe((reports) => {
      this.updateMarkers(reports);
    });
  }

  /** Aggiorna marker con colore in base alla categoria */
  private updateMarkers(reports: AppReport[]) {
    this.markersLayer.clearLayers();

    const colorCategory: Record<string, string> = {
      'Maltrattamento': 'red',
      'Avvistamento di animale selvatico': 'orange',
      'Smarrimento': 'yellow',
      'Ritrovamento': 'green',
      'Nido e/o cucciolata avvistato': 'blue',
      'Others': 'purple',
    };

    reports.forEach((report) => {
      if (report.latitude && report.longitude) {
        const mainCategory = report.categoryNames?.[0] ?? 'Others';
        const color = colorCategory[mainCategory] || 'gray';

        const marker = L.circleMarker([report.latitude, report.longitude], {
          radius: 8,
          fillColor: color,
          color: '#000',
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8,
        });

        marker.bindPopup(this.createPopupContent(report));
        marker.addTo(this.markersLayer);
      }
    });
  }

  private createPopupContent(report: AppReport): string {
    const container = document.createElement('div');
    container.style.display = 'flex';
    container.className = 'popup-content';

    if (report.images && report.images.length > 0) {
      const image = document.createElement('img');
      image.src = report.images[0].base64;
      image.width = 50;
      image.height = 50;
      image.style.objectFit = 'cover';
      image.style.marginRight = '8px';
      container.appendChild(image);
    }

    const titleDiv = document.createElement('div');
    titleDiv.className = 'popup-title';
    titleDiv.textContent = report.title;
    container.appendChild(titleDiv);

    const descriptionDiv = document.createElement('div');
    descriptionDiv.className = 'popup-description';
    descriptionDiv.textContent = report.description;
    titleDiv.appendChild(descriptionDiv);

    return container.outerHTML;
  }
}
