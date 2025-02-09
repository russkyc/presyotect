<script lang="ts" setup>
import {getPageType} from "@utils/PathUtils.ts";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import ConfirmDialog from "primevue/confirmdialog";
import Toast from "primevue/toast";
import {computed, ref, watch} from "vue";
import {useRoute} from "vue-router";
import BottomNav from "@/components/BottomNav.vue";
import Sidebar from "@/components/Sidebar.vue";
import TopNav from "@/components/TopNav.vue";
import {DashboardGroupType} from "@/types/Types.ts";

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual("lg");

const router = useRoute();
const pageType = ref(getPageType(router.fullPath));
const isDashboardPageType = computed(() => pageType.value === DashboardGroupType.Dashboard);

watch(() => router.fullPath, (newPath) => {
    pageType.value = getPageType(newPath);
});

</script>
<template>
  <Toast
    class="max-sm:hidden mt-12 lg:mt-16"
    position="top-right"
  />
  <Toast
    class="sm:hidden mt-14"
    position="top-center"
  />
  <ConfirmDialog />
  <TopNav v-if="isDashboardPageType" />
  <Sidebar
    v-if="isDashboardPageType && !mdAndSmaller"
    mini-width="69px"
    width="260px"
  />
  <main
    :class="{'col-span-2 row-span-3': !isDashboardPageType}"
    class="flex flex-col overflow-y-auto"
  >
    <RouterView />
  </main>
  <BottomNav v-if="isDashboardPageType && mdAndSmaller" />
</template>