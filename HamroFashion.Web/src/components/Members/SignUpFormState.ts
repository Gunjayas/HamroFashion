import CreateUser from "../../api/Commands/CreateUser";

export interface SignUpFormState {
    /**
     * The CreateUser command the form is constructing
     */
    command: CreateUser;

    /**
     * Our field errors {string, string[]}
     */
    errors: any;

    confirmPassword: string;
}

export const newSignUpFormState = (): SignUpFormState => {
    return {
        command: {
            userName: '',
            emailAddress: '',
            password: '',
            profilePicture: ''
        } as CreateUser,
        confirmPassword: '',
        errors: {}
    };
};

export type SignUpFormActions = {
    type: 'SET_COMMAND_FIELD';
    name: string;
    value: string;
} | {
    type: 'SET_CONFIRM_PASSWORD';
    value: string;
} | {
    type: 'SET_IMAGE';
    image: string | ArrayBuffer | null;
    name: string;
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

export const SignUpFormReducer = (state: SignUpFormState, action: SignUpFormActions): SignUpFormState => {
    switch(action.type) {
        case 'SET_COMMAND_FIELD':
            const newCommand = state.command as any;
            newCommand[action.name] = action.value;

            return {
                ...state,
                command: newCommand
            };
        
        case 'SET_CONFIRM_PASSWORD':
        return {
            ...state,
            confirmPassword: action.value
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

        case 'SET_IMAGE':
            const anotherCommand = state.command as any;
            anotherCommand[`${action.name}Image`] = action.image;

            return {
                ...state,
                command: anotherCommand
            };
    }
}