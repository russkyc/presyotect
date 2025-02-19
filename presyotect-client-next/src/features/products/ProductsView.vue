<script lang="ts" setup>

import {FilterMatchMode} from "@primevue/core";
import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {ConfigurationService} from "@services/data/configuration-service.ts";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import {
    Button,
    Card,
    Checkbox,
    Chip,
    Column,
    ContextMenu,
    DataTable,
    type DataTableRowContextMenuEvent,
    Dialog,
    IconField,
    InputIcon,
    InputText,
    Message,
    SplitButton,
    useToast
} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {onMounted, ref} from "vue";
import {z} from "zod";
import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import {ProductsService} from "@/services/data/products-service.ts";
import type {BreadcrumbItem, Category, Classification, Product} from "@/types/Interfaces.ts";

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Products"}
];

const selectedClassifications = ref<Classification[]>([]);
const availableClassifications = ref<Classification[]>([]);

const availableCategories = ref<Category[]>([]);

const items = [
    {
        label: "Import",
        command: () => {
            // Import Command
        }
    }
];

const showAddClassificationDialog = ref(false);
const showAddCategoryDialog = ref(false);

const initialCategoryValues = ref<Category>({ name: "", shortName: "", group: "products" });
const initialClassificationValues = ref<Classification>({ name: "", shortName: "" });

const categoryResolver = ref(zodResolver(
    z.object({
        name: z.string().min(1, { message: "Category name is required." }),
        shortName: z.string().min(1, {message: "Short name is required"})
    })
));

const classificationResolver = ref(zodResolver(
    z.object({
        name: z.string().min(1, { message: "Classification name is required." }),
        shortName: z.string().min(1, {message: "Short name is required"})
    })
));

const onCategoryFormSubmit = async (form: FormSubmitEvent) => {
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Incomplete Category Details",
            detail: "Please fill-out all required fields.",
            life: 2000
        });
        return;
    }
    confirm.require({
        message: `Are you sure you want to add ${form.values.name} to the categories list?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Add"
        },
        accept: async () => {
            const category = form.values as Category;
            category.group = "products";
            const response = await ConfigurationService.addCategory(category);
            if (!response) {
                toast.add({
                    severity: "error",
                    summary: "Category Not Added",
                    detail: `An error occurred while adding ${form.values.name} to categories.`,
                    life: 2000
                });
                return;
            }
            toast.add({
                severity: "success",
                summary: "Category Added",
                detail: `${form.values.name} added to categories.`,
                life: 2000
            });
            availableCategories.value.push(response);
            showAddCategoryDialog.value = false;
        }
    });
};

const onClassificationFormSubmit = async (form: FormSubmitEvent) => {
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Incomplete Classification Details",
            detail: "Please fill-out all required fields.",
            life: 2000
        });
        return;
    }
    confirm.require({
        message: `Are you sure you want to add ${form.values.name} to the classifications list?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Add"
        },
        accept: async () => {
            const classification = form.values as Classification;
            const response = await ConfigurationService.addClassification(classification);
            if (!response) {
                toast.add({
                    severity: "error",
                    summary: "Classification Not Added",
                    detail: `An error occurred while adding ${form.values.name} to classifications.`,
                    life: 2000
                });
                return;
            }
            toast.add({
                severity: "success",
                summary: "Classification Added",
                detail: `${form.values.name} added to classifications.`,
                life: 2000
            });
            availableClassifications.value.push(response);
            showAddClassificationDialog.value = false;
        }
    });
};

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
    GetProducts();
    GetClassifications();
    GetCategories();
});

const GetProducts = () => {
    ProductsService.getProducts().then((response) => {
        products.value = response;
    });
}

const GetCategories = () => {
    ConfigurationService.getCategories("products").then((response) => {
        availableCategories.value = response;
    });
}

const GetClassifications = () => {
    ConfigurationService.getClassifications().then((response) => {
        availableClassifications.value = response;
    });
}

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

