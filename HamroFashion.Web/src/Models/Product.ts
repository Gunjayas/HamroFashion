import BaseModel from "./BaseModel";
import Tag from "./Tag";
import User from "./User";

export default interface Product extends BaseModel {
    createdBy?: User | null;             // Allow null or undefined
    name: string;
    description?: string;
    color: string;
    imageUrl: string;
    size: string;
    quantity: number;
    availability: boolean;
    category: Tag;        // Optional and nullable
    collection: Tag[];   // Optional and nullable
    label: Tag[];        // Optional and nullable
    price: number;
}


