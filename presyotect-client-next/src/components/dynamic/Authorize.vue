<script setup lang="ts">

import { useAuthStore } from "@features/stores.ts";
import { defineProps, onBeforeMount, ref } from "vue";

const props = defineProps<{
  roles?: string[]
}>();

const authStore = useAuthStore();
const isAuthorized = ref(false);

onBeforeMount(() => {
  if (props.roles){
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
<slot v-if="isAuthorized"></slot>
</template>

<style scoped>

</style>