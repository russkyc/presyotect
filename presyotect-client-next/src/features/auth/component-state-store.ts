import {ref} from 'vue';
import {defineStore} from "pinia";

export const useComponentStore = defineStore(
    'componentStore',
    () => {
        const isSidebarOpen = ref(false)
        return {isSidebarOpen}
    },
    {
        persist: true,
    },
)