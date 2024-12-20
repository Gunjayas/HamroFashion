import BaseModel from "./BaseModel";

export interface RoleModel extends BaseModel {
    description: string;
    name: string;
}