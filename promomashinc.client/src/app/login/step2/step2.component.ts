import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MyErrorStateMatcher } from '../step1/step1.component';
import { HttpClient } from '@angular/common/http';
import { Country } from '../../models/country';
import { catchError, delay, finalize, map, Observable, of } from 'rxjs';
import { Province } from '../../models/province';

@Component({
	selector: 'app-step2',
	templateUrl: './step2.component.html',
	styleUrl: './step2.component.scss'
})
export class Step2Component  implements OnInit {
	public countryFormControl = new FormControl('', [Validators.required]);
	public provinceFormControl = new FormControl('', [Validators.required]);

	public matcher = new MyErrorStateMatcher();
	public selectedCountry = '';
	public selectedProvince = '';
	public isLoading = false;
	public countries: any[] = [];
	public provincies: any[] = [];

	constructor(
		private router: Router,
		private http: HttpClient) {

	}

    ngOnInit() {
        this.getCountries().subscribe();
    }


	public getCountries() : Observable<any>{
		this.isLoading = true;
		return this.http.get<Country[]>('/weatherforecast/GetCountries').pipe(
			
			map(result => {
					this.countries = result.map(o => new Country(o));
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

	public getProvincies(countryCode: string) : Observable<any>{
		this.isLoading = true;
		return this.http.get<Country[]>(`/weatherforecast/GetProvince?countryCode=${countryCode}`).pipe(

			map(result => {
					this.provincies = result.map(o => new Province(o));
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

		this.router.navigate(['/step1']);

	}
	public save() {


	}
}
