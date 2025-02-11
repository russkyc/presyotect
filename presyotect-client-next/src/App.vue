<script lang="ts" setup>
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {getPageType} from "@utils/PathUtils.ts";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import ConfirmDialog from "primevue/confirmdialog";
import Toast from "primevue/toast";
import {useToast} from "primevue/usetoast";
import {computed, ref, watch} from "vue";
import {useRoute} from "vue-router";
import BottomNav from "@/components/BottomNav.vue";
import Sidebar from "@/components/Sidebar.vue";
import TopNav from "@/components/TopNav.vue";
import {DashboardGroupType} from "@/types/Types.ts";

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual("lg");

const toast = useToast();
const monitoringStore = useMonitoringStore();
const router = useRoute();
const pageType = ref(getPageType(router.fullPath));
const isDashboardPageType = computed(() => pageType.value === DashboardGroupType.Dashboard);

watch(() => router.fullPath, (newPath) => {
    pageType.value = getPageType(newPath);
});

monitoringStore.$onAction((action) => {
    if (action.name === "localDataUploaded") {
        toast.add({
            severity: "success",
            summary: "Records Uploaded",
            detail: "Local monitored prices synced to the server successfully.",
            life: 2000
        });
    }
});

</script>
<template>
  <div class="grid h-screen grid-cols-[1fr] lg:grid-cols-[auto_1fr] grid-rows-[auto_1fr_auto] bg-[--p-primary-contrast-color] text-[--p-text-color]">
    <Toast
      class="max-sm:hidden mt-12 lg:mt-16"
      position="top-right"
    />
    <Toast
      class="max-sm:px-4 sm:hidden sm:mt-14"
      position="top-center"
    />
    <ConfirmDialog class="max-sm:w-full max-sm:mx-4" />
    <TopNav v-if="isDashboardPageType" />
    <Sidebar
      v-if="isDashboardPageType && !mdAndSmaller"
      mini-width="69px"
      width="260px"
    />
    <main
      :class="{'col-span-2 row-span-3': !isDashboardPageType}"
      class="flex flex-1 flex-col overflow-hidden"
    >
      <RouterView />
    </main>
    <BottomNav v-if="isDashboardPageType && mdAndSmaller" />
  </div>
</template>