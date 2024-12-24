import * as React from 'react';
import TextInput from '../common/TextInput';
import { FaCircleExclamation } from 'react-icons/fa6';
import { Link, useNavigate } from 'react-router-dom';
import { SignInFormReducer, newSignInFormState } from './SignInFormState';
import useHamroFashionClient from '../../Hooks/useHamroFashionClient';
import AuthenticationSuccessful from '../../api/Events/AuthenticationSuccessful';

const SignInPage: React.FC = () => {
    const[state, dispatch] = React.useReducer(SignInFormReducer, newSignInFormState());
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

        if (state.command.password.length < 8) {
            dispatch({ type: 'SET_ERROR', name: 'password', value: 'please provide a password atleast 8 characters' });
            isValid = false;
        }

        if(isValid) {
            try {
                api.clearSession();
                const res: AuthenticationSuccessful = await api.post('/token/auth/credentials', state.command);
                console.log(res);
                api.setSession(res);
                redirect('/');
            } catch (err: any) {
                if (err.statusCode === 400 || err.message.statusCode === 400)
                    dispatch({ type: 'SET_ERRORS', errors: err.fieldErrors });
            }
        }
    }, [api, state, redirect]);

return <div className='bg-gray-800'>
        <form className="py-8 mx-auto border-green-600 2xl:max-w-xl lg:max-w-lg sm:max-w-md xs:max-w-sm" onSubmit={handleSubmit}>
            <div className="relative z-0 w-full py-4 mb-5 bg-green-600 group rounded-t-xl">
                <h1 className='text-2xl font-bold ps-6'>Sign In</h1>
            </div>
            <div className='text-white text-md'>
                Enter the gateway to your virtual adventures. Sign in now to explore, interact, and conquer in the realm of api games!
            </div>
            <hr className='my-4'/>
            <div className="relative z-0 w-full mb-5 group">
                <TextInput
                    id="userName"
                    label="Username"
                    onChange={handleCommandChange}
                    placeholder="your name"
                    type="text"
                    value={state.command.userName}
                    errors={errors['userName']}
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
            <div className="relative z-0 w-full mb-8 group">
                <TextInput
                    id="password"
                    label="Password"
                    onChange={handleCommandChange}
                    placeholder="*********"
                    type="password"
                    value={state.command.password}
                    errors={errors['password']}
                />
            </div>
            <div className='flex items-center justify-between text-green-500'>
                <Link to='/signup' className='underline'>Create a Account</Link>
                <Link to='/somewhere' className='underline'>Forgot Password?</Link>
                <button type="submit" className="text-white hover:bg-green-800 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center bg-green-600 focus:ring-green-800">Sign In</button>
            </div>
            {errors[''] && <p className="text-xs font-semibold text-red-600">
                <FaCircleExclamation className="inline ml-2 mr-1" />
                {errors['']}
            </p>}

        </form>
    </div>

}
export default SignInPage;