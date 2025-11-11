import { Routes } from '@angular/router';
import { MapComponent } from './componenti/map-component/map-component';
import { FeedComponent } from './componenti/feed-component/feed-component';
import { DetailComponent } from './componenti/detail-component/detail-component';
import { UserComponent } from './componenti/user-component/user-component';
import { NotfoundComponent } from './componenti/notfound-component/notfound-component';
import { NewReportComponent } from './componenti/new-report-component/new-report-component';
import { RegisterComponent } from './componenti/register-component/register-component';

export const routes: Routes = [
  { path: '', redirectTo: '/map', pathMatch: 'full' },
  { path: 'map', component: MapComponent },
  { path: 'feed', component: FeedComponent },
  { path: 'detail/:id', component: DetailComponent },
  { path: 'new-report', component: NewReportComponent },
  { path: 'user', component: UserComponent },
  { path:'register',component: RegisterComponent },
  { path: '**', component: NotfoundComponent },
];
