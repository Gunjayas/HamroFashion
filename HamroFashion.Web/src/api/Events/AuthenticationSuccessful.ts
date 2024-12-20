import User from "../../Models/User";

export default interface AuthenticationSuccessful {
    bearerToken: string;
    bearerTokenExpires: Date;
    refreshToken: string;
    refreshTokenExpires: Date;
    user: User;
}