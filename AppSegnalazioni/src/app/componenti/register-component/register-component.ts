import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import {
  MatCardContent,
  MatCard,
  MatCardHeader,
  MatCardTitle,
  MatCardSubtitle,
} from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/dataservice/dataservice';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCheckboxModule,
    RouterLink,
    MatCardContent,
    MatCard,
    MatCardHeader,
    MatCardTitle,
    MatCardSubtitle,
  ],
  templateUrl: './register-component.html',
  styleUrl: './register-component.scss',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private dataServ = inject(DataService);
  private router = inject(Router);

  public isLoading = false;

  registrationForm = this.fb.group({
    username: [
      '',
      [Validators.required, Validators.minLength(3), Validators.maxLength(10)],
    ],
    gender: ['', [Validators.required, Validators.maxLength(1)]],
    dob: ['', Validators.required],
    acceptTerms: [false, Validators.requiredTrue],
  });

  async onSubmit() {
    if (this.registrationForm.valid) {
      try {
        this.isLoading = true;
        const formData = this.registrationForm.value;

        // Chiama il servizio per salvare nel DB
        const result = await this.dataServ.registerUser({
          username: formData.username!,
          gender: formData.gender!,
          dob: formData.dob!,
        });

        // Salva l’utente nel localStorage
        localStorage.setItem('userId', result.id.toString());

        console.log('Registrazione completata:', result);

        // Reindirizza al login dopo il successo
        this.router.navigate(['/login']);
      } catch (error) {
        console.error('Errore durante la registrazione:', error);
        // Qui puoi mostrare un messaggio di errore all'utente
      } finally {
        this.isLoading = false;
      }
    }
  }
}
