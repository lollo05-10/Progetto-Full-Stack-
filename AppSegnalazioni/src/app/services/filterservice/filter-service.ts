import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class FilterService {
  private categorieSelezionate = new BehaviorSubject<string[]>([]);
  categorieSelezionate$ = this.categorieSelezionate.asObservable();

  setCategorie(categorie: string[]) {
    this.categorieSelezionate.next(categorie);
  }
}

//Questo serve per leggere e/o aggiornare il filtro in tempo reale
