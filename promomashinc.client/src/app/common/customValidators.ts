import { ValidatorFn, AbstractControl, ValidationErrors, FormGroup } from "@angular/forms";

export class CustomValidators {

    static regExps: { [key: string]: RegExp; } = {
        password: /^(?=.*[0-9])(?=.*[A-Z]).+$/
    };

    static requiredTrue(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            const value = control.value;
            if (!value && !control.untouched) {
                return { required: true }
            }
            return null;
        }
    }
    
    static passwordMatch(): ValidationErrors | null {
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
}
