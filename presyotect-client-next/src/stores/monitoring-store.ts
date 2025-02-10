import {defineStore} from "pinia";
import {ref} from "vue";
import type {Establishment} from "@/types/Interfaces.ts";

export const useMonitoringStore = defineStore(
    "monitoringStore",
    () => {
        const activeEstablishment = ref<Establishment|null>()
        return {activeEstablishment}
    },
    {
        persist: true,
    },
);