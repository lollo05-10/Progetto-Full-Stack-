import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { FilterService } from '../../services/filterservice/filter-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dialog-component',
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  templateUrl: './dialog-component.html',
  styleUrl: './dialog-component.scss',
})

export class DialogComponent {
  selectedCategories: string[] = [];

  categories = [
    'Maltrattamento',
    'Avvistamento di animale selvatico',
    'Smarrimento',
    'Ritrovamento',
    'Nido e/o cucciolata avvistato',
    'Others'
  ];

  constructor(private filterService: FilterService) {}

  toggleCategory(cat: string) {
    if (this.selectedCategories.includes(cat)) {
      this.selectedCategories = this.selectedCategories.filter(c => c !== cat);
    } else {
      this.selectedCategories.push(cat);
    }
  }

  applyFilter() {
    this.filterService.setCategorie(this.selectedCategories);
  }

}
