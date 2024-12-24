import BaseModel from "./BaseModel";
import Cart from "./Cart";
import Product from "./Product";

export default interface CartItem extends BaseModel {
    cart: Cart;
    cartId: string;
    product: Product;
    productId: string;
    quantity: string;
    price: number
}