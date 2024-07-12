
export class BaseConstructor {

    constructor(json: any) {
        Object.assign(this, json);
    }

    public Assign(json: any) {
        if (!json) {
            json = {};
        }
        Object.assign(this, json);
        return json;
    }
}

