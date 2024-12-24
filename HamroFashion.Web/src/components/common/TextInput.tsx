import * as React from 'react';
import { FaCircleExclamation } from 'react-icons/fa6';

/**
 * Custom props for our TextInput component
 */
interface TextInputProps {
    disabled?: boolean;
    id: string;
    errors?: string[];
    label: string;
    name?: string;
    onChange: React.ChangeEventHandler<HTMLInputElement>;
    pattern?: string;
    placeholder?: string;
    type: string;
    value: string | undefined;
    req?: boolean;
}

/**
 * Represents a basic text input
 * 
 * @param props TextInputProps
 * @returns <TextInput />
 */
const TextInput: React.FC<TextInputProps> = (props: TextInputProps) => {
return <div>
        <input disabled={props.disabled} id={props.id} name={props.name ?? props.id} onChange={props.onChange} type={props.type} value={props.value} className="block py-2.5 px-0 w-full text-sm text-white bg-transparent border-0 border-b-2 appearance-none border-gray-600 focus:outline-none focus:ring-0 focus:border-green-600 peer" placeholder=" " {...(props.req ? {required: true} : {})} />
        <label className="peer-focus:font-medium absolute text-sm text-gray-400 duration-300 transform -translate-y-6 scale-75 top-3 -z-10 origin-[0] peer-focus:start-0 rtl:peer-focus:translate-x-1/4 rtl:peer-focus:left-auto peer-focus:text-green-500 peer-placeholder-shown:scale-100 peer-placeholder-shown:translate-y-0 peer-focus:scale-75 peer-focus:-translate-y-6">{props.label}</label>
        {props.errors?.map((error, i) => <p className='text-red-600 text-xs font-semibold'><FaCircleExclamation className="inline ml-2 mr-1" key={`error-${props.name ?? props.id}-${i}`}/>{error}
        </p>)}
     </div>
}
export default TextInput;