

// interface WeatherForecast {
//     date: string;
//     temperatureC: number;
//     temperatureF: number;
//     summary: string;

import { BaseConstructor } from "./base-constructor";
import { Province } from "./province";


//   }


export class Country extends BaseConstructor {
    public Code = ''
    public DisplayText = '';

    constructor(json: Province | any = null) {
        super(json);
        this.Assign(json);
    }
}

