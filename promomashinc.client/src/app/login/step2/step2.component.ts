import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError,  finalize, map, Observable, of } from 'rxjs';
import { Province } from '../../models/province';
import { Helpers } from '../../common/helpers';
import { ToastrService } from 'ngx-toastr';
import { Result, ResultGeneric } from '../../models/result';
import { Country } from '../../models/country';
import { UserData } from '../../models/user-data';
import { LoginService } from '../services/login-service';

@Component({
	selector: 'app-step2',
	templateUrl: './step2.component.html',
    styleUrls: ['./step2.component.scss', '../../app.component.scss']
})
export class Step2Component implements OnInit {
	public userRegistrationForm: FormGroup;
	public countryFormControl = new FormControl('', [Validators.required]);
	public provinceFormControl = new FormControl('', [Validators.required]);

	public isLoading = false;
	public countries: Country[] = [];
	public provincies: Province[] = [];
	public userData!: UserData;

	constructor(
		private router: Router,
		private formBuilder: FormBuilder,
		private loginService: LoginService,
		private toastr: ToastrService) {

		this.userRegistrationForm = this.formBuilder.group({
			countryFormControl: this.countryFormControl,
			provinceFormControl: this.provinceFormControl,
		});
	}

	ngOnInit() {
		if (!history.state.userData) {
			this.router.navigateByUrl('/step1');
		}
		this.userData = new UserData(history.state.userData);
		this.getCountries().subscribe();
	}


	public getCountries(): Observable<Result | any > {
		this.isLoading = true;
		return this.loginService.getCountries().pipe(
			map(result => {
				if (result.IsFailure) {
					this.errorCatch(result.Exception, result.Message);
					return;
				}

				this.countries = result.Value.map(o => new Country(o));
			}),
			catchError((error) => {
				this.errorCatch(error, 'Error while get countries');
				return of(Result.Error());
			}),
			finalize(() => {
				this.isLoading = false;
			}),
		);
	}


	public getProvincies(countryCode: string): Observable<any> {
		this.isLoading = true;
		return this.loginService.getProvincies(countryCode).pipe(
			map(result => {
				if (result.IsFailure) {
					this.errorCatch(result.Exception, result.Message);
					return;
				}

				this.provincies = result.Value.map(o => new Province(o));
			}),
			catchError(err => {
				return this.errorCatch(err, 'Error while get provincies');
			}),
			finalize(() => {
				this.isLoading = false;
			}),
		);
	}

	public onCountrySelected(currentCountry: string) {
		if (!currentCountry) {
			this.provincies = [];
			return;
		}
		this.getProvincies(currentCountry).subscribe();
	}

	public gotoStep1() {
        this.router.navigateByUrl('/step1', { state: { userData: this.userData} });
	
	}

	public save() {
		for (var i in this.userRegistrationForm.controls) {
			Helpers.markAsTouched(this.userRegistrationForm);
		}
		if (this.userRegistrationForm.invalid) {
			return;
		}
		this.userData.CountryCode = this.countryFormControl.value!;
		this.userData.ProvinceCode = this.provinceFormControl.value!
		this.isLoading = true;
		this.loginService.saveUser(this.userData).pipe(

			map(result => {
				if (result.IsFailure) {
					this.errorCatch(result.Exception, result.Message);
					return;
				}
				this.toastr.success('User successfully created')
			}),
			catchError(err => {
				return this.errorCatch(err, 'Error while save data');
			}),
			finalize(() => {
				this.isLoading = false;
			}),
		).subscribe();
	}

	
	private errorCatch(err: any, message: string): Observable<Result> {
		this.toastr.error(message);
		console.log(message, err);
		return of(Result.Error(message));
	}
}
