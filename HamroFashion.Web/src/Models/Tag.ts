import BaseModel from "./BaseModel"

export default interface Tag extends BaseModel {
    description: string;
    name: string;
    tagType: string;
}