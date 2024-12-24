import * as React from 'react';
import { FaCircleExclamation } from 'react-icons/fa6';

interface SelectInputProps {
    disabled?: boolean;
    id: string;
    errors?: string[];
    label: string;
    name?: string;
    onChange: React.ChangeEventHandler<HTMLSelectElement>;
    value: string | undefined;
    placeholder?: string;
    options: string[];
}
const SelectInput: React.FC<SelectInputProps> = (props: SelectInputProps) => {
return <>
        <label className="block mb-2 text-sm font-medium text-gray-400">{props.label}</label>
        <select id={props.id} value={props.value} onChange={props.onChange} disabled={props.disabled} name={props.id} className="bordertext-gray-900 text-sm rounded-lg block w-full p-1.5 bg-gray-700 border-gray-600 placeholder-gray-400 text-gray-400">
        <option selected>{props.value? 'None' : (props.placeholder? props.placeholder : 'Choose below')}</option>
        {props.options.map((opt) => <option value={opt} className='p-8'>{opt}</option>)}
        </select>
        {props.errors?.map((error, i) => <p className='text-red-600 text-xs font-semibold'><FaCircleExclamation className="inline ml-2 mr-1" key={`error-${props.name ?? props.id}-${i}`}/>{error}
        </p>)}
     </>
}
export default SelectInput;