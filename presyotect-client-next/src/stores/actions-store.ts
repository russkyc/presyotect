import {defineStore} from "pinia";
import {ref} from "vue";

export const useActionsStore = defineStore(
    "actionsStore",
    () => {
        const hasPendingActions = ref(false);
        const addPendingActions = () => {
            hasPendingActions.value = true;
        };
        const clearPendingActions = () => {
            hasPendingActions.value = false;
        }
        return {hasPendingActions, addPendingActions, clearPendingActions};
    }
)