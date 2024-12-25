import * as React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import TextInput from '../common/TextInput';
import { SignUpFormReducer, newSignUpFormState } from './SignUpFormState';
import { FaCircleExclamation } from 'react-icons/fa6';
import { ImageInput } from '../common/FileInput';
import useHamroFashionClient from '../../Hooks/useHamroFashionClient';

const SignUpForm: React.FC = () => {
    const [state, dispatch] = React.useReducer(SignUpFormReducer, newSignUpFormState());
    const api = useHamroFashionClient();
    const redirect = useNavigate();
    const errors = state.errors ?? {};

    const handleCommandChange = React.useCallback((e: React.ChangeEvent<HTMLInputElement>) => 
        dispatch({ type: 'SET_COMMAND_FIELD', name: e.currentTarget.id, value: e.currentTarget.value }), []
    );

    const handleConfirmPasswordChange = React.useCallback((e: React.ChangeEvent<HTMLInputElement>) =>
        dispatch({ type: 'SET_CONFIRM_PASSWORD', value: e.currentTarget.value }), []
    );

    const handleImageChange = React.useCallback((name: string, image: string | ArrayBuffer | null) =>
        dispatch({ type: 'SET_IMAGE', name: name,  image: image}), []
    )

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

        if (state.command.password !== state.confirmPassword) {
            dispatch({ type: 'SET_ERROR', name: 'password', value: 'passwords do not match' });
            dispatch({ type: 'SET_ERROR', name: 'confirmPassword', value: 'passwords do not match' });
            isValid = false;
        }

        if(isValid) {
            try {
                console.log(state.command)
                await api.post('/user', state.command);

                redirect(`/signin`);
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
            Unlock exclusive deals and personalized picks — sign up now!
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
            <div className="relative z-0 w-full mb-8 group">
                <TextInput
                    id="confirmPassword"
                    label="Confirm Password"
                    onChange={handleConfirmPasswordChange}
                    placeholder="*********"
                    type="password"
                    value={state.confirmPassword}
                    errors={errors['confirmPassword']}
                />
            </div>
            <div className="relative z-0 w-full mb-8 group">
                <ImageInput 
                    label='Profile Picture' 
                    id='profilePicture'  
                    onChange={handleImageChange}
                    errors={errors['profilePicture']}
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
export default SignUpForm;