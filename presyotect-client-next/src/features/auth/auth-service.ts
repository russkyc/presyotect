import axios, {AxiosError} from 'axios';
import {useAuthStore} from "@features/auth/auth-store.ts";
import type {AuthState} from "@/types/Interfaces.ts";

const axiosConfig = {baseURL: import.meta.env.VITE_API_HOST};

export async function login(username: string, password: string): Promise<AuthState> {

    const authStore = useAuthStore();
    
    try {
        console.log(axiosConfig.baseURL);
        const result = await axios.post('/auth/login', {username, password}, axiosConfig);
        if (result.data.success === false){
            return {isAuthenticated: false, data: result.data.errors[0]};
        }
        authStore.token = result.data.content;
        return {isAuthenticated: true, data: authStore.userClaims};
    } catch (error) {
        if (error instanceof AxiosError) {
            return {isAuthenticated: false, data: "Server cannot be reached."};
        }
        return {isAuthenticated: false, data: "Unknown error has occured."};
    }
}

export async function logout(): Promise<void> {
    const authStore = useAuthStore();
    authStore.token = null;
}