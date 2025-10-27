import { Routes } from '@angular/router';
import { MapComponent } from './componenti/map-component/map-component';
import { FeedComponent } from './componenti/feed-component/feed-component';
import { DetailComponent } from './componenti/detail-component/detail-component';
import { UserComponent } from './componenti/user-component/user-component';
import { NotfoundComponent } from './componenti/notfound-component/notfound-component';

export const routes: Routes = [
  { path: '', redirectTo: '/map', pathMatch: 'full' },
  { path: 'map', component: MapComponent },
  { path: 'feed', component: FeedComponent },
  { path: 'detail/:id', component: DetailComponent },
  { path: 'user', component: UserComponent },
  { path: '**', component: NotfoundComponent },
];
