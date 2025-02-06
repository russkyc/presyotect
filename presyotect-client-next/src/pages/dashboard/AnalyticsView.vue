<script setup lang="ts">

import Page from "@/components/Page.vue";
import {Card} from "primevue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";

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