<script setup lang="ts">

import Page from "@/components/Page.vue";
import {Card} from "primevue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";

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