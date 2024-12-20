/**
 * Represents environment specific application settings
 */
export interface AppSettings {
    baseUrl: string;

    userId: string;
}

const useAppSettings = (): AppSettings => {
    return {
        baseUrl: 'http://localhost:5000',
        userId: 'c7878adf-244d-40b3-9ee4-3750da0f7c56'
    }
}

export default useAppSettings;