<script lang="ts" setup>

import {onBeforeMount, ref} from "vue";
import {useAuthStore} from "@/stores/auth-store.ts";

const props = defineProps<{
  roles?: string[]
}>();

const authStore = useAuthStore();
const isAuthorized = ref(false);

onBeforeMount(() => {
    if (props.roles) {
        if (!authStore.userClaims) return;
        isAuthorized.value = props.roles.includes(authStore.userClaims.role);
        return;
    }
    isAuthorized.value = true;
});

authStore.$subscribe(() => {
    if (!authStore.userClaims) return;
    isAuthorized.value = props.roles === undefined ? true : props.roles.includes(authStore.userClaims.role);
});

</script>

<template>
  <slot v-if="isAuthorized" />
</template>

<style scoped>

</style>