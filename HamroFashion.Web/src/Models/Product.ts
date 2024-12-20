import BaseModel from "./BaseModel";
import Tag from "./Tag";
import User from "./User";

export default interface Product extends BaseModel {
    createdBy: User;
    name: string;
    description?: string;
    color: string;
    imageUrls: string[];
    size: string;
    quantity: number;
    availability: boolean;
    category: Tag;
    collection: Tag;
    label: Tag;
}

