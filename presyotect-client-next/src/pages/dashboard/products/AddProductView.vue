<script setup lang="ts">

import type { Product } from "@/types/Interfaces.ts";
import { ProductsService } from "@features/data/products-service.ts";
import { useActionsStore } from "@features/stores.ts";
import { Form } from "@primevue/forms";
import { zodResolver } from "@primevue/forms/resolvers/zod";
import { Button, InputText, Message, MultiSelect, Select } from "primevue";
import { useConfirm } from "primevue/useconfirm";
import { useToast } from "primevue/usetoast";
import { onMounted, ref } from "vue";
import { z } from "zod";

const props = defineProps<{
  completedAction: Function | null;
}>();

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

const availableCategories = ref([
  "Basic Necessities",
  "Prime Commodities",
  "Construction Materials",
  "School Supplies"
]);

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
      classification: z.string(),
      category: z.array(z.string()).nullable()
    })
));

onMounted(() => {
  actionsStore.addPendingActions();
});

const onFormSubmit = async (form: any) => {
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
      if (!response){
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
      props.completedAction?.();
    }
  });
};

</script>

<template>
  <Form v-slot="$form" :resolver="resolver" :initialValues="initialValues" @submit="onFormSubmit"
    class="flex flex-col gap-8 w-full">
    <div class="flex flex-col gap-4">
      <div class="flex flex-col gap-1">
        <label for="key">Custom Key (optional)</label>
        <InputText variant="filled" name="key" placeholder="Numbers, letters, or alphanumeric" type="text" fluid />
        <Message v-if="$form.key?.invalid" severity="error" size="small" variant="simple">
          {{ $form.key.error?.message }}
        </Message>
      </div>
      <div class="flex flex-col gap-1">
        <label for="sku">Product Sku (optional)</label>
        <InputText variant="filled" name="sku" placeholder="EAN, Code 39, Code 128, or others" type="text" fluid />
        <Message v-if="$form.sku?.invalid" severity="error" size="small" variant="simple">
          {{ $form.sku.error?.message }}
        </Message>
      </div>
      <div class="flex flex-col gap-1">
        <label for="name">Product Name</label>
        <InputText variant="filled" name="name" placeholder="Full product name" type="text" fluid />
        <Message v-if="$form.name?.invalid" severity="error" size="small" variant="simple">
          {{ $form.name.error?.message }}
        </Message>
      </div>
      <div class="flex flex-col gap-1">
        <label for="name">Product Size (with unit)</label>
        <InputText variant="filled" name="size" placeholder="Size in weight, volume, or pieces" type="text" fluid />
        <Message v-if="$form.size?.invalid" severity="error" size="small" variant="simple">
          {{ $form.size.error?.message }}
        </Message>
      </div>
      <div class="flex flex-col gap-1">
        <label for="classification">Classification</label>
        <Select name="classification" variant="filled" :options="availableClassifications"
          placeholder="Select classification" fluid />
        <Message v-if="$form.classification?.invalid" severity="error" size="small" variant="simple">
          {{ $form.classification.error?.message }}
        </Message>
      </div>
      <div class="flex flex-col gap-1">
        <label for="category">Categories (multiple, optional)</label>
        <MultiSelect name="category" variant="filled" :options="availableCategories" :maxSelectedLabels="3"
          placeholder="Select categories" fluid />
        <Message v-if="$form.category?.invalid" severity="error" size="small" variant="simple">
          {{ $form.category.error?.message }}
        </Message>
      </div>
    </div>
    <Button class="sm:ml-auto" type="submit" label="Submit">Save Product</Button>
  </Form>
</template>

<style scoped>

</style>