<script setup lang="ts">

import Page from "@/components/Page.vue";
import type { BreadcrumbItem } from "@/types/Interfaces.ts";
import { Card } from "primevue";

const breadcrumbs: BreadcrumbItem[] = [
  { label: "Users" }
];

</script>

<template>
  <Page :breadcrumbs="breadcrumbs" title="Users" subtitle="Manage employees and user information">
    <template #content>
      <Card class="grow rounded-lg"></Card>
    </template>
  </Page>
</template>

<style scoped>

</style>