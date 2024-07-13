import { BaseConstructor } from "./base-constructor";


export class UserData extends BaseConstructor {

    public Email = ''
    public Password = '';
    public CountryCode = '';
    public ProvinceCode = '';

    constructor(json: UserData | any = null) {
        super(json);
        this.Assign(json);
    }
}
