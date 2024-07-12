
export class CustomValidators {

    static regExps: { [key: string]: RegExp; } = {
        password: /^(?=.*[0-9])(?=.*[A-Z]).+$/
    };

}
