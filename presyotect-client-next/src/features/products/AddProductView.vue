<script lang="ts" setup>

import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {ConfigurationService} from "@services/data/configuration-service.ts";
import {useActionsStore} from "@stores/actions-store";
import {Button, InputText, Message, MultiSelect, Select} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {useToast} from "primevue/usetoast";
import {onMounted, ref} from "vue";
import {z} from "zod";
import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import {ProductsService} from "@/services/data/products-service.ts";
import type {BreadcrumbItem, Product} from "@/types/Interfaces.ts";

const toast = useToast();
const confirm = useConfirm();
const actionsStore = useActionsStore();

const initialValues = ref<Product>({
    sku: null,
    key: null,
    name: "",
    size: "",
    classification: null,
    category: [],
    srp: null
});

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Products", url: "/products"},
    {label: "Add"}
];

const availableCategories = ref();

const availableClassifications = ref([
    "Basic Necessities",
    "Prime Commodities",
    "Construction Materials",
    "School Supplies"
]);

const resolver = ref(zodResolver(
    z.object({
        sku: z.string().nullable(),
        key: z.string().nullable(),
        name: z.string().min(1, {message: "Product name is required."}),
        size: z.string().min(1, {message: "Product size is required."}),
        classification: z.string({message: "Product classification is required."}),
        category: z.array(z.string()).nullable()
    })
));

onMounted(() => {
    actionsStore.addPendingActions();

    ConfigurationService.getCategories().then((response) => {
        availableCategories.value = response;
    });
});

const onFormSubmit = async (form: FormSubmitEvent) => {
    console.log(form.values);
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Incomplete Product Details",
            detail: "Please fill-out all required fields.",
            life: 2000
        });
        return;
    }
    confirm.require({
        message: `Are you sure you want to add ${form.values.name} to the products list?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Add"
        },
        accept: async () => {

            const product = {
                sku: form.values.sku,
                key: form.values.customIdentifier,
                name: form.values.name,
                size: form.values.size,
                classification: form.values.classification,
                category: form.values.category,
                srp: form.values.srp
            };
            const response = await ProductsService.addProduct(product);
            if (!response) {
                toast.add({
                    severity: "error",
                    summary: "Product Not Added",
                    detail: `An error occurred while adding ${form.values.name} to products.`,
                    life: 2000
                });
                return;
            }
            toast.add({
                severity: "success",
                summary: "Product Added",
                detail: `${form.values.name} added to products.`,
                life: 2000
            });
            actionsStore.clearPendingActions();
            await router.push({path: "/products"});
        }
    });
};

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="Add new product"
    title="Products"
  >
    <template #content>
      <PageCard>
        <template #card-title>
          <h3 class="text-lg font-medium">
            Product Details
          </h3>
        </template>
        <template #card-content>
          <Form
            v-slot="$form"
            :initial-values="initialValues"
            :resolver="resolver"
            class="flex flex-col gap-8 w-full"
            @submit="onFormSubmit"
          >
            <div class="flex flex-col gap-4">
              <div class="flex flex-col gap-1">
                <label for="key">Custom Key (optional)</label>
                <InputText
                  fluid
                  name="key"
                  placeholder="Numbers, letters, or alphanumeric"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.key?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.key.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="sku">Product Sku (optional)</label>
                <InputText
                  fluid
                  name="sku"
                  placeholder="EAN, Code 39, Code 128, or others"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.sku?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.sku.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="name">Product Name</label>
                <InputText
                  fluid
                  name="name"
                  placeholder="Full product name"
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
                <label for="name">Product Size (with unit)</label>
                <InputText
                  fluid
                  name="size"
                  placeholder="Size in weight, volume, or pieces"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.size?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.size.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="classification">Classification</label>
                <Select
                  :options="availableClassifications"
                  fluid
                  name="classification"
                  placeholder="Select classification"
                  variant="filled"
                />
                <Message
                  v-if="$form.classification?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.classification.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="category">Categories (multiple, optional)</label>
                <MultiSelect
                  :max-selected-labels="3"
                  :options="availableCategories"
                  fluid
                  option-label="name"
                  option-value="name"
                  name="category"
                  placeholder="Select categories"
                  variant="filled"
                />
                <Message
                  v-if="$form.category?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.category.error?.message }}
                </Message>
              </div>
            </div>
            <Button
              class="sm:ml-auto"
              label="Submit"
              type="submit"
            >
              Save Product
            </Button>
          </Form>
        </template>
      </PageCard>
    </template>
  </Page>
</template>

<style scoped>

</style>