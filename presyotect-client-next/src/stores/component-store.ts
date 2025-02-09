import {defineStore} from "pinia";
import {ref} from "vue";

export const useComponentStore = defineStore(
    "componentStore",
    () => {
        const isSidebarOpen = ref(false)
        return {isSidebarOpen}
    },
    {
        persist: true,
    },
);