import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EventDetailComponent } from './event-detail.component';
import { BookingComponent } from './booking/booking.component';

const routes: Routes = [
  { path: 'events/:id', component: EventDetailComponent },
  { path: 'booking/:id', component: BookingComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
