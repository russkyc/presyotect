<script lang="ts" setup>

import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import {Card} from "primevue";
import Page from "@/components/Page.vue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";

const breakpoints = useBreakpoints(breakpointsTailwind);
const isMobile = breakpoints.smallerOrEqual("sm");

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Settings"}
];

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="Manage global and account preferences"
    title="Settings"
  >
    <template #content>
      <template v-if="!isMobile">
        <Card class="grow rounded-lg" />
      </template>
      <template v-if="isMobile">
        <div class="flex flex-col gap-4 grow">
          <h2 class="font-semibold text-lg">
            Settings
          </h2>
          <Card class="grow rounded-lg" />
        </div>
      </template>
    </template>
  </Page>
</template>

<style scoped>

</style>