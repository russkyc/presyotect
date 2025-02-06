import axios, {AxiosError} from 'axios';
import {useAuthStore} from "@features/auth/auth-store.ts";
import type {AuthState} from "@/types/Interfaces.ts";

const axiosConfig = {baseURL: import.meta.env.VITE_API_HOST};

export async function login(username: string, password: string): Promise<AuthState> {

    const authStore = useAuthStore();
    
    try {
        const result = await axios.post('/authentication/login', {username, password}, axiosConfig);
        authStore.token = result.data.data;
        return {isAuthenticated: true, data: authStore.userClaims};
    } catch (error) {
        if (error instanceof AxiosError) {
            return {isAuthenticated: false, data: error.response?.data.errors["Account"]};
        }
        return {isAuthenticated: false, data: "An error occurred"};
    }
}

export async function logout(): Promise<void> {
    const authStore = useAuthStore();
    authStore.token = null;
}