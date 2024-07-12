import { AbstractControl, FormGroup } from "@angular/forms";

export class Helpers {
    public static markAsTouched(control: AbstractControl) {
        if (control instanceof FormGroup) {
            const controls = (control as FormGroup).controls
            for (const name in controls) {
                this.markAsTouched(controls[name]);
            }
        }
        control.markAsTouched();
    }
}


