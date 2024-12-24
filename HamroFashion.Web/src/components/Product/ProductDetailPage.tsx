import React from "react";
import Product from "../../Models/Product";
import useHamroFashionClient from "../../Hooks/useHamroFashionClient";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import ApiException from "../../api/ApiException";

interface ProductDetailPageState {
    errors?: ApiException | Error | string | any;
    product: Product;
    isLoading: boolean;
}
const ProductDetailPage: React.FC = () => {
    const[state, setState] = React.useState({
        product: {}as Product,
        errors: '',
        isLoading: true
    }as ProductDetailPageState);

    const api = useHamroFashionClient();
    const { productId } = useParams();
    console.log(productId);
    const userId = api.getUserId(); 
    const location = useLocation();
    const redirect = useNavigate();
    React.useEffect(() => {
        getProduct();
    }, [api, productId]);
    

    const getProduct = React.useCallback(async () => {
        try {
            const product: Product = await api.get(`/products/${productId}`);
           console.log(product);
            setState({
                ...state,
                product,
                isLoading: false
            });
            console.log(state);
        } catch (err: ApiException | Error | string | any) {
            setState({
                ...state,
                errors: err
            })
            console.log(err);
        }
    
    }, [api, productId, state.product]);

    const handleClick = () => {
        redirect(`/user/${state.product.createdById}/detail`)
    }

    const addToCart = React.useCallback(async() => {
        console.log(state.product)
        try {
            await api.post(`/user/cart/${productId}/add`);
        } catch (err: ApiException | Error | string | any) {
            console.log(err);
        }
    }, [])

return <div>
    {state.isLoading && <div>Loading...</div>}
    {!state.isLoading && <div>
            <div className='px-12 py-16 text-white bg-gray-800'>
                <div className='grid max-w-5xl gap-10 mx-auto lg:grid-cols-7'>
                    <div className='lg:col-span-3 xs:col-span-1'>
                        <img src={state.product.imageUrl} alt="" className='object-cover w-full h-full'/>
                    </div>
                    <div className='flex flex-col gap-3 py-0 lg:col-span-4 xs:col-span-1'>
                        <div className='font-semibold text-green-500 xs:text-3xl sm:text-5xl'>
                            <h1>{state.product.name}</h1>
                        </div>
                        <div className='text-xl font-medium'>
                            <small>Added on <span className='text-gray-500'>- {new Date(state.product.createdOn).getFullYear()}-{(new Date(state.product.createdOn).getMonth() + 1).toString().padStart(2, '0')}-{new Date(state.product.createdOn).getDate().toString().padStart(2, '0')}</span></small>
                        </div>
                        <div className='flex flex-wrap items-center gap-3 font-semibold xs:text-xl sm:text-2xl'>
                            <h2>Category :</h2>
                            <p className='flex-shrink-0 text-xl text-green-600 underline'>{state.product.category?.name}</p>
                        </div>
                        <div className='flex flex-wrap items-center gap-3 font-semibold xs:text-xl sm:text-2xl'>
                            <h2>Label :</h2>
                            {state.product?.label && state.product?.label.map((label) => 
                                <p className='flex-shrink-0 text-xl text-green-600 underline'>{label.name}</p>
                            )}
                        </div>
                        <div className='flex flex-wrap items-center gap-3 font-semibold xs:text-xl sm:text-2xl'>
                            <h2>Collection :</h2>
                            {state.product?.collection && state.product?.collection.map((collection) => 
                                <p className='flex-shrink-0 text-xl text-green-600 underline'>{collection.name}</p>
                            )}
                        </div>
                        <div>
                            Price:
                            {state.product.price}
                        </div>
                        <div>
                            Color:
                            {state.product.color}
                        </div>
                        <div>
                            Availability:
                            {state.product.availability? 'On Stock': 'Out of Stock'}
                        </div>
                        <div>
                            Size:
                            {state.product.size}
                        </div>
                        <div>
                            Available quantity:
                            {state.product.quantity}
                        </div>
                        <div className="border border-red-500 border-solid" onClick={() => addToCart()}>
                            Add To Cart
                        </div>
                    </div>
                </div>
            </div>

            <div className='py-16 text-white bg-gray-900'>
                <div className='mx-auto max-w-5xl font-medium xs:text-[1rem] sm:text-[1.08rem] py-8 px-6 break-words text-ellipsis overflow-hidden'>
                    {state.product.description}
                </div>
            </div>

            {/* <ProductCommentSection onCommentsRendered={() => setCommentsLoaded(true)} highlightedCommentId={highlightedCommentId}/> */}
        </div>}
</div>
        

}
export default ProductDetailPage;