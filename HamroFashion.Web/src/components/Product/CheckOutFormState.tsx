import PlaceOrder from "../../api/Commands/PlaceOrder";


export interface CheckOutFormState {
    /**
     * The PlaceOrder command the form is constructing
     */
    command: PlaceOrder;

    /**
     * Our field errors {string, string[]}
     */
    errors: any;

}

export const newCheckOutFormState = (): CheckOutFormState => {
    return {
        command: {
            name: '',
            emailAddress: '',
            address: '',
            phone: '',
            amount: ''
        } as PlaceOrder,
        errors: {}
    };
};

export type CheckOutFormActions = {
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

export const CheckOutFormReducer = (state: CheckOutFormState, action: CheckOutFormActions): CheckOutFormState => {
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