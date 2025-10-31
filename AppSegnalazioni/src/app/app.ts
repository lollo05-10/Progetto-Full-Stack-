import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import 'leaflet/dist/leaflet.css';
import { TabsComponent } from './componenti/tabs-component/tabs-component';
import { HeaderComponent } from "./componenti/header-component/header-component";


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TabsComponent, HeaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('AppSegnalazioni');
}
