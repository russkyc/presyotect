import {refreshToken} from "@services/auth/auth-service.ts";
import {useIdle, useIntervalFn} from "@vueuse/core";
import {jwtDecode} from "jwt-decode";
import {defineStore} from "pinia";
import {computed, ref} from "vue";
import type {TokenPayload} from "@/types/Interfaces.ts";

export const useAuthStore = defineStore(
    "authStore",
    () => {
        const token = ref();
        const {idle} = useIdle(5 * 60 * 1000);
        const userClaims = computed(() => {
            return jwtDecode(token.value) as TokenPayload | null;
        });
        
        const isTokenExpired = computed(() => {
            if (!userClaims.value) {
                return true;
            }
            const currentTime = Math.floor(Date.now() / 1000);
            return userClaims.value.exp! < currentTime;
        });

        const checkTokenExpiration = async () => {
            console.log("Idle: ", idle.value);
            if (isTokenExpired.value && !idle.value) {
                console.log("Checking token expiration...");
                await refreshToken();
            }
        };

        useIntervalFn(checkTokenExpiration, 15 * 60 * 1000);

        checkTokenExpiration();

        return {token, userClaims, isTokenExpired};
    },
    {
        persist: true,
    },
)