import GetUser from "../../api/Commands/GetUser";


export interface SignInFormState {
    /**
     * The GetUser command the form is constructing
     */
    command: GetUser;

    /**
     * Our field errors {string, string[]}
     */
    errors: any;

}

export const newSignInFormState = (): SignInFormState => {
    return {
        command: {
            userName: '',
            emailAddress: '',
            password: ''
        } as GetUser,
        errors: {}
    };
};

export type SignInFormActions = {
    type: 'SET_COMMAND_FIELD';
    name: string;
    value: string;
} | {
    type: 'SET_ERROR';
    name: string;
    value: string;
} | {
    type: 'SET_ERRORS';
    errors: any;
} | {
    type: 'CLEAR_ERRORS';
};

export const SignInFormReducer = (state: SignInFormState, action: SignInFormActions): SignInFormState => {
    switch(action.type) {
        case 'SET_COMMAND_FIELD':
            const newCommand = state.command as any;
            newCommand[action.name] = action.value;

            return {
                ...state,
                command: newCommand
            };
        
        case 'SET_ERROR':
            const newErrors = state.errors as any;
            newErrors[action.name] = [
                ...newErrors[action.name] ?? [],
                action.value
            ]

            return {
                ...state,
                errors: newErrors
            }
        
        case 'SET_ERRORS':
            return {
                ...state,
                errors: action.errors
            }
        
        case 'CLEAR_ERRORS':
            return {
                ...state,
                errors: {}
            }
    }
}