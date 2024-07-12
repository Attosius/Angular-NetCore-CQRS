import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Country, UserData } from '../../models/country';
import { catchError, delay, finalize, map, Observable, of } from 'rxjs';
import { Province } from '../../models/province';
import { Helpers } from '../../common/helpers';
import { Result, ResultGeneric } from '../../models/Result';

@Component({
	selector: 'app-step2',
	templateUrl: './step2.component.html',
	styleUrl: './step2.component.scss'
})
export class Step2Component implements OnInit {
	public countryFormControl = new FormControl('', [Validators.required]);
	public provinceFormControl = new FormControl('', [Validators.required]);

	// public matcher = new MyErrorStateMatcher();
	public selectedCountry = '';
	public selectedProvince = '';
	public isLoading = false;
	public countries: any[] = [];
	public provincies: any[] = [];
	public userData!: UserData;
	public userRegistrationForm: FormGroup;

	constructor(
		private router: Router,
		private http: HttpClient,
		private activatedRoute: ActivatedRoute,
		private formBuilder: FormBuilder) {

		this.userRegistrationForm = this.formBuilder.group({
			countryFormControl: this.countryFormControl,
			provinceFormControl: this.provinceFormControl,
		});
	}

	ngOnInit() {
		console.dir(history.state.userData);
		if (!history.state.userData) {
			this.router.navigateByUrl('/step1');
		}
		this.userData = new UserData(history.state.userData);

		this.getCountries().subscribe();
	}


	public getCountries(): Observable<any> {
		this.isLoading = true;
		return this.http.get<ResultGeneric<Country[]>>('/Dictionary/GetCountries').pipe(

			map(result => {
				if (result.IsFailure) {
					// this.popupService.error(getBrokerProfilesResult.Message);
					console.error(result.Exception);
					return;
				}

				this.countries = result.Value.map(o => new Country(o));
			}),
			catchError(err => {
				console.log('Error while get countries', err);
				return of([]);
			}),
			finalize(() => {
				this.isLoading = false;
			}),
		);
	}

	public getProvincies(countryCode: string): Observable<any> {
		this.isLoading = true;
		return this.http.get<ResultGeneric<Province[]>>(`/Dictionary/GetProvince?countryCode=${countryCode}`).pipe(

			map(result => {
				if (result.IsFailure) {
					// this.popupService.error(getBrokerProfilesResult.Message);
					console.error(result.Exception);
					return;
				}

				this.provincies = result.Value.map(o => new Province(o));
			}),
			catchError(err => {
				console.log('Error while get provincies', err);
				return of([]);
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
		this.selectedCountry = currentCountry;
		this.getProvincies(currentCountry).subscribe();
		// this.currentProvinceList = this.provinceList.filter(o => o.parent === currentCountry);
	}

	public gotoStep1() {
        this.router.navigateByUrl('/step1', { state: { userData: this.userData} });
	
	}

	public save() {
		for (var i in this.userRegistrationForm.controls) {
			Helpers.markAsTouched(this.userRegistrationForm);
		}
		if (this.userRegistrationForm.invalid) {
			// return;
		}
		this.userData.CountryCode = this.countryFormControl.value!;
		this.userData.ProvinceCode = this.provinceFormControl.value!
		this.isLoading = true;
		this.http.post<Result>(`/User/Save`, this.userData).pipe(

			map(result => {
				console.dir(result);
			}),
			catchError(err => {
				console.log('Error while save data', err);
				return of([]);
			}),
			finalize(() => {
				this.isLoading = false;
			}),
		).subscribe();
	}
}
