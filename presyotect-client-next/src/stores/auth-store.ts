import {refreshToken} from "@services/auth/auth-service.ts";
import {useIntervalFn} from "@vueuse/core";
import {jwtDecode} from "jwt-decode";
import {defineStore} from "pinia";
import {computed, ref} from "vue";
import type {TokenPayload} from "@/types/Interfaces.ts";

export const useAuthStore = defineStore(
    "authStore",
    () => {
        const token = ref();
        const userClaims = computed(() => {
            if (token.value == null){
                return null;
            } else {
                return jwtDecode(token.value) as TokenPayload | null;
            }
        });
        
        const isTokenExpired = computed(() => {
            if (!userClaims.value) {
                return true;
            }
            const currentTime = Math.floor(Date.now() / 1000);
            return userClaims.value.exp! < currentTime;
        });

        const checkTokenExpiration = async () => {
            if (isTokenExpired.value) {
                await refreshToken();
            }
        };

        useIntervalFn(checkTokenExpiration, 15 * 60 * 1000);

        return {token, userClaims, isTokenExpired};
    },
    {
        persist: true,
    },
)