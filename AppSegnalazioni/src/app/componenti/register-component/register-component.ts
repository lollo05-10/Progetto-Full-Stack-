import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardContent, MatCard, MatCardHeader, MatCardTitle, MatCardSubtitle } from '@angular/material/card';
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
    MatCardSubtitle
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
    gender: ['M', [Validators.required]], // Valore di default
    dob: ['', Validators.required],
    isAdmin: [false], // Aggiunto campo isAdmin
    acceptTerms: [false, Validators.requiredTrue],
  });

 async onSubmit() {
  if (this.registrationForm.valid) {
    try {
      this.isLoading = true;
      const formData = this.registrationForm.value;

      if (!formData.username || !formData.gender || !formData.dob) {
        throw new Error('Dati del form incompleti');
      }

      const userData = {
        username: formData.username,
        gender: formData.gender,
        dob: formData.dob,
        isAdmin: formData.isAdmin || false
      };

      console.log('Invio dati:', userData);

      //  CHIAMA IL SERVICIO E DEBUGGA LA RISPOSTA
      const result = await this.dataServ.registerUser(userData);
      
      console.log('Risposta completa dal server:', result);
      console.log('Tipo risposta:', typeof result);
      console.log('Keys risposta:', Object.keys(result || {}));

      //  CONTROLLO PIÙ FLESSIBILE
      if (result) {
        // Prova diversi formati di risposta
        const userId = result.id || result.userId || result.data?.id;
        
        if (userId) {
          localStorage.setItem('userId', userId.toString());
          localStorage.setItem('username', userData.username);
          
          console.log('Registrazione completata. ID:', userId);
          this.router.navigate(['/login']);
        } else {
          // Se non c'è ID ma la chiamata è andata bene
          console.log('Registrazione OK ma senza ID. Response:', result);
          this.router.navigate(['/login']);
        }
      } else {
        console.log('Response vuoto ma chiamata OK');
        this.router.navigate(['/login']);
      }

    } catch (error) {
      console.error('Errore durante la registrazione:', error);
      alert('Errore durante la registrazione: ' + (error as Error).message);
    } finally {
      this.isLoading = false;
    }
  }
}
}