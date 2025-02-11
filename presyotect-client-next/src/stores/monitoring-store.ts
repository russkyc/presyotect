import axios from "axios";
import {defineStore} from "pinia";
import {computed, ref} from "vue";
import type {MonitoredEstablishment, Product} from "@/types/Interfaces.ts";

export const useMonitoringStore = defineStore(
    "monitoringStore",
    () => {
        const activeEstablishment = ref<MonitoredEstablishment|null>();
        const assignedEstablishments = ref<MonitoredEstablishment[]|null>();
        const products = ref<Product[]|null>();
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
        const clearAll = () => {
            activeEstablishment.value = null;
            assignedEstablishments.value = null;
            products.value = null;
        };
        return {activeEstablishment, assignedEstablishments, products, isOnline, clearAll}
    },
    {
        persist: true,
    },
);