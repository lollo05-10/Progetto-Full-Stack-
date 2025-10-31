import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatTabsModule} from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-tabs-component',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatDividerModule, RouterLink],
  templateUrl: './tabs-component.html',
  styleUrl: './tabs-component.scss',
})
export class TabsComponent {

}
