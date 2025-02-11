import {useAuthStore} from "@stores/auth-store.ts";
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios from "axios";
import type {MonitoredEstablishment, Product} from "@/types/Interfaces.ts";

export class MonitoringService {

    static async getAssignedEstablishmentsCount(): Promise<number> {
        const monitoringStore = useMonitoringStore();
        const isOnline = await monitoringStore.isOnline;
        if (!isOnline) {
            return monitoringStore.assignedEstablishments?.length || 0;
        }

        const axiosConfig = getAxiosConfig();
        const authStore = useAuthStore();

        const userId = authStore.userClaims?.nameid;
        const response = await axios.get(`/monitoring/assigned-establishments/count?personnelId=${userId}`, axiosConfig);

        return response.data.content;
    }

    static async getAssignedEstablishments(): Promise<MonitoredEstablishment[] | undefined> {

        const monitoringStore = useMonitoringStore();
        const isOnline = await monitoringStore.isOnline;
        if (!isOnline) {
            return monitoringStore.assignedEstablishments || undefined;
        }

        const axiosConfig = getAxiosConfig();
        const authStore = useAuthStore();

        try {
            const userId = authStore.userClaims?.nameid;
            const response = await axios.get(`/monitoring/assigned-establishments?personnelId=${userId}`, axiosConfig);

            monitoringStore.assignedEstablishments = response.data.content;
            return response.data.content;
        } catch {
            return monitoringStore.assignedEstablishments || undefined;
        }
    }

    static async getMonitoredProductsCount(): Promise<number> {
        const monitoringStore = useMonitoringStore();
        const isOnline = await monitoringStore.isOnline;
        if (!isOnline) {
            return monitoringStore.products?.length || 0;
        }

        const axiosConfig = getAxiosConfig();
        const authStore = useAuthStore();

        const userId = authStore.userClaims?.nameid;
        const response = await axios.get(`/monitoring/assigned-establishments/count?personnelId=${userId}`, axiosConfig);

        return response.data.content;
    }

    static async getMonitoredProducts(): Promise<Product[] | undefined> {

        const monitoringStore = useMonitoringStore();
        const isOnline = await monitoringStore.isOnline;
        if (!isOnline) {
            return monitoringStore.products || undefined;
        }

        const axiosConfig = getAxiosConfig();
        const authStore = useAuthStore();

        try {
            const userId = authStore.userClaims?.nameid;
            const response = await axios.get(`/monitoring/all-monitored-products?personnelId=${userId}`, axiosConfig);

            monitoringStore.products = response.data.content;
            return response.data.content;
        } catch {
            return monitoringStore.products || undefined;
        }
    }
}