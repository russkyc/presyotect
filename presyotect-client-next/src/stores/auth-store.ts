import {useIntervalFn} from "@vueuse/core";
import {jwtDecode} from "jwt-decode";
import {defineStore} from "pinia";
import {computed, ref} from "vue";
import router from "@/router.ts";
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

        const isTokenExpired = ref(true);

        const checkTokenExpiration = async () => {
            if (!token.value) {
                isTokenExpired.value = true;
                await router.push({name: "login"});
                return;
            }
            const claims = jwtDecode(token.value) as IDecodedToken | null;
            if (!claims) {
                isTokenExpired.value = true;
                await router.push({name: "login"});
                return;
            }
            const currentTime = Math.floor(Date.now() / 1000);
            isTokenExpired.value = claims.exp < currentTime;
            if (isTokenExpired.value) {
                await router.push({name: "login"});
            }
        };

        // Check token expiration every minute
        useIntervalFn(checkTokenExpiration, 15000);

        checkTokenExpiration();

        return {token, userClaims, userRoles, isTokenExpired};
    },
    {
        persist: true,
    },
)