import * as React from 'react';
import Cart from '../../Models/Cart';
import useHamroFashionClient from '../../Hooks/useHamroFashionClient';
import { Link, useNavigate, useParams } from 'react-router';
import ApiException from '../../api/ApiException';

interface CartDetailPageState {
    errors?: ApiException | Error | string | any;
    cart: Cart;
    isLoading: boolean;
}
const CartDetailPage: React.FC = () => {
    const [state, setState] = React.useState({
        cart: {} as Cart,
        errors: '',
        isLoading: true
    }as CartDetailPageState);
    const api = useHamroFashionClient();
    const redirect = useNavigate();
    const { cartId } = useParams();
    const userId = api.getUserId();
    React.useEffect(() => {
        getCart();
    }, [api, cartId])

    const getCart = React.useCallback(async() => {
        try {
            console.log('hello');
            const cart: Cart = await api.get(`user/cart`)
            console.log(cart);
            setState({
                ...state,
                cart,
                isLoading: false
            });
            console.log(cart);
        } catch (err: ApiException | Error | string | any) {
            setState({
                ...state,
                errors: err
            })
            console.log(err);
        }
    }, [api, state.cart]);

    const removeItem = React.useCallback(async(productId:string) => {
        try {
            await api.post(`/user/cart/${productId}/remove`);
            await getCart();
        } catch (err: ApiException | Error | string | any) {
            console.log(err);
        }
    }, [])
    const addToCart = React.useCallback(async(productId:string) => {
        try {
            await api.post(`/user/cart/${productId}/add`);
            console.log('calling card');
            await getCart();
        } catch (err: ApiException | Error | string | any) {
            console.log(err);
        }
    }, [])
    const decreaseItem = React.useCallback(async(productId: string) => {
        try {
            await api.post(`/user/cart/${productId}/decreaseitem`);
            await getCart();
        } catch (err: ApiException | Error | string | any) {
            console.log(err);
        }
    }, [])

    // const getTotalCost = React.useCallback(async() => {
    //     try {
    //         await api.get(`/user/${cartId}/totalcost`);
    //     } catch (err: ApiException | Error | string | any) {
    //         console.log(err);
    //     }
    // }, [])
  return <div>
    {state.isLoading && <div>Loading...</div>}
    {!state.isLoading && <div>
        <table className="table-auto">
            <thead className='bg-gray-800 text-white'>
                <tr>
                <th className='px-10 py-3'>Name</th>
                <th className='px-10 py-3'>Price</th>
                <th className='px-10 py-3'>Amount</th>
                <th className='px-10 py-3'>Remove</th>
                </tr>
            </thead>
            <tbody className='bg-gray-600 text-white'>
                {state.cart.cartItems && state.cart.cartItems.map((item) => 
                <tr>
                <td className='px-10 py-3'>{item.product.name}</td>
                <td className='px-10 py-3'>{item.product.price}</td>
                <td className='px-10 py-3'>
                    <div className="flex space-x-4">
                        <div onClick={() => addToCart(item.productId)}><i className="fa-solid fa-plus hover:text-red-500"></i></div>
                        <div>{item.quantity}</div>
                        <div onClick={() => decreaseItem(item.productId)}><i className="fa-solid fa-minus hover:text-red-500"></i></div>
                    </div>
                </td>
                <td className='px-10 py-3'>
                    <div onClick={() => removeItem(item.productId)} className='hover:text-red-500'>Remove</div>
                </td>
                </tr>)}
            </tbody>
        </table>
        <div>
            Total Cost: {state.cart.totalPrice}
        </div>
        <Link to={`/checkout/${cartId}/`} className='underline'>CheckOut</Link>
    </div>}
    </div>
};
export default CartDetailPage;