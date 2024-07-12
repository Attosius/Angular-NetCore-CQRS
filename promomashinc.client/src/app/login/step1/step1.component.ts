import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { Route, Router } from '@angular/router';
import { UserData } from '../../models/country';
import { Helpers } from '../../common/helpers';

@Component({
    selector: 'app-login-step1',
    templateUrl: './step1.component.html',
    styleUrl: './step1.component.scss'
})
export class Step1Component implements OnInit {
    public loginFormControl = new FormControl('', [Validators.required, Validators.email]);
    public passwordFormControl = new FormControl('', [
        Validators.required,
        Validators.pattern(CustomValidators.regExps['password'])
        // createPasswordStrengthValidator()
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
    // public matcher = new MyErrorStateMatcher();
    // public login = '';

    public userRegistrationForm: FormGroup;
    public userData: UserData | undefined;


    // form = this.formBuilder.group({
    //     email: ['', {
    //         validators: [
    //             Validators.required,
    //             Validators.email
    //         ],
    //         updateOn: 'blur'
    //     }],
    //     password: [
    //         '',
    //         [Validators.required, Validators.minLength(8),
    //         createPasswordStrengthValidator()
    //         ]
    //     ]
    // }, {
    //     validators: [passwordMatch()]
    // });

    // get email() {
    //     return this.form.controls['email'];
    // }

    // get password() {
    //     return this.form.controls['password'];
    // }

    constructor(public router: Router,
        private formBuilder: FormBuilder) {
        this.userRegistrationForm = this.formBuilder.group({
            loginFormControl: this.loginFormControl,

            passwordFormControlGroup: this.passwordFormControlGroup,
            agreeFormControl: this.agreeFormControl
        });

        // this.createForm();
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
            // return;
        }
        const user = new UserData();
        user.Email = this.loginFormControl.value!;
        user.Password = this.passwordFormControl.value!
        this.router.navigateByUrl('/step2', { state: { userData: user } });
        // this.router.navigate(['/step2']);
    }
}

export function requiredTrue(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;
        if (value == false) {
            return { required: true }
        }
        if (!value) { // todo check
            return null;
        }
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
// export function passwordMatch2(password: string, confirmPassword: string) {
//     return (formGroup: FormGroup) => {
//       const passwordControl = formGroup.controls[password];
//       const confirmPasswordControl = formGroup.controls[confirmPassword];

//       if (!passwordControl || !confirmPasswordControl) {
//         return null;
//       }

//       if (
//         confirmPasswordControl.errors &&
//         !confirmPasswordControl.errors.passwordMismatch
//       ) {
//         return null;
//       }

//       if (passwordControl.value !== confirmPasswordControl.value) {
//         confirmPasswordControl.setErrors({ passwordMismatch: true });
//       } else {
//         confirmPasswordControl.setErrors(null);
//       }
//     };
//   }
// export function creatDateRangeValidator(): ValidatorFn {
//   return (form: FormGroup): ValidationErrors | null => {
//     if (!form) {
//       return null;
//     }
//     // const start: Date = form.get("startAt").value;

//     // const end: Date = form.get("endAt").value;

//     // if (start && end) {
//     //   const isRangeValid = (end.getTime() - start.getTime() > 0);

//     //   return isRangeValid ? null : { dateRange: true };
//     // }

//     return null;
//   }
// }

export function createPasswordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

        const value = control.value;

        if (!value) {
            return null;
        }

        const hasUpperCase = /[A-Z]+/.test(value);

        const hasLowerCase = /[a-z]+/.test(value);

        const hasNumeric = /[0-9]+/.test(value);

        const passwordValid = hasUpperCase && hasLowerCase && hasNumeric;

        return !passwordValid ? { passwordStrength: true } : null;
    }
}

export class CustomValidators {

    /**
  * Collection of reusable RegExps
  */
    static regExps: { [key: string]: RegExp } = {
        // password: /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{7,15}$/
        password: /^(?=.*[0-9])(?=.*[A-Z]).+$/
    };

    static confirmValidator = (password: FormControl) =>
        (confirmPassword: FormControl): ValidationErrors | null => {

            // return a object with error type if there is some kind of error 
            // return  null if there is no error
            return {
                "email": "error1"
            }

        }

    /**
     * Validates that child controls in the form group are equal
     */
    // static childrenEqual: ValidatorFn = (formGroup: FormGroup) => {
    //     const [firstControlName, ...otherControlNames] = Object.keys(formGroup.controls || {});
    //     // const isValid = otherControlNames.every(controlName => formGroup.get(controlName).value === formGroup.get(firstControlName).value);
    //     const isValid = false;
    //     return isValid ? null : { childrenNotEqual: true };
    // }
}

/**
* Custom ErrorStateMatcher which returns true (error exists) when the parent form group is invalid and the control has been touched
*/
// export class ConfirmValidParentMatcher implements ErrorStateMatcher {
//   isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
//       return control.parent.invalid && control.touched;
//   }
// }



/**
* Collection of reusable error messages
*/
// export const errorMessages: { [key: string]: string } = {
//     fullName: 'Full name must be between 1 and 128 characters',
//     email: 'Email must be a valid email address (username@domain)',
//     confirmEmail: 'Email addresses must match',
//     password: 'Password must be between 7 and 15 characters, and contain at least one number and special character',
//     confirmPassword: 'Passwords must match'
// };


// export class MyErrorStateMatcher implements ErrorStateMatcher {
//     public isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
//         // const isSubmitted = form && form.submitted;
//         const empty = control?.value;
//         return !!(control && control.invalid && (control.dirty || control.touched || empty));
//         // return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
//     }

// }