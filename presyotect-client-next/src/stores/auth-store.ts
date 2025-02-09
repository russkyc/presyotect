import {jwtDecode} from "jwt-decode";
import {defineStore} from "pinia";
import {computed, ref} from "vue";
import type {IDecodedToken} from "@/types/Interfaces.ts";

export const useAuthStore = defineStore(
    "authStore",
    () => {
        const token = ref();
        const userClaims = computed(() => {
            return jwtDecode(token.value) as IDecodedToken | null;
        });
        const userRoles = computed(() => {
            const claims = jwtDecode(token.value) as IDecodedToken | null;
            return claims?.role;
        });
        return {token, userClaims, userRoles};
    },
    {
        persist: true,
    },
)