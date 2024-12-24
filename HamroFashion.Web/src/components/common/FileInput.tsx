import * as React from 'react';
import { FaCircleExclamation } from 'react-icons/fa6';

/**
 * Custom properties for our FileInput control
 */
interface FileInputProps {
    /**
     * MIME types to restrict the user to (or undefined for no restrictions)
     */
    accept?: string;

    id: string; 

    /**
     * Error message to display for this text box
     */
    errors: string[];

    /**
     * Label to display for this text box
     */
    label: string;

    /**
     * The callback to invoke when the user selects a different file
     * 
     * @param image The base 64 value of the file selected
     * @param filename The filename that was selected (without path)
     */
    onChange: (name: string, image: string | ArrayBuffer | null) => void;

    /**
     * Disable the textbox?
     */
    disabled?: boolean;

}

/**
 * Renders a file input
 * 
 * @param props TextInputProps
 * @returns TextInput
 */
const FileInput: React.FC<FileInputProps> = (props: FileInputProps) => {
    /**
     * Calls the props.onChange when the user changes the value of the textbox,
     * parent should change the props.value to update the screen
     */
    const handleChange = React.useCallback((e: any) => {
        const file = e.target.files[0] as File;

        // This reader is how we load the file the user selected
        const reader = new FileReader();
        reader.onload = () => {
            props.onChange(
                props.id,
                reader.result
            );
        };

        // Read the file and call reader.onload when done
        reader.readAsDataURL(file);
        
    }, [props]);

    return <div className="mb-3">
        <div>
            <label className="block mb-2 text-sm font-medium text-gray-400" htmlFor="user_avatar">{props.label}</label>

            <input
                type="file"
                accept={props.accept}
                className="block w-full text-sm border rounded-lg cursor-pointer text-gray-400 focus:outline-none bg-gray-700 border-gray-600 placeholder-gray-400"
                id={props.id}
                onChange={handleChange}
                /*value={props.value}*/
            />
            {props.errors?.map((error, i) => <p className='text-red-600 text-xs font-semibold'><FaCircleExclamation className="inline ml-2 mr-1" key={`error-${props.id}-${i}`}/>{error}
            </p>)}
        </div>
    </div>;
};

export default FileInput;

/**
 * Renders a FileInput that accepts basic image file formats
 * 
 * @param props FileInputProps
 * @returns FileInput
 */
export const ImageInput: React.FC<FileInputProps> = (props: FileInputProps) => {
    const fileProps = {
        ...props,
        accept: 'image/jpeg,image/gif,image/png,application/pdf,image/x-eps'
    };

    return FileInput(fileProps);
};
