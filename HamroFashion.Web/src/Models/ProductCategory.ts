import BaseModel from "./BaseModel"
import Product from "./Product"
import Tag from "./Tag"

export default interface ProductCategory extends BaseModel {
    product: Product;
    productId: string;
    tag: Tag;
    tagId: string;
}