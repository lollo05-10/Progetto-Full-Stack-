import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import {
  MatCardContent,
  MatCard,
  MatCardHeader,
  MatCardTitle,
} from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/dataservice/dataservice';

@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
    MatCardContent,
    MatCard,
    MatCardHeader,
    MatCardTitle,
  ],
  templateUrl: './login-component.html',
  styleUrl: './login-component.scss',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private dataServ = inject(DataService);
  private router = inject(Router);

  public isLoading = false;

  //  FORM SEMPLIFICATO - SOLO USERNAME
  loginForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
  });

  async onSubmit() {
    if (this.loginForm.valid) {
      try {
        this.isLoading = true;
        const username = this.loginForm.value.username;

        if (!username) {
          throw new Error('Username obbligatorio');
        }

        console.log(' Tentativo login per:', username);

        //  CERCA L'UTENTE PER USERNAME
        const user = await this.dataServ.getUserByUsername(username);

        if (user) {
          //  LOGIN SUCCESSO - SALVA NEL LOCALSTORAGE
          localStorage.setItem('userId', user.id.toString());
          localStorage.setItem('username', user.username);
          localStorage.setItem('isAdmin', user.isAdmin ? 'true' : 'false');
          
          console.log(' Login completato:', user);
          this.router.navigate(['/new-report']);
        } else {
          throw new Error('Utente non trovato');
        }

      } catch (error) {
        console.error(' Errore durante il login:', error);
        alert('Errore durante il login: ' + (error as Error).message);
      } finally {
        this.isLoading = false;
      }
    } else {
      //  SE IL FORM NON È VALIDO
      this.markFormGroupTouched();
    }
  }

  //  METODO PER MARCARE TUTTI I CAMPI COME TOCCATI
  private markFormGroupTouched() {
    Object.keys(this.loginForm.controls).forEach(key => {
      const control = this.loginForm.get(key);
      control?.markAsTouched();
    });
  }

  //  GETTER PER FACILE ACCESSO AI CONTROLLI
  get username() { return this.loginForm.get('username'); }
}