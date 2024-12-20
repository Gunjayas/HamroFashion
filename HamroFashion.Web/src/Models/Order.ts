import BaseModel from "./BaseModel";
import OrderItemModel from "./OrderItem";
import User from "./User";

export default interface Order extends BaseModel {
    user: User;
    userId: string;
    orderItems: OrderItemModel[];
    totalPrice: number;
    shippingAddress: string;
    paymentMethod: string;
    status: string;
}