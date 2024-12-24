import * as React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import MultiSelectInput, { MultiSelectOption } from '../common/MultiSelectInput';
import { CreateProductFormReducer, newProductFormState } from './CreateProductFormState';
import useHamroFashionClient from '../../Hooks/useHamroFashionClient';
import Product from '../../Models/Product';
import Tag from '../../Models/Tag';
import TextInput from '../common/TextInput';
import SelectInput from '../common/SelectInput';
import { ImageInput } from '../common/FileInput';

const ProductForm: React.FC = () => {
    const { productId } = useParams();
    const api = useHamroFashionClient();
    const redirect = useNavigate();
    
    const [collections, setCollections] = React.useState<MultiSelectOption[]>([]);
    const [selectedCollections, setSelectedCollections] = React.useState<MultiSelectOption[]>([]);

    const [labels, setLabels] = React.useState<MultiSelectOption[]>([]);
    const [selectedLabels, setSelectedLabels] = React.useState<MultiSelectOption[]>([]);

    const [isLoading, setLoading] = React.useState(true);

    const isNew = React.useMemo(() => {
        return (productId?.toLowerCase() === 'new');
    }, [productId]);

    const [state, dispatch] = React.useReducer(CreateProductFormReducer, newProductFormState());

    const errors = state.errors ?? {};
    
   
    React.useEffect(() => {
        (async () => {
            try {
                const collection = await api.get('/admin/tag/Collection');
                console.log(collection)
                const options1: MultiSelectOption[] = collection.map((collection: any) => ({
                    value: collection.id,
                    label: collection.name
                }));
                setCollections(options1);
                
                const label = await api.get('/admin/tag/Label');
                console.log(label)
                const options2: MultiSelectOption[] = label.map((label: any) => ({
                    value: label.id,
                    label: label.name
                }));
                setLabels(options2);
            }   
            catch (err: any) {
                dispatch({type: 'SET_ERRORS', errors: err.fieldErrors})
            }
            
            if (isNew){
                setLoading(false);
                return;
            }

            try {
                const product: Product = await api.get(`products/${productId}`);
                
                // Iterate over each property in the product object
                for (const [fieldName, fieldValue] of Object.entries(product)) {
                    // Dispatch SET_COMMAND_FIELD action to update each field in state.command
                    dispatch({ type: 'SET_COMMAND_FIELD', name: fieldName, value: fieldValue });
                }
                const selectedoptions1: MultiSelectOption[] = product.collection.map((collection: Tag) => ({
                    value: collection.id,
                    label: collection.name
                }))
                setSelectedCollections(selectedoptions1);

                const selectedoptions2: MultiSelectOption[] = product.label.map((label: Tag) => ({
                    value: label.id,
                    label: label.name
                }))
                setSelectedLabels(selectedoptions2);

                setLoading(false);
                
            } catch (err: any) {
                dispatch({type: 'SET_ERRORS', errors: err.fieldErrors})
            }
            
        })();
    }, [api, isNew, productId]);

    // Handles command form field changes
    const handleCommandChange = React.useCallback((e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) =>
        {dispatch({ type: 'SET_COMMAND_FIELD', name: e.currentTarget.id, value: e.currentTarget.value });
        console.log(state.command)},
        []
    );
    const handleMulti = React.useCallback((name: string, value: string[]) => 
        dispatch({ type: 'SET_MULTISELECT', name: name, value: value }), []
    );
 
    const handleImageChange = React.useCallback((name: string, image: string | ArrayBuffer | null) =>
        dispatch({ type: 'SET_IMAGE', name: name,  image: image}), []
    )
    function isValidUrl(url: string) {
        const urlRegex = /^(https?:\/\/)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*\/?$/;
        return urlRegex.test(url);
      }

    const handleSubmit = React.useCallback(async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        dispatch({ type: 'CLEAR_ERRORS' });
        let isValid = true;

        if (state.command.name.length < 2) {
            dispatch({ type: 'SET_ERROR', name: 'name', value: 'no way the product name is only 1 word!' });
            isValid = false;
        }

        if (isValid) {
            try {
                if(isNew){
                    const res: Product = await api.post('/products/create', state.command);
                    console.log(res);
                    redirect(`/product/${res.id}/detail`);
                }
                else{
                    await api.put('/products/update', state.command);
                    redirect(`/product/${productId}/detail`);
                }

            } catch (err: any) {
                if (err.statusCode === 400 || err.message.statusCode === 400) 
                    dispatch({ type: 'SET_ERRORS', errors: err.fieldErrors });
            }
        }
        
    }, [api, state, redirect]);

