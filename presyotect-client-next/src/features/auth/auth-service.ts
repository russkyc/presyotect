import {useAuthStore} from "@features/stores.ts";
import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios, {AxiosError} from "axios";
import type {AuthState} from "@/types/Interfaces.ts";

export async function login(username: string, password: string): Promise<AuthState> {

    const authStore = useAuthStore();
    const axiosConfig = getAxiosConfig();

    try {
        const result = await axios.post("/auth/login", {username, password}, axiosConfig);
        if (result.data.success === false) {
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