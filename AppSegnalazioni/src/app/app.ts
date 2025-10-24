import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import 'leaflet/dist/leaflet.css';
import { TabsComponent } from './componenti/tabs-component/tabs-component';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TabsComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('AppSegnalazioni');
}
