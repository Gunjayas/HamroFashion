import BaseModel from "./BaseModel";
import { RoleModel } from "./RoleModel";

export default interface User extends BaseModel {
    profilePicUrl?: string;
    userName?: string;
    emailAddress: string;
    userRoles: RoleModel[];
}