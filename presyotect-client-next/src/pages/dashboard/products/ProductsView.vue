<script setup lang="ts">

import {InputText} from "primevue";
import SplitButton from 'primevue/splitbutton';
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';
import Page from "@/components/Page.vue";
import type {BreadcrumbItem} from "@/types/Interfaces.ts";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";

const breadcrumbs: BreadcrumbItem[] = [
  {label: "Products"}
];

const items = [
  {
    label: 'Import',
    command: () => {
      // Import Command
    }
  }
];

const addProduct = () => {
  router.push({path:"/products/add"});
}

</script>

<template>
  <Page :breadcrumbs="breadcrumbs" title="Products" subtitle="Add and modify monitored products">
    <template #content>
      <PageCard>
        <template #card-title>
          <IconField>
            <InputIcon>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none"
                   stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                   class="lucide lucide-search my-auto">
                <circle cx="11" cy="11" r="8"/>
                <path d="m21 21-4.3-4.3"/>
              </svg>
            </InputIcon>
            <InputText variant="filled" placeholder="Find Product" name="search" type="text" fluid class="w-80">
            </InputText>
          </IconField>
        </template>
        <template #card-actions>
          <SplitButton :model="items" @click="addProduct">
            <div class="flex gap-2">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                   stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                   class="my-auto lucide lucide-plus"><path d="M5 12h14"/><path d="M12 5v14"/></svg>
              <p class="my-auto">Add</p>
            </div>
          </SplitButton>
        </template>
      </PageCard>
    </template>
  </Page>
</template>

<style scoped>

</style>