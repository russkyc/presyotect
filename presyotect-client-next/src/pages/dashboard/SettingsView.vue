<script setup lang="ts">

import Page from "@/components/Page.vue";
import type { BreadcrumbItem } from "@/types/Interfaces.ts";
import { Card } from "primevue";

const breadcrumbs: BreadcrumbItem[] = [
  { label: "Settings" }
];

</script>

<template>
  <Page :breadcrumbs="breadcrumbs" title="Settings" subtitle="Manage global and account preferences">
    <template #content>
      <Card class="grow rounded-lg"></Card>
    </template>
  </Page>
</template>

<style scoped>

</style>