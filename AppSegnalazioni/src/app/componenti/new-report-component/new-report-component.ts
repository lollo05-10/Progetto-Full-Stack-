import {
  ChangeDetectionStrategy,
  Component,
  inject,
  AfterViewInit,
} from '@angular/core';
import {
  FormBuilder,
  FormArray,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  ReactiveFormsModule,
} from '@angular/forms';
import { DataService } from '../../services/dataservice/dataservice';
import * as L from 'leaflet';
import { AppReport, AppReportPost } from '../../models/app-report';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-new-report-component',
  templateUrl: './new-report-component.html',
  styleUrls: ['./new-report-component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
  ],
})
export class NewReportComponent implements AfterViewInit {
  private fb = new FormBuilder();
  private dataServ = inject(DataService);

  public categoryNames: string[] = [];
  public images: string[] = []; // Per preview (base64)
  public imageFiles: File[] = []; // File da caricare
  public uploadedImagePaths: string[] = []; // Percorsi caricati sul server

  private map!: L.Map;
  private markersLayer!: L.LayerGroup;

  constructor() {
    this.dataServ.getCategories().then((categories) => {
      this.categoryNames = categories;
    });
  }

  ngAfterViewInit(): void {
    this.setupMap();
  }

  private setupMap() {
    this.map = L.map('map').setView([44.406144, 8.9494], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '&copy; OpenStreetMap contributors',
    }).addTo(this.map);

    this.markersLayer = L.layerGroup().addTo(this.map);

    // Click sulla mappa per aggiungere un nuovo report
    this.map.on('click', (e: L.LeafletMouseEvent) => {
      const { lat, lng } = e.latlng;

      // Aggiorna il form
      this.reportForm.patchValue({ latitude: lat, longitude: lng });

      // Aggiorna marker dinamico
      this.addOrUpdateMarker();
    });
  }

  // FORM REATTIVO
  public reportForm = this.fb.group({
    title: [
      '',
      [Validators.required, Validators.minLength(5), Validators.maxLength(20)],
    ],
    description: [
      '',
      [Validators.required, Validators.minLength(5), Validators.maxLength(50)],
    ],
    categories: this.fb.array([this.fb.control('')]),
    date: ['', [Validators.required, this.DateValidator()]],
    latitude: [0, Validators.required],
    longitude: [0, Validators.required],
  });

  get categories(): FormArray {
    return this.reportForm.get('categories') as FormArray;
  }

  addCategoryInput() {
    this.categories.push(this.fb.control(''));
  }

  removeCategoryInput(index: number) {
    this.categories.removeAt(index);
  }

  DateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      const today = Date.now();
      const inputDate = new Date(control.value).getTime();
      return inputDate > today ? { futureDate: true } : null;
    };
  }

  onImageSelected(event: Event) {
    const element = event.target as HTMLInputElement;
    if (element.files && element.files.length > 0) {
      const file = element.files[0];
      this.imageFiles.push(file);
      
      // Leggi come base64 per preview
      const reader = new FileReader();
      reader.onload = () => {
        this.images.push(reader.result as string);
        this.addOrUpdateMarker(); // Aggiorna marker se immagine selezionata
      };
      reader.readAsDataURL(file);
    }
  }

  postReport() {
    if (!this.reportForm.valid) return;

    const formValue = this.reportForm.value;

    // Prima carica tutte le immagini
    if (this.imageFiles.length > 0) {
      const uploadObservables = this.imageFiles.map((file) =>
        this.dataServ.uploadImage(file)
      );

      forkJoin(uploadObservables).subscribe({
        next: (results) => {
          // Estrai i percorsi dalle risposte
          this.uploadedImagePaths = results.map((r) => r.path);
          this.createReport(formValue);
        },
        error: (err) => {
          console.error('Errore nel caricamento immagini:', err);
        },
      });
    } else {
      // Nessuna immagine da caricare, crea direttamente il report
      this.createReport(formValue);
    }
  }

  private createReport(formValue: any) {
    const report: AppReportPost = {
      userId: 1,
      reportDate: new Date().toISOString(),
      title: formValue.title ?? '',
      description: formValue.description ?? '',
      latitude: formValue.latitude ?? 0,
      longitude: formValue.longitude ?? 0,
      // filtriamo null e forziamo il tipo string[]
      categoryNames: (formValue.categories ?? []).filter(
        (c: any): c is string => !!c
      ),
      images: this.uploadedImagePaths.map((path) => ({ path })),
    };

    this.dataServ.postReport(report).subscribe({
      next: () => {
        console.log('Report salvato con successo');
        this.reportForm.reset();
        this.images = [];
        this.imageFiles = [];
        this.uploadedImagePaths = [];
        this.markersLayer.clearLayers();
      },
      error: (err) => console.error('Errore nel post report:', err),
    });
  }

  /** Aggiunge o aggiorna marker sulla mappa in stile MapComponent */
  private addOrUpdateMarker() {
    const { latitude, longitude } = this.reportForm.value;
    if (latitude && longitude) {
      this.markersLayer.clearLayers();

      const mainCategory = this.categories.controls[0]?.value || 'Others';
      const colorCategory: Record<string, string> = {
        Maltrattamento: 'red',
        'Avvistamento di animale selvatico': 'orange',
        Smarrimento: 'yellow',
        Ritrovamento: 'green',
        'Nido e/o cucciolata avvistato': 'blue',
        Others: 'purple',
      };
      const color = colorCategory[mainCategory] || 'gray';

      const marker = L.circleMarker([latitude, longitude], {
        radius: 8,
        fillColor: color,
        color: '#000',
        weight: 1,
        opacity: 1,
        fillOpacity: 0.8,
      });

      marker.addTo(this.markersLayer);
    }
  }

}
