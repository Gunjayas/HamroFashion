import BaseModel from "./BaseModel";
import CartItem from "./CartItem";
import User from "./User";

export default interface Cart extends BaseModel {
    userId: string;
    user: User;
    cartItems: CartItem[];
    totalPrice: number;  
}