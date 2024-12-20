import moment from "moment";
import AuthenticationSuccessful from "./Events/AuthenticationSuccessful";
import ApiException from "./ApiException";
import User from "../Models/User";
import { RoleModel } from "../Models/RoleModel";

/**
 * HamroFashionClient is a helper to communicate with the hamrofashion api server
 */
export default class HamroFashionClient {
    constructor(private readonly rootUrl: string) {
        if (!rootUrl.endsWith('/')) {
            this.rootUrl = `${rootUrl}/`;
            
        }
    }

   /**
    * Clears our user session and local storage
    */ 
    public clearSession() {
        window.sessionStorage.removeItem('bearerToken');
        window.sessionStorage.removeItem('currentUser');
        window.sessionStorage.removeItem('tokenExpires');

        window.localStorage.removeItem('refreshToken');
        window.localStorage.removeItem('userId');
    }

    /**
     * Sets our users session and local storage values from the
     * provided hamroFashionResult
     * 
     * @param hamroFashionResult The UserhamroFashionResult containing bearer, refresh and UserAccount
     */
    public setSession(hamroFashionResult: AuthenticationSuccessful) {
        window.sessionStorage.bearerToken = hamroFashionResult.bearerToken;
        window.sessionStorage.currentUser = JSON.stringify(hamroFashionResult.user);
        //window.sessionStorage.currentUser = "";

        window.localStorage.refreshToken = hamroFashionResult.refreshToken;
        window.localStorage.userId = hamroFashionResult.user.id;
    }

    /**
     * Helper method to call an HTTP GET
     * 
     * @param path The path to GET (such as /games, or /users, etc)
     * @param query Optional object that gets serialized to query string parameters
     * @returns The response payload
     */
   
    public async get(path: string, query?: any) : Promise<any> {
        const qs = this.serializeQs(query);
        const url = path + qs;
        return this.fetch('GET', url);
    }

    /**
     * Underlying fetch call, customized to work with our api
     * 
     * @param method The HTTP verb / method
     * @param path The path to call
     * @param payload The payload to serialize (as json) to the body
     * @returns The response payload
     */
    public async fetch(method: string, path: string, payload?: any): Promise<any> {
        const url = this.buildUrl(path);
        const body = payload? JSON.stringify(payload) : null;

        const req: RequestInit = {
            method,
            redirect: 'follow',
            headers: { "Content-Type": "application/json" },
            body: body ?? undefined
        };

        if(this.isLoggedIn()) {

            if (
                moment(window.sessionStorage.tokenExpires) <= moment(new Date()) && !path.endsWith('/hamrofashion/refresh')
            ) {
                //this.refreshToken();
            }

            if (req && req.headers)
                req.headers = {
                    ...req.headers,
                    'Authorization': `Bearer ${window.sessionStorage.bearerToken}`
                };
        }

        const res = await fetch(url, req);

        if (res.redirected === true) {
            window.location.href = res.url;
            return undefined;
        }

        const data = await res.text();
        const responseType = res.headers.get('Content-Type');

        if(res.status >= 200 && res.status <= 299) {
            if (responseType && responseType.indexOf('application/json') >= 0) {
                return data ? await JSON.parse(data) : null;
            } else {
                return data;
            }
        }

        if (res.status === 400) {
            const err = data? await JSON.parse(data) : undefined;

            throw {
                fieldErrors: err?.errors,
                message: err?.detail,
                statusCode: res.status ?? 500
            } as ApiException;
        }
    }

    /**
     * Simple means of determining if this HamroFashionClient is currently "logged in"
     * 
     * @returns boolean representing if the HamroFashionClient is signed in
     */
    public isLoggedIn(): boolean {
        if (window.sessionStorage.bearerToken)
            return true;
        return false;
    }

    public getUserId() {
        var userString = window.sessionStorage.currentUser;
        if (userString) {
            var user = JSON.parse(userString);
            return user.id;
        } else {
            // Handle the case where currentUser is not set
            return null;
        }
    }
    public getUserRoles() {
        const currentUserString = window.sessionStorage.getItem('currentUser');
        if (!currentUserString) {
            console.log("not logged in")
            return null;
        }
        try {
            const user: User = JSON.parse(currentUserString);
            const userRoles: RoleModel[] = user.userRoles;
            return userRoles; 
        } catch (e) {
            console.error("Failed to parse currentUser from sessionStorage", e);
            return null; // or handle error as appropriate
        }
    }
    public IsMod() {
        const currentUserString = window.sessionStorage.getItem('currentUser');
        if (!currentUserString) {
            console.log("not logged in")
            return null;
        }
        try {
            const user: User = JSON.parse(currentUserString);
            const userRoles: RoleModel[] = user.userRoles;
            
            const isMod = userRoles.some(role => role.name === "Mod");
            return isMod; 
        } catch (e) {
            console.error("Failed to parse currentUser from sessionStorage", e);
            return null; // or handle error as appropriate
        }
    }
    

    /**
     * Helper method to call an HTTP POST
     * 
     * @param path The path to POST
     * @param payload Optional object that gets serialized to json in the request body
     * @returns The response payload
     */
    public async post(path: string, payload?: object): Promise<any> {
        return this.fetch('POST', path, payload);
    }

    /**
     * Helper method to call an HTTP PUT
     * 
     * @param path The path to POST
     * @param payload Optional object that gets serialized to json in the request body
     * @returns The response payload
     */
    public async put(path: string, payload?: object): Promise<any> {
        return this.fetch('PUT', path, payload);
    }

     /**
     * Sign out helper, will post to API asking to invalidate the current
     * refresh token, then removes bearer and refresh tokens from session storage
     */
    public async signOut() {
        try {
            if (window.localStorage.refreshToken) {
                await this.post(`/hamrofashion/signout/${encodeURIComponent(window.localStorage.refreshToken)}`);
            }
        } finally {
            this.clearSession();
        }
    }

    /**
     * Sign out helper, will post to API asking to invalidate all refresh tokens, 
     * then removes bearer and refresh tokens from session storage
     */
    public async signOutAll() {
        try {
            await this.post('/hamrofashion/signout/all');
        } finally {
            this.clearSession();
        }
    }

    /**
     * Helper to build the full url we will call
     * 
     * @param path The path we are trying to call
     * @returns The full url to call
     */
    protected buildUrl(path: string): string {
        return this.rootUrl + (path.startsWith('/') ? path.substring(1) : path);
    }
    
    /**
     * Helper to serialize the provided query object into a proper http
     * query string (encoded and all)
     * 
     * @param query The query object to serialize
     * @returns query string (with leading ?)
     */
    protected serializeQs(query: any): string {
        if (!query) return '';

        const q: { [key: string]: any } = query;

        return '?' + Object.keys(q)
            .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(q[key])}`)
            .join('&');
    }
}