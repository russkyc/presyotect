<script setup lang="ts">

import {TransitionExpand} from "@limonische/vue3-transition-expand";
import {Button, Card} from "primevue";
import {ref} from "vue";
import type {Product} from "@/types/Interfaces.ts";

defineProps<{
  product: Product
}>();

const showDetails = ref(false);

const toggleShow = () => {
    showDetails.value = !showDetails.value;
};

const formatter = new Intl.NumberFormat("default", {
    style: "currency",
    currency: "PHP",
});

</script>

<template>
  <Card
    class="rounded-lg [&>.p-card-body]:p-4"
  >
    <template #content>
      <div class="flex flex-col">
        <div
          class="flex gap-4"
          @click="toggleShow"
        >
          <div class="flex flex-col grow">
            <p class="truncate font-semibold leading-none">
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
                {{ product.sku }}
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
              <Button
                label="Submit"
                type="submit"
              >
                Set Price Movement
              </Button>
              <Button
                label="Submit"
                type="submit"
                variant="outlined"
              >
                No Price Movement
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