<script setup lang="ts">

import {ref, watchEffect} from "vue";
import {useRoute} from 'vue-router';
import {getPageType} from "@utils/PathUtils.ts";
import TopNav from "@/components/TopNav.vue";
import Sidebar from "@/components/Sidebar.vue";
import Toast from "primevue/toast";
import ConfirmDialog from "primevue/confirmdialog";
import {DashboardGroupType} from "@/types/Types.ts";
import {breakpointsTailwind, useBreakpoints} from '@vueuse/core'
import BottomNav from "@/components/BottomNav.vue";

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual('lg');

const router = useRoute();
const pageType = ref(getPageType(router.fullPath));

watchEffect(() => {
  pageType.value = getPageType(router.fullPath);
});

</script>
<template>
  <Toast class="lg:block hidden" position="bottom-right"/>
  <Toast class="max-lg:block hidden" position="top-center"/>
  <ConfirmDialog/>
  <TopNav v-if="pageType === DashboardGroupType.Dashboard"/>
  <main class="relative flex overflow-clip" :class="{'grow': pageType === DashboardGroupType.Auth,'h-[calc(100vh-63.2px)]': pageType === DashboardGroupType.Dashboard,'h-[calc(100vh-126.4px)]': pageType === DashboardGroupType.Dashboard && mdAndSmaller}">
    <Sidebar mini-width="69px" width="240px" v-if="pageType === DashboardGroupType.Dashboard && !mdAndSmaller"/>
    <div class="flex flex-col grow overflow-auto">
      <RouterView/>
    </div>
  </main>
  <BottomNav v-if="pageType === DashboardGroupType.Dashboard && mdAndSmaller"/>
</template>
