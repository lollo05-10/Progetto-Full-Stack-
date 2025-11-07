import { Component } from '@angular/core';
import * as L from 'leaflet';
import { GeoJsonObject } from 'geojson';
import { MatIconModule } from "@angular/material/icon";
import { ButtonsComponent } from "../buttons-component/buttons-component";
import { DataService } from '../../services/dataservice/dataservice';

@Component({
  selector: 'app-map-component',
  imports: [MatIconModule, ButtonsComponent],
  templateUrl: './map-component.html',
  styleUrl: './map-component.scss'
})
export class MapComponent {
  private map: L.Map | undefined;

  constructor(private dataServ: DataService) {}
  
  ngAfterViewInit() {
    this.setupMap();
  }
  
async setupMap() {
    this.map = L.map('map');

    this.map.setView([44.40614435613236, 8.949400422559357], 13);

    const tileLayer = L.tileLayer(
      'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
      {
        maxZoom: 19,
        attribution:
          '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      }
    );

    tileLayer.addTo(this.map);

    const reports = await this.dataServ.getReportsGeoJson();

    const geojsonLayer = L.geoJSON(reports as GeoJsonObject, {
      pointToLayer: this.myPointToLayer,
      onEachFeature: this.myOnEachFeature,
    });

    geojsonLayer.addTo(this.map);
  }

  myPointToLayer(point: any, latLng: L.LatLng) {

    const colorCategory = {
      Selvatici: 'red',
      Avvistamenti: 'orange'

    }

    //const categoryKey = String(point?.properties?.categories.?[0] ?? '');
    //const fillColor = colorCategory[categoryKey as keyof typeof colorCategory] || 'blue';


    const geojsonMarkerOptions = {
      radius: 8,
      fillColor: 'blue',
      color: '#000',
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8,
    };
    return L.circleMarker(latLng, geojsonMarkerOptions);
  }

  myOnEachFeature(point: any, layer: L.Layer) {

    if (point.properties && point.properties.title) {
      console.log('point properties:', point.properties);
      const content = createPopupContent(point.properties);
      layer.bindPopup(content);
    }
  }
}

function createPopupContent(properties: any): string {
  const container = document.createElement('div');
  container.style.display = 'flex';
  container.className = 'popup-content';

  
  if (properties.images && properties.images.length > 0) {
    const image = document.createElement('img');
    image.src = properties.images[0];
    image.width = 50;
    image.height = 50;
    image.style.objectFit = 'cover';
    container.appendChild(image);
  }

  const titleDiv = document.createElement('div');
  titleDiv.className = 'popup-title';
  titleDiv.textContent = properties.title;
  container.appendChild(titleDiv);

  return container.outerHTML;
}
