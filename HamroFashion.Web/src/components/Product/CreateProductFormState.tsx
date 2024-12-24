import CreateProduct from "../../api/Commands/CreateProduct";
import User from "../../Models/User";

export interface CreateProductFormState {
    /**
     * The CreateProduct command the form is constructing
     */
    command: CreateProduct;

    /**
     * Our field errors {string, string[]}
     */
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    errors: any;

    user?: User;
}

export const newProductFormState = (): CreateProductFormState => {
    return{
        command: { 
            name: '',
            description: '',
            color: '',
            imageUrl: '',
            size: '',
            quantity: '',
            price: '',
            availability: true,
            productCategory: '',
            productCollection: [],
            productLabel: []
        } as CreateProduct,
        errors: {}
    };
        
};

export type CreateProductFormActions = {
    type: 'SET_COMMAND_FIELD';
    name: string;
    value: string;
} | {
    type: 'SET_MULTISELECT';
    name: string;
    value: string[];
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

export const CreateProductFormReducer = (state: CreateProductFormState, action: CreateProductFormActions): CreateProductFormState => {
    switch(action.type) {
        case 'SET_COMMAND_FIELD':
            const newCommand = state.command as any;
            newCommand[action.name] = action.value;

            return {
                ...state,
                command: newCommand
            };
        
        case 'SET_MULTISELECT':
            const firstCommand = state.command as any;
            firstCommand[action.name] = action.value;

            return {
                ...state,
                command: firstCommand
            }
            
        case 'SET_IMAGE':
            const anotherCommand = state.command as any;
            anotherCommand[`${action.name}Image`] = action.image;

            return {
                ...state,
                command: anotherCommand
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
};
