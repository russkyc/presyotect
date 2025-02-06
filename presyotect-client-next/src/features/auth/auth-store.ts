import {computed, ref} from 'vue';
import {defineStore} from "pinia";
import {jwtDecode} from "jwt-decode";

export const useAuthStore = defineStore(
    'authStore',
    () => {
        const token = ref();
        const userClaims = computed(() => {
            return jwtDecode(token.value) as any;
        });
        const userRoles = computed(() => {
            const claims = jwtDecode(token.value) as any;
            return claims?.role ?? null;
        });
        return {token, userClaims, userRoles};
    },
    {
        persist: true,
    },
)