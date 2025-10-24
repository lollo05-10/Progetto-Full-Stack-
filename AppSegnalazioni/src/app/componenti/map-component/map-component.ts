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

}


