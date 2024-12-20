import HamroFashionClient from "../api/HamroFashionClient";
import useAppSettings from "./useAppSettings";


/**
 * We use a singleton pattern, there is one HamroFashionClient that this
 * entire app will use.
 */
let hamroFashionClient: HamroFashionClient | undefined = undefined;
/**
 * Hook that allows us to request a reference to this applications HamroFashionClient
 * @returns HamroFashionClient
 */
const useHamroFashionClient = () : HamroFashionClient => {
    const appSettings = useAppSettings();
    
    if (!hamroFashionClient)
        hamroFashionClient = new HamroFashionClient(appSettings.baseUrl);

    return hamroFashionClient;
}

export default useHamroFashionClient;