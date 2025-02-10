<script lang="ts" setup>

import {Breadcrumb, Drawer} from "primevue";
import {ref} from "vue";
import router from "@/router.ts";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";

const props = defineProps<{
  title: string,
  subtitle: string,
  breadcrumbs?: BreadcrumbItem[],
  showDrawerToggle?: boolean
}>();

const home = ref({
    label: "Dashboard",
    url: "/"
});

const openDrawer = ref(false);
const showDrawerToggle = props.showDrawerToggle ?? false;

const toggleDrawer = () => {
    openDrawer.value = !openDrawer.value;
};

const navigate = (url: string | undefined) => {
    if (url === undefined) return;
    router.push({path: url});
};

</script>

<template>
  <div class="flex grow overflow-y-auto flex-col px-4 pt-3 sm:px-6 sm:pt-5 gap-3 sm:gap-4">
    <div class="flex gap-4 max-sm:hidden">
      <div class="flex flex-col">
        <h1 class="text-xl font-semibold">
          {{ title }}
        </h1>
        <p
          class="opacity-60 max-md:hidden"
        >
          {{ subtitle }}
        </p>
      </div>
      <Breadcrumb
        :home="home"
        :model="breadcrumbs"
        class="ml-auto max-sm:hidden"
      >
        <template #item="{ item }">
          <button @click="navigate(item.url)">
            <p>{{ item.label }}</p>
          </button>
        </template>
        <template #separator>
          /
        </template>
      </Breadcrumb>
      <button
        v-if="showDrawerToggle"
        class="my-auto max-sm:ml-auto p-button-secondary rounded-full p-1"
        @click="toggleDrawer"
      >
        <svg
          class="lucide lucide-bolt"
          fill="none"
          height="18"
          stroke="currentColor"
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          viewBox="0 0 24 24"
          width="18"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"
          />
          <circle
            cx="12"
            cy="12"
            r="4"
          />
        </svg>
      </button>
    </div>
    <slot name="content-header" />
    <div class="flex grow flex-col overflow-y-auto">
      <div class="flex flex-col grow mb-6">
        <slot name="content" />
      </div>
    </div>
    <slot name="content-footer" />
  </div>
  <Drawer
    v-model:visible="openDrawer"
    header="Right Drawer"
    position="right"
  >
    <slot name="drawer-content" />
  </Drawer>
</template>

<style scoped>

</style>