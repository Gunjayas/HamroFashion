import { FaCircleExclamation } from "react-icons/fa6";
import useHamroFashionClient from "../../Hooks/useHamroFashionClient";
import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { CheckOutFormReducer, newCheckOutFormState } from "./CheckOutFormState";
import TextInput from "../common/TextInput";

const CheckOutForm: React.FC = () => {
    const [state, dispatch] = React.useReducer(CheckOutFormReducer, newCheckOutFormState());
    const api = useHamroFashionClient();
    const redirect = useNavigate();
    const errors = state.errors ?? {};

    const handleCommandChange = React.useCallback((e: React.ChangeEvent<HTMLInputElement>) => 
        dispatch({ type: 'SET_COMMAND_FIELD', name: e.currentTarget.id, value: e.currentTarget.value }), []
    );

    const handleSubmit = React.useCallback(async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        dispatch({ type: 'CLEAR_ERRORS' });
        let isValid = true;

        if (!/(.+)@(.+){2,}\.(.+){2,}/.test(state.command.emailAddress)) {
            dispatch({ type: 'SET_ERROR', name: 'emailAddress', value: 'please provide a valid email address' });
            isValid = false;
        }

        if(isValid) {
            try {
                console.log(state.command)
                await api.post('/user/placeorder', state.command);
                await api.post('initiate/payment', state.command);

            } catch (err: any) {
                if (err.statusCode === 400 || err.message.statusCode === 400)
                    dispatch({ type: 'SET_ERRORS', errors: err.fieldErrors });
            }
        }
    }, [api, state, redirect]);
    
return <form className="py-8 mx-auto border-green-600 2xl:max-w-xl lg:max-w-lg sm:max-w-md xs:max-w-sm" onSubmit={handleSubmit}>
            <div className="relative z-0 w-full py-4 mb-5 bg-green-600 group rounded-t-xl">
                <h1 className='text-2xl font-bold ps-6'>Sign Up</h1>
            </div>
            <div className='text-white text-md'>
                Enter the gateway to your virtual adventures. Sign up now to explore, interact, and conquer in the realm of api games!
            </div>
            <hr className='my-4'/>
            <div className="relative z-0 w-full mb-5 group">
                <TextInput
                    id="name"
                    label="name"
                    onChange={handleCommandChange}
                    placeholder="your name"
                    type="text"
                    value={state.command.name}
                    errors={errors['name']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <TextInput
                    id="emailAddress"
                    label="Email Address"
                    onChange={handleCommandChange}
                    placeholder="you@email.com"
                    type="email"
                    value={state.command.emailAddress}
                    errors={errors['emailAddress']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <TextInput
                    id="phone"
                    label="Phone Number"
                    onChange={handleCommandChange}
                    placeholder="your number"
                    type="text"
                    value={state.command.phone}
                    errors={errors['phone']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <TextInput
                    id="address"
                    label="Delievery Address"
                    onChange={handleCommandChange}
                    placeholder="address"
                    type="text"
                    value={state.command.address}
                    errors={errors['address']}
                />
            </div>
            <div className='flex items-center justify-between text-green-500'>
                <Link to='/signin' className='underline'>Already Have a Account?</Link>
                <button type="submit" className="text-white hover:bg-green-800 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center bg-green-600 focus:ring-green-800">Sign Up</button>
            </div>
            {errors[''] && <p className="text-xs font-semibold text-red-600">
                <FaCircleExclamation className="inline ml-2 mr-1" />
                {errors['']}
            </p>}

        </form>
}
export default CheckOutForm;