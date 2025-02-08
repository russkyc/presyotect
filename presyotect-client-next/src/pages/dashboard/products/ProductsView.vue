<script setup lang="ts">

import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import type { BreadcrumbItem, Product } from "@/types/Interfaces.ts";
import { ProductsService } from "@features/data/products-service.ts";
import { FilterMatchMode } from "@primevue/core";
import { Chip, Column, ContextMenu, DataTable, IconField, InputIcon, InputText, SplitButton, useToast } from "primevue";
import { useConfirm } from "primevue/useconfirm";
import { onMounted, ref } from "vue";

const breadcrumbs: BreadcrumbItem[] = [
  {label: "Products"}
];

const items = [
  {
    label: "Import",
    command: () => {
      // Import Command
    }
  }
];

const confirm = useConfirm();
const toast = useToast();
const selectedProduct = ref();
const products = ref();
const filters = ref({
  global: {value: null, matchMode: FilterMatchMode.CONTAINS},
  name: {value: null, matchMode: FilterMatchMode.STARTS_WITH}
});

onMounted(() => {
  ProductsService.getProducts().then((response) => {
    products.value = response;
  });
});

const onRowContextMenu = (event: any) => {
  contextMenu.value.show(event.originalEvent);
};

const addProduct = () => {
  router.push({path: "/products/add"});
}

const deleteProduct = (product: Product) => {
  confirm.require({
    header: "Delete Product",
    message: `This will remove ${product.name} permanently. Are you sure?`,
    rejectProps: {
      label: "Cancel",
      severity: "secondary"
    },
    acceptProps: {
      label: "Delete",
      severity: "danger"
    },
    accept: async () => {
      if (product.id == null) return;
      const deleted = await ProductsService.deleteProduct(product.id);
      
      if (!deleted){
        toast.add({severity: "error", summary: "Unsuccessful Deletion", detail: `${product.name} cannot be deleted.`, life: 3000});
        return;
      }
      
      toast.add({severity: "success", summary: "Product Deleted", detail: `Successfully removed ${product.name} from producs.`, life: 3000});
      products.value = products.value.filter((p: Product) => p.id !== product.id);
      selectedProduct.value = null;
    }
  });
};

const contextMenu = ref();
const menuModel = ref([
  {
    label: "Edit",
    icon: "pi pi-fw pi-pencil"
  },
  {
    label: "Delete",
    icon: "pi pi-fw pi-times",
    command: () => deleteProduct(selectedProduct.value)
  }
]);

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
            <InputText v-model="filters['global'].value" variant="filled" placeholder="Find Product" name="search"
                       type="text" fluid class="max-sm:grow md:w-80">
            </InputText>
          </IconField>
        </template>
        <template #card-actions>
          <SplitButton :model="items" @click="addProduct">
            <div class="flex gap-2">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                   stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                   class="my-auto lucide lucide-plus max-sm:hidden">
                <path d="M5 12h14"/>
                <path d="M12 5v14"/>
              </svg>
              <p class="my-auto">Add</p>
            </div>
          </SplitButton>
        </template>
        <template #card-content>
          <ContextMenu ref="contextMenu" :model="menuModel" @hide="selectedProduct = null"/>
          <DataTable v-model:filters="filters" :value="products" paginator :rows="8"
                     :rowsPerPageOptions="[8, 20, 50, 100]" tableStyle="min-width: 50rem"
                     :globalFilterFields="['sku', 'name', 'size', 'category']"
                     contextMenu v-model:contextMenuSelection="selectedProduct"
                     @rowContextmenu="onRowContextMenu"
                     removableSort>
            <Column field="id" header="Id" class="w-[5%]" :sortable="true"></Column>
            <Column field="name" header="Name" class="w-[15%]" :sortable="true"></Column>
            <Column field="size" header="Size" class="w-[10%]" :sortable="true"></Column>
            <Column field="sku" header="Sku" class="w-[10%]" :sortable="true"></Column>
            <Column field="category" header="Category" class="w-[10%]" :sortable="true">
              <template #body="slotProps">
                <div class="flex gap-2" v-if="slotProps.data.category.length > 0">
                  <Chip
                      class="py-1 font-medium text-xs px-2 rounded-full text-[--p-primary-color] bg-[--p-highlight-background]">
                    {{ slotProps.data.category[0] }}
                  </Chip>
                  <Chip v-if="slotProps.data.category.length > 1"
                        class="py-1 px-1.5 font-medium text-xs rounded-full  text-[--p-primary-color] bg-[--p-highlight-background]">
                    +{{ slotProps.data.category.length - 1 }}
                  </Chip>
                </div>
              </template>
            </Column>
            <Column field="classification" header="Classification" class="w-[10%]" :sortable="true">
              <template #body="slotProps">
                <Chip
                    class="py-1 font-medium text-xs px-2 rounded-full  text-[--p-primary-color] bg-[--p-highlight-background]">
                  {{ slotProps.data.classification }}
                </Chip>
              </template>
            </Column>
          </DataTable>
        </template>
      </PageCard>
    </template>
  </Page>
</template>

<style scoped>

</style>