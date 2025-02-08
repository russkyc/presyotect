<script setup lang="ts">
import Page from "@/components/Page.vue";
import type { BreadcrumbItem } from "@/types/Interfaces.ts";
import { Card } from "primevue";

const breadcrumbs: BreadcrumbItem[] = [
  { label: "Price Monitoring" }
];

</script>

<template>
  <Page :breadcrumbs="breadcrumbs" title="Price Monitoring" subtitle="View latest recorded prices and product price movements">
    <template #content>
      <Card class="grow rounded-lg"></Card>
    </template>
  </Page>
</template>

<style scoped>

</style>