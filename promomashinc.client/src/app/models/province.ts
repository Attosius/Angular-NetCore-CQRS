import { BaseConstructor } from "./base-constructor";


export class Province extends BaseConstructor {
    public Code = '';
    public ParentCode = '';
    public DisplayText = '';

    constructor(json: Province | any = null) {
        super(json);
        this.Assign(json);
    }
}
