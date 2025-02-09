<script lang="ts" setup>

import {Card} from "primevue";
import Page from "@/components/Page.vue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Users"}
];

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="Manage employees and user information"
    title="Users"
  >
    <template #content>
      <Card class="grow rounded-lg" />
    </template>
  </Page>
</template>

<style scoped>

</style>