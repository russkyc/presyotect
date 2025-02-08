<script setup lang="ts">

import router from "@/router";
import { logout } from "@features/auth/auth-service.ts";
import { useComponentStore } from "@features/stores.ts";
import { breakpointsTailwind, useBreakpoints } from "@vueuse/core";
import { Avatar, Button, Menu, Toolbar } from "primevue";
import { useConfirm } from "primevue/useconfirm";
import { useToast } from "primevue/usetoast";
import { ref } from 'vue';

const componentStore = useComponentStore();
const toast = useToast();
const confirm = useConfirm();
const profileMenu = ref();

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual('lg');

const togglePopover = (e: unknown) => {
  profileMenu.value.toggle(e);
}

const toggleSidebar = () => {
  componentStore.isSidebarOpen = !componentStore.isSidebarOpen;
}

const requestLogout = () => {
  confirm.require({
    message: 'Are you sure you want to end this session?',
    header: 'Confirmation',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary'
    },
    acceptProps: {
      label: 'Logout'
    },
    accept: async () => {
      await logout();
      toast.add({
        severity: 'success',
        summary: 'Session Ended',
        detail: 'You have logged out.',
        life: 2000
      });
      await router.push('/login');
    }
  });
}

const items = ref([
  {
    label: 'Logout',
    command: requestLogout
  }
]);

</script>

<template>
  <header class="col-span-2">
    <Toolbar class="px-3 py-2 border-b rounded-none">
      <template #start>
        <div class="flex gap-3">
          <Button v-if="!mdAndSmaller" severity="secondary" class="p-3" @click="toggleSidebar" text>
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
              stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
              class="lucide lucide-align-justify">
              <path d="M3 12h18" />
              <path d="M3 18h18" />
              <path d="M3 6h18" />
            </svg>
          </Button>
          <img alt="" src="/branding/logo-horizontal.svg" class="h-7 my-auto" />
        </div>
      </template>
      <template #end>
        <Button @click="togglePopover" variant="text" class="p-0" rounded>
          <Avatar image="https://i.pravatar.cc/100" shape="circle" />
        </Button>
        <Menu ref="profileMenu" :popup="true" :model="items" />
      </template>
    </Toolbar>
  </header>
</template>

<style scoped>

</style>