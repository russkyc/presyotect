<script setup lang="ts">

import Page from "@/components/Page.vue";
import type { BreadcrumbItem } from "@/types/Interfaces.ts";
import { Card } from "primevue";

const breadcrumbs: BreadcrumbItem[] = [
  { label: "Analytics" }
];

</script>

<template>
  <Page :breadcrumbs="breadcrumbs" title="Analytics" subtitle="View detailed monitoring statistics">
    <template #content>
      <Card class="grow rounded-lg"></Card>
    </template>
  </Page>
</template>

<style scoped>

</style>