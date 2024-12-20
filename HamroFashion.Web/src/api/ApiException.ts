/**
 * Represents an error being reported from the Mudmunity API
 */
export default interface ApiException {
    /**
     * The individual field errors encountered
     */
    fieldErrors: Map<string, string>;

    /**
     * The generic error message
     */
    message: string;

    /**
     * The HTTP status code if applicable
     */
    statusCode: number | unknown;
}