import {getAxiosConfig} from "@utils/ApiUtils.ts";
import {useIntervalFn} from "@vueuse/core";
import axios from "axios";
import {defineStore} from "pinia";
import {computed, ref} from "vue";
import type {MonitoredEstablishment, MonitoredPrice, Product} from "@/types/Interfaces.ts";

export const useMonitoringStore = defineStore(
    "monitoringStore",
    () => {
        const activeEstablishment = ref<MonitoredEstablishment|null>();
        const assignedEstablishments = ref<MonitoredEstablishment[]|null>();
        const products = ref<Product[]|null>();
        const pendingMonitoredPrices = ref<MonitoredPrice[]>([]);
        const isOnline = computed(async () => {
            try {
                const apiHost = import.meta.env.VITE_API_HOST;
                await axios.get("/check",{
                    baseURL: apiHost,
                    timeout: 200
                })
                return true;
            } catch {
                return false;
            }
        });

        const addPendingMonitoredPrice = (monitoredPrice: MonitoredPrice) => {
            pendingMonitoredPrices.value.push(monitoredPrice);
        };

        const syncPendingMonitoredPrices = async () => {
            if (await isOnline.value) {
                for (const price of pendingMonitoredPrices.value) {
                    try {
                        await axios.post("/monitoring/monitored-price", price, getAxiosConfig());
                        pendingMonitoredPrices.value = pendingMonitoredPrices.value.filter(p => p !== price);
                    } catch (error) {
                        console.error("Failed to sync price:", price, error);
                    }
                }
                localDataUploaded();
            }
        };
        
        const localDataUploaded = () => {};

        const clearAll = () => {
            activeEstablishment.value = null;
            assignedEstablishments.value = null;
            products.value = null;
            pendingMonitoredPrices.value = [];
        };

        useIntervalFn(syncPendingMonitoredPrices, 5 * 1000);
        return {activeEstablishment, assignedEstablishments, products, isOnline, pendingMonitoredPrices, localDataUploaded, addPendingMonitoredPrice, syncPendingMonitoredPrices, clearAll}
    },
    {
        persist: true,
    },
);