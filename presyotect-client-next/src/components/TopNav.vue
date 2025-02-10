<script lang="ts" setup>

import {useAuthStore} from "@stores/auth-store.ts";
import {useComponentStore} from "@stores/component-store.ts";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import {Avatar, Button, Chip, Menu, Toolbar} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {useToast} from "primevue/usetoast";
import {onMounted, ref} from "vue";
import Authorize from "@/components/dynamic/Authorize.vue";
import router from "@/router";
import {logout} from "@/services/auth/auth-service.ts";

const authStore = useAuthStore();
const componentStore = useComponentStore();
const toast = useToast();
const confirm = useConfirm();
const profileMenu = ref();
const nickname = ref();

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual("lg");
const small = breakpoints.smaller("sm");

onMounted(() => {
    if (authStore.userClaims) {
        nickname.value = authStore.userClaims.username;
    }
});

const togglePopover = (e: unknown) => {
    profileMenu.value.toggle(e);
}

const toggleSidebar = () => {
    componentStore.isSidebarOpen = !componentStore.isSidebarOpen;
}

const requestLogout = () => {
    confirm.require({
        message: "Are you sure you want to end this session?",
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Logout"
        },
        accept: async () => {
            await logout();
            toast.add({
                severity: "success",
                summary: "Session Ended",
                detail: "You have logged out.",
                life: 2000
            });
            await router.push("/login");
        }
    });
}

const items = ref([
    {
        label: "Logout",
        command: requestLogout
    }
]);

</script>

<template>
  <header class="col-span-2">
    <Toolbar class="px-3 py-2 border-b rounded-none">
      <template #start>
        <div class="flex gap-3">
          <Button
            v-if="!mdAndSmaller"
            class="p-3"
            severity="secondary"
            text
            @click="toggleSidebar"
          >
            <svg
              class="lucide lucide-align-justify"
              fill="none"
              height="20"
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              viewBox="0 0 24 24"
              width="20"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path d="M3 12h18" />
              <path d="M3 18h18" />
              <path d="M3 6h18" />
            </svg>
          </Button>
          <img
            alt=""
            class="h-7 my-auto"
            src="/branding/logo-horizontal.svg"
          >
        </div>
      </template>
      <template #end>
        <Authorize>
          <Chip
            v-if="!small"
            @click="togglePopover"
            :label="nickname"
            image="https://i.pravatar.cc/100"
          />
          <Avatar
            v-if="small"
            @click="togglePopover"
            shape="circle"
            image="https://i.pravatar.cc/100"
          />
        </Authorize>
        <Menu
          ref="profileMenu"
          :model="items"
          :popup="true"
        />
      </template>
    </Toolbar>
  </header>
</template>

<style scoped>

</style>