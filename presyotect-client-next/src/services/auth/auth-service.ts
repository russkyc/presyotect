import {useActionsStore} from "@stores/actions-store.ts";
import {useAuthStore} from "@stores/auth-store.ts";
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios, {AxiosError} from "axios";
import router from "@/router.ts";
import type {AuthState} from "@/types/Interfaces.ts";

export async function login(username: string, password: string): Promise<AuthState> {

    const authStore = useAuthStore();
    const axiosConfig = getAxiosConfig();

    try {
        const result = await axios.post("/auth/login", {username, password}, axiosConfig);
        if (result.data.success === false) {
            return {isAuthenticated: false, data: result.data.errors[0]};
        }
        authStore.token = result.data.content; // Store only JWT token
        return {isAuthenticated: true, data: authStore.userClaims};
    } catch (error) {
        if (error instanceof AxiosError) {
            return {isAuthenticated: false, data: "Server cannot be reached."};
        }
        return {isAuthenticated: false, data: "Unknown error has occured."};
    }
}

export async function refreshToken(): Promise<void> {
    
    const authStore = useAuthStore();
    const axiosConfig = getAxiosConfig();

    try {
        const result = await axios.post("/auth/refresh-token", {token: authStore.token}, axiosConfig);
        authStore.token = result.data.content;
    } catch {
        await logout();
    }
}

export async function logout(): Promise<void> {
    const authStore = useAuthStore();
    const actionsStore = useActionsStore();
    const monitoringStore = useMonitoringStore();

    monitoringStore.clearAll();
    actionsStore.clearPendingActions();
    authStore.token = null;
    await router.push({name: "login"});
}