return <div>
    {isLoading && <div>Loading...</div>}
    {<div className='bg-gray-800'>
        <form onSubmit={handleSubmit} className="py-8 mx-auto border-green-600 2xl:max-w-xl lg:max-w-lg sm:max-w-md xs:max-w-sm">
            <div className="relative z-0 w-full py-4 mb-5 bg-green-600 group rounded-t-xl">
                <h1 className='text-2xl font-bold ps-6'>Create Product</h1>
            </div>
            <div className='text-white text-md'>
                Use this form to {isNew? 'create' : 'update'} the product
            </div>
            <hr className='my-4'/>
            <div className="relative z-0 w-full mb-5 group">
                <TextInput 
                    label='Product name' 
                    id='name' 
                    type='text' 
                    value={state.command.name} 
                    onChange={handleCommandChange}
                    errors={errors['name']}
                />
            </div>

            <div className="relative z-0 w-full mb-5 group">
                <TextInput 
                    label='Price' 
                    id='price' 
                    type='text' 
                    value={state.command.price} 
                    onChange={handleCommandChange}
                    errors={errors['price']}
                />
            </div>

            <div className="relative z-0 w-full mb-5 group">
                <TextInput 
                    label='Quantity' 
                    id='quantity' 
                    type='text' 
                    value={state.command.quantity} 
                    onChange={handleCommandChange}
                    errors={errors['quantity']}
                />
            </div>
         
            <div className="relative z-0 w-full mb-5 group">
                <ImageInput 
                    label='Cover Image' 
                    id='cover'  
                    onChange={handleImageChange}
                    errors={errors['coverImage']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
             
                <MultiSelectInput 
                    label="Collection" 
                    id='productCollection' 
                    onChange={handleMulti} 
                    options={collections}
                    errors={errors['productCollection']}
                    selectedValues={selectedCollections}
                />
        
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <MultiSelectInput 
                    label="Label" 
                    id='productLabel' 
                    onChange={handleMulti} 
                    options={labels}
                    errors={errors['productLabel']}
                    selectedValues={selectedLabels}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <SelectInput 
                    label='Category' 
                    id='productCategory'  
                    value={state.command.productCategory} 
                    onChange={handleCommandChange}
                    errors={errors['productCategory']}
                    options={['Lehenga', 'Kurtha', 'Saree']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <SelectInput 
                    label='Size' 
                    id='size'  
                    value={state.command.size} 
                    onChange={handleCommandChange}
                    errors={errors['size']}
                    options={['S', 'M', 'L', 'XL']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">
                <SelectInput 
                    label='Color' 
                    id='color'  
                    value={state.command.color} 
                    onChange={handleCommandChange}
                    errors={errors['color']}
                    options={['Red', 'Blue', 'Green', 'Purple']}
                />
            </div>
            <div className="relative z-0 w-full mb-5 group">   
                <label htmlFor="message" className="block mb-2 text-sm font-medium text-gray-400">Description</label>
                <textarea rows={4} className="block p-2.5 w-full text-sm bg-gray-700 rounded-lg border border-gray-600 placeholder-gray-400 text-gray-400" placeholder="Description of the product..." id='description'  
                    value={state.command.description} 
                    onChange={handleCommandChange}
                    ></textarea>
                <small>{errors['description']}</small>
            </div>
            <button type="submit" className="text-white hover:bg-green-800 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center bg-green-600 focus:ring-green-800">{isNew? 'Create': 'Update'} Product</button>
        </form>
    </div>}
    </div>

}
export default ProductForm;


