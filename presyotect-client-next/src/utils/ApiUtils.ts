import { useAuthStore } from "@features/stores.ts";

export function getAxiosConfig(){

    const authStore = useAuthStore();
    const apiHost = import.meta.env.VITE_API_HOST;

    if(authStore.token){
        return {
            baseURL: apiHost,
            headers: {
                Authorization: `Bearer ${authStore.token}`
            }
        }
    }
    return {
        baseURL: apiHost
    }
}