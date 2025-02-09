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
        if (!authStore.userRoles) return;
        isAuthorized.value = props.roles.includes(authStore.userRoles);
        return;
    }
    isAuthorized.value = true;
});

authStore.$subscribe(() => {
    if (!authStore.userRoles) return;
    isAuthorized.value = props.roles === undefined ? true : props.roles.includes(authStore.userRoles);
    console.log(isAuthorized.value);
});

</script>

<template>
  <slot v-if="isAuthorized" />
</template>

<style scoped>

</style>