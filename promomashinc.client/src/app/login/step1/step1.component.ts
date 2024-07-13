import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Helpers } from '../../common/helpers';
import { CustomValidators } from '../../common/customValidators';
import { UserData } from '../../models/user-data';

@Component({
    selector: 'app-login-step1',
    templateUrl: './step1.component.html',
    styleUrls: ['./step1.component.scss', '../../app.component.scss']
})
export class Step1Component implements OnInit {

    public loginFormControl = new FormControl('', [Validators.required, Validators.email]);
    public passwordFormControl = new FormControl('', [ Validators.required, Validators.pattern(CustomValidators.regExps['password'])]);
    public confirmPasswordFormControl = new FormControl('', [Validators.required]);
    public agreeFormControl = new FormControl('', [Validators.required, CustomValidators.requiredTrue()]);

    public passwordFormControlGroup = this.formBuilder.group({
            passwordFormControl: this.passwordFormControl,
            confirmPasswordFormControl: this.confirmPasswordFormControl
        }, { validators: [CustomValidators.passwordMatch()]} as AbstractControlOptions);

    public userRegistrationForm: FormGroup;
    public userData!: UserData;


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


