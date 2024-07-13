

export class Result {
    public Message = '';
    public IsSuccess = false;
    public IsFailure = true;
    public Type = ResultType.Success;
    public Exception = '';
    public ErrorList: string[] = [];


    public static Success(): Result {
        const r = new Result();
        r.IsFailure = false;
        r.IsSuccess = true;
        return r;
    }
    
    public static Error(message: string = 'Sevice unavailable'): Result {
        const r = new Result();
        r.IsFailure = true;
        r.IsSuccess = false;
        r.Message = message;
        return r;
    }


    constructor(json: any = null) {
        if (!json) {
            return;
        }
        Object.assign(this, json);
    }
}


export class ResultGeneric<T> extends Result {
    public Value!: T;

    constructor(json: any = null) {
        super(json);
        Object.assign(this, json);
    }
    public static Empty<T>() {
        const r = new ResultGeneric<T>();
        return r;
    }

    public static CreateObjects<T>(json: ResultGeneric<T[]>, type: { new(json: any): T; } ) {
        let result = null;
        if (Array.isArray(json.Value)) {
            result = new ResultGeneric<T[]>(json);
            result.Value = json.Value.map(o => new type(o));
        } else {
            result = new ResultGeneric<T>(json);
            result.Value = new type(json.Value);
        }
        return result;
    }
}

export enum ResultType
{
    None = 0,
    Success = 1,
    Debug = 2,
    Info = 3,
    Warn = 4, 
    Error = 5 
}

