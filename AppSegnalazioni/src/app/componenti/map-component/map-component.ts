import { Component } from '@angular/core';
import * as L from 'leaflet';
import { GeoJsonObject } from 'geojson';
import { MatIconModule } from "@angular/material/icon";
import { ButtonsComponent } from "../buttons-component/buttons-component";

@Component({
  selector: 'app-map-component',
  imports: [MatIconModule, ButtonsComponent],
  templateUrl: './map-component.html',
  styleUrl: './map-component.scss'
})
export class MapComponent {
  private map: L.Map | undefined;
  
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
  }
}

