import BaseModel from "./BaseModel";
import Order from "./Order";
import Product from "./Product";

export default interface OrderItemModel extends BaseModel {
    order: Order;
    orderId: string;
    product: Product;
    productId: string;
    quantity: string;
}