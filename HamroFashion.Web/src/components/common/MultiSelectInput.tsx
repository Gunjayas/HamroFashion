import * as React from 'react';
import { FaCircleExclamation } from 'react-icons/fa6';
import Select, { MultiValue } from 'react-select';

export interface MultiSelectOption {
    label: string;
    value: string;
};

/**
 * Custom properties for our MultiSelectInput control
 */
interface MultiSelectInputProps {
    /**
     * Error message to display for this input
     */
    errors?: string[];

    selectedValues: MultiSelectOption[];
    /**
     * Label to display for this input
     */
    label: string;

    /**
     * Name of the value this input controls (used when textbox calls onChange)
     */
    id: string;

    /**
     * The onchange method this input will call
     * 
     * @param id Will be the name from props
     * @param value Will be the current value of the input
     */
    onChange: (id: string, value: string[]) => void;

    /**
     * Options that the user can select from
     */
    options: MultiSelectOption[];

    /**
     * Currently selected option values
     */
    value?: string[];
};

/**
 * That cool drop down select input that shows your options as pills
 * 
 * @param props MultiSelectInputProps
 */
const MultiSelectInput: React.FC<MultiSelectInputProps> = (props: MultiSelectInputProps) => {
    const handleChange = React.useCallback((options: MultiValue<MultiSelectOption>) => {
      console.log(options)
        const values = options.map(option => option.value);
        props.onChange(props.id, values);
    }, [props]);

    return <div className="mb-3">
        <div>
            <label htmlFor="genre" className="block mb-2 text-sm font-medium text-gray-400">{props.label}</label>
            <Select
                isMulti
                onChange={handleChange}
                options={props.options}
                defaultValue={props.selectedValues}
                id={props.id}
                styles={{
                    menuPortal: base => ({ ...base, zIndex: 9999 }), // Adjust z-index
                    menu: (provided) => ({
                        ...provided,
                        padding: '0px',
                        fontSize: '0.875rem',
                        borderRadius: '0.5rem',
                        backgroundColor: '#374151',
                        borderColor: '#374151',
                        color: '#fff'
                      }),
                   
                    control: (provided) => ({
                        ...provided,
                        padding: '0px',
                        fontSize: '0.875rem',
                        borderRadius: '0.5rem',
                        backgroundColor: '#374151',
                        borderColor: '#374151',
                        color: '#fff'
                      }),
                      placeholder: (provided) => ({
                        ...provided,
                        color: '#9ca0a6'
                      }),
                    option: (provided, state) => ({
                        ...provided,
                        padding: '5px 8px 5px 8px',
                        fontSize: '0.875rem',
                        borderRadius: '0.5rem',
                        backgroundColor: state.isSelected ? '#767676' : state.isFocused ? '#767676' : 'transparent',
                        '&:hover': {
                          backgroundColor: '#767676', // Background color on hover
                        },
                        borderColor: '#374151',
                        color: '#fff'
                      }),
                      multiValue: (provided) => ({
                        ...provided,
                        backgroundColor: '#1f2937', // Set background color to red
                        borderRadius: '0.375rem', // Adjust border radius as needed
                      }),
                      multiValueLabel: (provided) => ({
                        ...provided,
                        color: '#fff', // Set text color to white
                      }),
                      multiValueRemove: (provided) => ({
                        ...provided,
                        color: '#fff', // Set close icon color to white
                        '&:hover': {
                          color: '#e53e3e', 
                          background: '#1f2937'
                        },
                      }),
                  }}
                  menuPortalTarget={document.body}
            />

            {props.errors?.map((error, i) => <p className='text-red-600 text-xs font-semibold'><FaCircleExclamation className="inline ml-2 mr-1" key={`error-${props.id}-${i}`}/>{error}
            </p>)}
        </div>
    </div>;
};

export default MultiSelectInput;
