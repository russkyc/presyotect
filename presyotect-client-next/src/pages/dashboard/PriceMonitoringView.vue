<script lang="ts" setup>
import {Card} from "primevue";
import Page from "@/components/Page.vue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Price Monitoring"}
];

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="View latest recorded prices and product price movements"
    title="Price Monitoring"
  >
    <template #content>
      <Card class="grow rounded-lg" />
    </template>
  </Page>
</template>

<style scoped>

</style>