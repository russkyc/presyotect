<script lang="ts" setup>
import {EstablishmentsService} from "@services/data/establishments-service.ts";
import {ProductsService} from "@services/data/products-service.ts";
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {breakpointsTailwind, useBreakpoints, watchDebounced} from "@vueuse/core";
import {Button, Card, IconField, InputIcon, InputText} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {onMounted, ref} from "vue";
import Page from "@/components/Page.vue";
import type {BreadcrumbItem, Establishment, Product} from "@/types/Interfaces.ts";

const confirm = useConfirm();
const monitoringStore = useMonitoringStore();
const breakpoints = useBreakpoints(breakpointsTailwind);
const isMobile = breakpoints.smallerOrEqual("sm");
const breadcrumbs: BreadcrumbItem[] = [
    {label: "Price Monitoring"}
];

const filter = ref<string>("");

const establishments = ref();
const filteredEstablishments = ref();

const products = ref();
const fiteredProducts = ref();

onMounted(() => {
    if (monitoringStore.activeEstablishment) {
        ProductsService.getProducts().then((response) => {
            products.value = response;
            fiteredProducts.value = response;
        });
    } else {
        EstablishmentsService.getEstablishments().then((response) => {
            establishments.value = response;
            filteredEstablishments.value = response;
        });
    }
});

watchDebounced(filter, (value) => {
    if (value && value.length > 0) {
        if (monitoringStore.activeEstablishment) {
            fiteredProducts.value = products.value.filter((product:Product) => product.name.toLowerCase().includes(value.toLowerCase()));
        } else {
            filteredEstablishments.value = establishments.value.filter((establishment:Establishment) => establishment.name.toLowerCase().includes(value.toLowerCase()));
        }
    } else {
        if (monitoringStore.activeEstablishment) {
            fiteredProducts.value = products.value;
        } else {
            filteredEstablishments.value = establishments.value;
        }
    }
}, {debounce: 300});

