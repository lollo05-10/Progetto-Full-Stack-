import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { DataService } from '../../services/dataservice/dataservice';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-component',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  templateUrl: './login-component.html',
  styleUrls: ['./login-component.scss'],
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private dataServ = inject(DataService);
  private router = inject(Router);

  public isLoading = false;

  loginForm = this.fb.group({
    username: [
      '',
      [Validators.required, Validators.minLength(3), Validators.maxLength(10)],
    ],
    acceptTerms: [false, Validators.requiredTrue],
  });

  async onSubmit() {
    if (this.loginForm.valid) {
      try {
        this.isLoading = true;
        const formData = this.loginForm.value;

        const result = await this.dataServ.registerUser({
          username: formData.username!,
        });

        // Salva userId come stringa nel localStorage
        localStorage.setItem('userId', result.id.toString());

        console.log('Login completata:', result);

        // Vai al feed dopo login
        this.router.navigate(['/feed']);
      } catch (error) {
        console.error('Errore durante il login:', error);
        alert('Errore durante il login. Riprova.');
      } finally {
        this.isLoading = false;
      }
    }
  }
}
