import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Province } from "../../models/province";
import { Result, ResultGeneric } from "../../models/result";
import { UserData } from "../../models/user-data";
import { Country } from "../../models/country";


@Injectable({
    providedIn: 'root'
})
export class LoginService {

    constructor(
        private http: HttpClient
    ) { }

    private apiEndpointUrlDictionary = '/Dictionary/';  
    private apiEndpointUrlUser = '/User/';  

    public getCountries(): Observable<ResultGeneric<Country[]>> {
		return this.http.get<ResultGeneric<Country[]>>(`${this.apiEndpointUrlDictionary}GetCountries`);
	}

    public getProvincies(countryCode: string): Observable<ResultGeneric<Province[]>> {
		return this.http.get<ResultGeneric<Province[]>>(`${this.apiEndpointUrlDictionary}GetProvince?countryCode=${countryCode}`);
	}

    public  saveUser(userData: UserData) : Observable<Result> {
		return this.http.post<Result>(`${this.apiEndpointUrlUser}/Save`, userData);
	}
}