const selectClassification = (selectedClassification: Classification) => {
    const classification = selectedClassifications.value.find(classification => classification == selectedClassification);
    if (classification) {
        selectedClassifications.value = selectedClassifications.value.filter((selected: Classification) => selected != selectedClassification);
    } else {
        selectedClassifications.value.push(selectedClassification);
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
          <template
            v-for="classification in availableClassifications"
            :key="classification.id"
          >
            <Card class="grow rounded-lg [&>.p-card-body]:p-3">
              <template #content>
                <div class="flex">
                  <p class="grow">
                    {{ classification.name }}
                  </p>
                  <Checkbox
                    binary
                    class="[&>.p-checkbox-box]:rounded-md"
                  />
                </div>
              </template>
            </Card>
          </template>
          <Button
            variant="outlined"
            @click="showAddClassificationDialog = true"
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
          </Button>
        </div>
        <h3 class="text-lg font-medium">
          Categories
        </h3>
        <div class="flex flex-col gap-3">
          <template
            v-for="category in availableCategories"
            :key="category.id"
          >
            <Card
              class="grow rounded-lg [&>.p-card-body]:p-3"
            >
              <template #content>
                <div class="flex">
                  <p class="grow">
                    {{ category.name }}
                  </p>
                  <Checkbox
                    binary
                    class="[&>.p-checkbox-box]:rounded-md"
                  />
                </div>
              </template>
            </Card>
          </template>
          <Button
            variant="outlined"
            @click="showAddCategoryDialog = true"
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
          </Button>
        </div>
      </div>
    </template>
  </Page>
  <Dialog
    v-model:visible="showAddCategoryDialog"
    modal
    class="md:w-96 w-full"
    header="Add Category"
  >
    <Form
      v-slot="$form"
      :initial-values="initialCategoryValues"
      :resolver="categoryResolver"
      class="flex flex-col gap-8 w-full"
      @submit="onCategoryFormSubmit"
    >
      <div class="flex flex-col gap-4">
        <div class="flex flex-col gap-1">
          <label for="name">Category Name</label>
          <InputText
            fluid
            name="name"
            placeholder="Category name"
            type="text"
            variant="filled"
          />
          <Message
            v-if="$form.name?.invalid"
            severity="error"
            size="small"
            variant="simple"
          >
            {{ $form.name.error?.message }}
          </Message>
        </div>
        <div class="flex flex-col gap-1">
          <label for="shortName">Short Name</label>
          <InputText
            fluid
            name="shortName"
            placeholder="Classification name"
            type="text"
            variant="filled"
          />
          <Message
            v-if="$form.shortName?.invalid"
            severity="error"
            size="small"
            variant="simple"
          >
            {{ $form.shortName.error?.message }}
          </Message>
        </div>
      </div>
      <div class="flex justify-end gap-2">
        <Button
          type="button"
          label="Cancel"
          severity="secondary"
          @click="showAddCategoryDialog = false"
        />
        <Button
          type="submit"
          label="Save"
        />
      </div>
    </Form>
  </Dialog>
  <Dialog
    v-model:visible="showAddClassificationDialog"
    modal
    class="md:w-96 w-full"
    header="Add Classification"
  >
    <Form
      v-slot="$form"
      :initial-values="initialClassificationValues"
      :resolver="classificationResolver"
      class="flex flex-col gap-8 w-full"
      @submit="onClassificationFormSubmit"
    >
      <div class="flex flex-col gap-4">
        <div class="flex flex-col gap-1">
          <label for="name">Classification Name</label>
          <InputText
            fluid
            name="name"
            placeholder="Classification name"
            type="text"
            variant="filled"
          />
          <Message
            v-if="$form.name?.invalid"
            severity="error"
            size="small"
            variant="simple"
          >
            {{ $form.name.error?.message }}
          </Message>
        </div>
        <div class="flex flex-col gap-1">
          <label for="shortName">Short Name</label>
          <InputText
            fluid
            name="shortName"
            placeholder="Classification name"
            type="text"
            variant="filled"
          />
          <Message
            v-if="$form.shortName?.invalid"
            severity="error"
            size="small"
            variant="simple"
          >
            {{ $form.shortName.error?.message }}
          </Message>
        </div>
      </div>
      <div class="flex justify-end gap-2">
        <Button
          type="button"
          label="Cancel"
          severity="secondary"
          @click="showAddClassificationDialog = false"
        />
        <Button
          type="submit"
          label="Save"
        />
      </div>
    </Form>
  </Dialog>
</template>

<style scoped>

</style>