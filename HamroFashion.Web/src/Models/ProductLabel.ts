import BaseModel from "./BaseModel"
import Product from "./Product"
import Tag from "./Tag"

export default interface ProductLabel extends BaseModel {
    product: Product;
    productId: string;
    tag: Tag;
    tagId: string;
}