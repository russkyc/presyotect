<script lang="ts" setup>

import {FilterMatchMode} from "@primevue/core";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import {
    Card,
    Chip,
    Column,
    ContextMenu,
    DataTable,
    type DataTableRowContextMenuEvent,
    IconField,
    InputIcon,
    InputText,
    SplitButton,
    useToast
} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {onMounted, ref} from "vue";
import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import {ProductsService} from "@/services/data/products-service.ts";
import type {BreadcrumbItem, Product} from "@/types/Interfaces.ts";

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Products"}
];

const selectedClassifications = ref<string[]>([]);
const availableClassifications = ref([
    {shortName: "Basic", name: "Basic Necessities"},
    {shortName: "Prime", name: "Prime Commodities"},
    {shortName: "Construction", name: "Construction Materials"},
    {shortName: "School", name: "School Supplies"}
]);

const availableCategories = ref([
    "Powdered Milk",
    "Canned Goods",
    "Bottled Water"
]);

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
    name: {value: null, matchMode: FilterMatchMode.STARTS_WITH},
    classifications: {value: null, matchMode: FilterMatchMode.EQUALS}
});

onMounted(() => {
    ProductsService.getProducts().then((response) => {
        products.value = response;
    });
});

const onRowContextMenu = (event: DataTableRowContextMenuEvent) => {
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

            if (!deleted) {
                toast.add({
                    severity: "error",
                    summary: "Unsuccessful Deletion",
                    detail: `${product.name} cannot be deleted.`,
                    life: 3000
                });
                return;
            }

            toast.add({
                severity: "success",
                summary: "Product Deleted",
                detail: `Successfully removed ${product.name} from producs.`,
                life: 3000
            });
            products.value = products.value.filter((p: Product) => p.id !== product.id);
            selectedProduct.value = null;
        }
    });
};

const breakpoints = useBreakpoints(breakpointsTailwind);
const isMobile = breakpoints.smallerOrEqual("sm");
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

const selectClassification = (classification: string) => {
    if (selectedClassifications.value.includes(classification)) {
        selectedClassifications.value = selectedClassifications.value.filter((selected: string) => selected !== classification);
    } else {
        selectedClassifications.value.push(classification);
    }
}

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    :show-drawer-toggle="true"
    subtitle="Add and modify monitored products"
    title="Products"
  >
    <template #content>
      <template v-if="!isMobile">
        <PageCard v-if="!isMobile">
          <template #card-title>
            <div class="flex gap-4">
              <IconField>
                <InputIcon>
                  <svg
                    class="lucide lucide-search my-auto"
                    fill="none"
                    height="16"
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    viewBox="0 0 24 24"
                    width="16"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <circle
                      cx="11"
                      cy="11"
                      r="8"
                    />
                    <path d="m21 21-4.3-4.3" />
                  </svg>
                </InputIcon>
                <InputText
                  v-model="filters['global'].value"
                  class="max-sm:grow md:w-80"
                  fluid
                  name="search"
                  placeholder="Find Product"
                  type="text"
                  variant="filled"
                />
              </IconField>
              <div class="flex gap-2 flex-wrap">
                <template
                  v-for="classification in availableClassifications"
                  :key="classification.shortName"
                >
                  <div class="card flex justify-center">
                    <Chip
                      :data-selected="selectedClassifications.includes(classification.name)"
                      @click="selectClassification(classification.name)"
                      class="text-sm leading-none pb-1.5 my-auto py-1 px-3 font-semibold rounded-full text-[--p-primary-color] border bg-[--p-highlight-background] data-[selected=true]:text-[--p-primary-contrast-color] data-[selected=true]:border-[--p-primary-active-color] data-[selected=true]:bg-[--p-primary-color]"
                    >
                      {{ classification.shortName }}
                    </Chip>
                  </div>
                </template>
              </div>
            </div>
          </template>
          <template #card-actions>
            <SplitButton
              :model="items"
              @click="addProduct"
            >
              <div class="flex gap-2">
                <svg
                  class="my-auto lucide lucide-plus max-sm:hidden"
                  fill="none"
                  height="20"
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  viewBox="0 0 24 24"
                  width="20"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path d="M5 12h14" />
                  <path d="M12 5v14" />
                </svg>
                <p class="my-auto">
                  Add
                </p>
              </div>
            </SplitButton>
          </template>
          <template #card-content>
            <ContextMenu
              ref="contextMenu"
              :model="menuModel"
              @hide="selectedProduct = null"
            />
            <DataTable
              v-model:context-menu-selection="selectedProduct"
              v-model:filters="filters"
              :global-filter-fields="['sku','key', 'name', 'size', 'category']"
              :rows="8"
              :rows-per-page-options="[8, 20, 50, 100]"
              :value="products"
              context-menu
              paginator
              removable-sort
              table-style="min-width: 50rem"
              @row-contextmenu="onRowContextMenu"
            >
              <Column
                :sortable="true"
                class="w-[15%]"
                field="name"
                header="Name"
              />
              <Column
                :sortable="true"
                class="w-[10%]"
                field="size"
                header="Size"
              />
              <Column
                :sortable="true"
                class="w-[10%]"
                field="sku"
                header="Sku"
              />
              <Column
                :sortable="true"
                class="w-[10%]"
                field="category"
                header="Category"
              >
                <template #body="slotProps">
                  <div
                    v-if="slotProps.data.category && slotProps.data.category.length > 0"
                    class="flex gap-2"
                  >
                    <Chip
                      class="py-1 font-medium text-xs px-2 rounded-full text-[--p-primary-color] bg-[--p-highlight-background]"
                    >
                      {{ slotProps.data.category[0] }}
                    </Chip>
                    <Chip
                      v-if="slotProps.data.category.length > 1"
                      class="py-1 px-1.5 font-medium text-xs rounded-full  text-[--p-primary-color] bg-[--p-highlight-background]"
                    >
                      +{{ slotProps.data.category.length - 1 }}
                    </Chip>
                  </div>
                </template>
              </Column>
              <Column
                :sortable="true"
                class="w-[10%]"
                field="classification"
                header="Classification"
              >
                <template #body="slotProps">
                  <Chip
                    class="py-1 font-medium text-xs px-2 rounded-full  text-[--p-primary-color] bg-[--p-highlight-background]"
                  >
                    {{ slotProps.data.classification }}
                  </Chip>
                </template>
              </Column>
            </DataTable>
          </template>
        </PageCard>
      </template>
      <template v-if="isMobile">
        <div class="flex flex-col gap-4 grow">
          <h2 class="font-semibold text-lg">
            Products
          </h2>
          <Card class="grow rounded-lg" />
        </div>
      </template>
    </template>
    <template #drawer-header>
      <h2 class="font-medium text-xl">
        Configuration
      </h2>
    </template>
    <template #drawer-content>
      <div class="flex flex-col gap-4">
        <h3 class="text-lg font-medium">
          Classifications
        </h3>
        <div class="flex flex-col gap-3">
          <template v-for="classification in availableClassifications">
            <Card class="grow rounded-lg">
              <template #content>
                <p>{{ classification.name }}</p>
              </template>
            </Card>
          </template>
        </div>
        <h3 class="text-lg font-medium">
          Categories
        </h3>
        <div class="flex flex-col gap-3">
          <template v-for="category in availableCategories">
            <Card class="grow rounded-lg">
              <template #content>
                <p>{{ category }}</p>
              </template>
            </Card>
          </template>
        </div>
      </div>
    </template>
  </Page>
</template>

<style scoped>

</style>