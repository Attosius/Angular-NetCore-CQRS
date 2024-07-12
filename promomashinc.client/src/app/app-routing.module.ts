import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Step1Component } from './login/step1/step1.component';
import { Step2Component } from './login/step2/step2.component';

const routes: Routes = [
  // { path: '', component: Step1Component },
  { path: '', redirectTo: 'step1', pathMatch: 'full'  },
  { path: 'step1', component: Step1Component },
  { path: 'step2', component: Step2Component },
  { path: '**', redirectTo: 'step1', pathMatch: 'full' }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
