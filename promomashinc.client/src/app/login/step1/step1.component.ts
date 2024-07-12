import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { Route, Router } from '@angular/router';
import { UserData } from '../../models/country';
import { Helpers } from '../../common/helpers';
import { CustomValidators } from '../../common/customValidators';

@Component({
    selector: 'app-login-step1',
    templateUrl: './step1.component.html',
    styleUrls: ['./step1.component.scss', '../../app.component.scss']
})
export class Step1Component implements OnInit {
    public loginFormControl = new FormControl('', [Validators.required, Validators.email]);
    public passwordFormControl = new FormControl('', [
        Validators.required,
        Validators.pattern(CustomValidators.regExps['password'])
    ]);
    public confirmPasswordFormControl = new FormControl('', [Validators.required]);

    public agreeFormControl = new FormControl('', [Validators.required, requiredTrue()]);

    public passwordFormControlGroup = this.formBuilder.group({
        passwordFormControl: this.passwordFormControl,
        confirmPasswordFormControl: this.confirmPasswordFormControl
    }, {
        validators: [passwordMatch()]
    }
    );
    public userRegistrationForm: FormGroup;
    public userData: UserData | undefined;


    constructor(public router: Router,
        private formBuilder: FormBuilder) {
        this.userRegistrationForm = this.formBuilder.group({
            loginFormControl: this.loginFormControl,

            passwordFormControlGroup: this.passwordFormControlGroup,
            agreeFormControl: this.agreeFormControl
        });

    }

    ngOnInit() {
        this.userData = new UserData(history.state.userData);
        if (history.state.userData) {
            this.loginFormControl.setValue(this.userData.Email);
            this.passwordFormControl.setValue(this.userData.Password);
            this.confirmPasswordFormControl.setValue(this.userData.Password);
        }
    }

    public gotoStep2() {
        Helpers.markAsTouched(this.userRegistrationForm);

        if (this.userRegistrationForm.invalid) {
            return;
        }
        const user = new UserData();
        user.Email = this.loginFormControl.value!;
        user.Password = this.passwordFormControl.value!
        this.router.navigateByUrl('/step2', { state: { userData: user } });
    }
}

export function requiredTrue(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;
        if (value == false) {
            return { required: true }
        }
        // if (!value) { // todo check
        //     return null;
        // }
        return null;
    }
}

export function passwordMatch() {
    return (formGroup: FormGroup) => {
        const password = formGroup.controls['passwordFormControl'].value;
        const confirmPassword = formGroup.controls['confirmPasswordFormControl'].value;
        if (password === confirmPassword) {
            return null;
        }
        formGroup.controls['confirmPasswordFormControl'].setErrors({ "notsame": true });
        return true;
    }
}

