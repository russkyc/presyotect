<script lang="ts" setup>
import {EstablishmentsService} from "@services/data/establishments-service.ts";
import {ProductsService} from "@services/data/products-service.ts";
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {breakpointsTailwind, useBreakpoints, watchDebounced} from "@vueuse/core";
import {Card, IconField, InputIcon, InputText} from "primevue";
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
            {{ monitoringStore.activeEstablishment.name }}
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
                  {{ product.name }}
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
                class="rounded-lg [&>.p-card-body]:px-4 [&>.p-card-body]:py-3"
                @click="startMonitoring(establishment)"
              >
                <template #content>
                  {{ establishment.name }}
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