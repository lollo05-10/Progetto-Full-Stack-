import { Component } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-map-component',
  imports: [],
  templateUrl: './map-component.html',
  styleUrl: './map-component.scss'
})
export class MapComponent {
  private map: L.Map | undefined;
  
  ngAfterViewinit() {
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

