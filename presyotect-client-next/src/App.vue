<script setup lang="ts">
import {ref, watch} from "vue";
import {useRoute} from 'vue-router';
import {getPageType} from "@utils/PathUtils.ts";
import TopNav from "@/components/TopNav.vue";
import Sidebar from "@/components/Sidebar.vue";
import Toast from "primevue/toast";
import ConfirmDialog from "primevue/confirmdialog";
import {DashboardGroupType} from "@/types/Types.ts";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import BottomNav from "@/components/BottomNav.vue";

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual('lg');

const router = useRoute();
const pageType = ref(getPageType(router.fullPath));

watch(() => router.fullPath, (newPath) => {
  pageType.value = getPageType(newPath);
});

</script>
<template>
  <Toast class="max-sm:hidden mt-12 lg:mt-16" position="top-right" />
  <Toast class="sm:hidden mt-14" position="top-center" />
  <ConfirmDialog />
  <TopNav v-if="pageType === DashboardGroupType.Dashboard" />
  <Sidebar mini-width="69px" width="260px" v-if="pageType === DashboardGroupType.Dashboard && !mdAndSmaller" />
  <main class="flex flex-col overflow-y-auto"
    :class="{'col-span-2 row-span-3': pageType !== DashboardGroupType.Dashboard}">
    <RouterView />
  </main>
  <BottomNav v-if="mdAndSmaller && pageType === DashboardGroupType.Dashboard" />
</template>