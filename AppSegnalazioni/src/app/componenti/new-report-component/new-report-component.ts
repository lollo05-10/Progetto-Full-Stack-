import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';
import { DataService } from '../../services/dataservice/dataservice';
import {
  MatAnchor,
  MatButton,
  MatFabButton,
  MatIconButton,
} from '@angular/material/button';
import {
  MatSelectModule,
  MatFormField,
  MatLabel,
  MatOption,
} from '@angular/material/select';
import { MatInputModule, MatInput } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
@Component({
  selector: 'app-new-report-component',
  imports: [
    ReactiveFormsModule,
    MatAnchor,
    MatFormField,
    MatLabel,
    MatInput,
    MatOption,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIcon,
    MatFabButton,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './new-report-component.html',
  styleUrl: './new-report-component.scss',
})
export class NewReportComponent {
  private fb = new FormBuilder();
  public dataServ = inject(DataService);
  public categoryNames: string[] = [];

  constructor() {
    this.dataServ.getCategories().then((categories) => {
      this.categoryNames = categories;
    });
  }

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
  });

  get categories() {
    return this.reportForm.get('categories') as FormArray;
  }

  addCategoryInput() {
    this.categories.push(this.fb.control(''));
  }

  removeCategoryInput(index: number) {
    console.log(index);

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
  postReport() {
    console.log(this.reportForm.valid);
    console.log(this.reportForm.value);
  }
}