const startMonitoring = (establishment: Establishment) => {
    confirm.require({
        message: `Start monitoring ${establishment.name}?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Start"
        },
        accept: async () => {
            monitoringStore.activeEstablishment = establishment;
            ProductsService.getProducts().then((response) => {
                products.value = response;
                fiteredProducts.value = response;
            });
        }
    });
    
};

const endMonitoring = () => {
    confirm.require({
        message: `End monitoring ${monitoringStore.activeEstablishment!.name}?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "End"
        },
        accept: async () => {
            monitoringStore.activeEstablishment = null;
            EstablishmentsService.getEstablishments().then((response) => {
                establishments.value = response;
                filteredEstablishments.value = response;
            });
        }
    });
    
};

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="View latest recorded prices and product price movements"
    title="Price Monitoring"
  >
    <template #content-header>
      <template v-if="monitoringStore.activeEstablishment && isMobile">
        <h2 class="font-semibold text-lg">
          Currently Monitoring
        </h2>
        <Card
          v-if="monitoringStore.activeEstablishment"
          class="rounded-lg [&>.p-card-body]:px-4 [&>.p-card-body]:py-3"
          @click="endMonitoring()"
        >
          <template #content>
            <div class="flex flex-row gap-3">
              <div class="flex flex-col overflow-hidden grow">
                <p class="truncate font-semibold">
                  {{ monitoringStore.activeEstablishment.name }}
                </p>
                <p class="truncate text-xs opacity-60">
                  {{ monitoringStore.activeEstablishment.categories[0] }}
                </p>
              </div>
              <div class="flex grow-0 rounded-md bg-[--p-primary-color] text-[--p-primary-contrast-color] p-2">
                <svg
                  :height="24"
                  :width="24"
                  class="lucide lucide-store"
                  fill="none"
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  viewBox="0 0 24 24"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path d="m2 7 4.41-4.41A2 2 0 0 1 7.83 2h8.34a2 2 0 0 1 1.42.59L22 7" />
                  <path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8" />
                  <path d="M15 22v-4a2 2 0 0 0-2-2h-2a2 2 0 0 0-2 2v4" />
                  <path d="M2 7h20" />
                  <path
                    d="M22 7v3a2 2 0 0 1-2 2a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 16 12a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 12 12a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 8 12a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 4 12a2 2 0 0 1-2-2V7"
                  />
                </svg>
              </div>
            </div>
          </template>
        </Card>
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
            v-model="filter"
            class="max-sm:grow md:w-80"
            fluid
            name="search"
            placeholder="Find Product"
            type="text"
            variant="filled"
          />
        </IconField>
        <Button
          class="sm:ml-auto shrink-0 flex"
          size="small"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="14"
            height="14"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
            class="lucide lucide-scan-barcode my-auto shrink-0"
          ><path d="M3 7V5a2 2 0 0 1 2-2h2" /><path d="M17 3h2a2 2 0 0 1 2 2v2" /><path d="M21 17v2a2 2 0 0 1-2 2h-2" /><path d="M7 21H5a2 2 0 0 1-2-2v-2" /><path d="M8 7v10" /><path d="M12 7v10" /><path d="M17 7v10" /></svg>
          <p class="font-medium my-auto truncate">
            Barcode Scanner
          </p>
        </Button>
      </template>
      <template v-else-if="!monitoringStore.activeEstablishment && isMobile">
        <h2 class="font-semibold text-lg">
          Assigned Establishments
        </h2>
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
            v-model="filter"
            class="max-sm:grow md:w-80"
            fluid
            name="search"
            placeholder="Find Establishment"
            type="text"
            variant="filled"
          />
        </IconField>
      </template>
    </template>
    <template #content>
      <template v-if="!isMobile">
        <Card class="grow rounded-lg" />
      </template>
      <template v-if="isMobile">
        <div class="flex flex-col gap-3 grow">
          <template v-if="monitoringStore.activeEstablishment">
            <template
              v-for="product in fiteredProducts"
              :key="product.id"
            >
              <Card class="rounded-lg [&>.p-card-body]:px-4 [&>.p-card-body]:py-3">
                <template #content>
                  <div class="flex gap-4">
                    <div class="flex flex-col grow">
                      <p class="truncate font-semibold">
                        {{ product.name }}
                      </p>
                      <p class="truncate text-sm font-semibold opacity-60">
                        {{ product.size }}
                      </p>
                    </div>
                    <p class="font-bold text-md">
                      ₱0.00
                    </p>
                  </div>
                </template>
              </Card>
            </template>
          </template>
          <template v-else>
            <template
              v-for="establishment in filteredEstablishments"
              :key="establishment.id"
            >
              <Card
                class="rounded-lg [&>.p-card-body]:p-3"
                @click="startMonitoring(establishment)"
              >
                <template #content>
                  <div class="flex flex-row gap-3">
                    <div class="flex grow flex-col mb-auto overflow-hidden">
                      <p class="truncate font-semibold">
                        {{ establishment.name }}
                      </p>
                      <p class="truncate font-semibold text-xs opacity-60">
                        {{ establishment.completeAddress }}
                      </p>
                      <p class="truncate mt-4 opacity-60 mr-auto text-xs font-medium rounded-full">
                        {{ establishment.categories[0] }}
                      </p>
                    </div>
                    <div class="flex grow-0 mb-auto rounded-md bg-[--p-primary-color] text-[--p-primary-contrast-color] p-2">
                      <svg
                        :height="24"
                        :width="24"
                        class="lucide lucide-store"
                        fill="none"
                        stroke="currentColor"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        viewBox="0 0 24 24"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path d="m2 7 4.41-4.41A2 2 0 0 1 7.83 2h8.34a2 2 0 0 1 1.42.59L22 7" />
                        <path d="M4 12v8a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-8" />
                        <path d="M15 22v-4a2 2 0 0 0-2-2h-2a2 2 0 0 0-2 2v4" />
                        <path d="M2 7h20" />
                        <path
                          d="M22 7v3a2 2 0 0 1-2 2a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 16 12a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 12 12a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 8 12a2.7 2.7 0 0 1-1.59-.63.7.7 0 0 0-.82 0A2.7 2.7 0 0 1 4 12a2 2 0 0 1-2-2V7"
                        />
                      </svg>
                    </div>
                  </div>
                </template>
              </Card>
            </template>
          </template>
        </div>
      </template>
    </template>
  </Page>
</template>

<style scoped>

</style>