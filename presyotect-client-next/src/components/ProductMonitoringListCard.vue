<script setup lang="ts">

import {TransitionExpand} from "@limonische/vue3-transition-expand";
import {onClickOutside} from "@vueuse/core";
import {Button, Card, IconField, InputIcon, InputText} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {ref} from "vue";
import type {Product} from "@/types/Interfaces.ts";

defineProps<{
  product: Product
}>();

const confirm = useConfirm();
const showDetails = ref(false);
const card = ref(null);

onClickOutside(card, () => {
    showDetails.value = false;
});

const toggleShow = () => {
    showDetails.value = !showDetails.value;
};

const formatter = new Intl.NumberFormat("default", {
    style: "currency",
    currency: "PHP",
});

const setCurrentPrice = () => {
    confirm.require({
        message: "Are you sure you want to update the price?",
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Update"
        },
        accept: async () => {
            showDetails.value = false;
        }
    });
}

</script>

<template>
  <Card
    ref="card"
    class="rounded-lg [&>.p-card-body]:p-4 [&>.p-card-body]:pt-3"
  >
    <template #content>
      <div class="flex flex-col">
        <div
          class="flex gap-4"
          @click="toggleShow"
        >
          <div class="flex flex-col grow">
            <p class="truncate font-semibold leading-normal">
              {{ product.name }}
            </p>
            <p class="truncate text-sm font-semibold opacity-60">
              {{ product.size }}
            </p>
          </div>
          <p class="font-bold text-md">
            {{ formatter.format(0) }}
          </p>
        </div>
        <TransitionExpand :duration="320">
          <div
            class="flex flex-col mt-3"
            v-if="showDetails"
          >
            <div class="flex gap-4">
              <p class="grow truncate">
                SKU / Barcode
              </p>
              <p class="font-semibold">
                {{ product.sku ?? "none" }}
              </p>
            </div>
            <div class="flex gap-4">
              <p class="grow truncate">
                Suggested Retail Price
              </p>
              <p class="font-semibold">
                {{ formatter.format(0) }}
              </p>
            </div>
            <div class="flex gap-4">
              <p class="grow truncate">
                Prevailing Price
              </p>
              <p class="font-semibold">
                {{ formatter.format(0) }}
              </p>
            </div>
            <div class="flex flex-col gap-3 mt-3">
              <div class="flex-flex-col gap-1">
                <IconField
                  class="[&>.p-inputtext:not(:first-child)]:ps-7"
                >
                  <InputIcon>
                    <p>₱</p>
                  </InputIcon>
                  <InputText
                    autofocus
                    fluid
                    name="currentPrice"
                    placeholder="0.00"
                    type="text"
                    variant="filled"
                  />
                </IconField>
              </div>
              <Button
                label="Submit"
                type="submit"
                @click="setCurrentPrice"
              >
                Set Current Price
              </Button>
              <Button
                label="Submit"
                type="submit"
                variant="outlined"
              >
                Set No Movement
              </Button>
            </div>
          </div>
        </TransitionExpand>
      </div>
    </template>
  </Card>
</template>

<style scoped>

</style>