<script setup lang="ts">
import Page from "@/components/Page.vue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";
import {Card} from "primevue";

const breadcrumbs: BreadcrumbItem[] = [
  { label: "Establishments" }
];

</script>

<template>
  <Page :breadcrumbs="breadcrumbs" title="Establishments" subtitle="Manage monitored establishments">
    <template #content>
      <Card class="grow rounded-lg"></Card>
    </template>
  </Page>
</template>

<style scoped>

</style>