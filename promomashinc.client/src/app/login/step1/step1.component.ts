import { Component } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-login-step1',
  templateUrl: './step1.component.html',
  styleUrl: './step1.component.scss'
})
export class Step1Component {
  loginFormControl = new FormControl('', [Validators.required]);
  passwordFormControl = new FormControl('', [Validators.required]);
  confirmPasswordFormControl = new FormControl('', [Validators.required]);
  agreeFormControl = new FormControl('', [Validators.required]);
  matcher = new MyErrorStateMatcher();
  public login = '';

  constructor(public router: Router) {

  }

  public gotoStep2() {
    this.router.navigate(['/step2']);
  }
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  public isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    // const isSubmitted = form && form.submitted;
    const empty = control?.value;
    return !!(control && control.invalid && (control.dirty || control.touched || empty));
    // return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }

}