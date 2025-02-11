<script setup lang="ts">
import {TransitionExpand} from "@limonische/vue3-transition-expand";
import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {useAuthStore} from "@stores/auth-store.ts";
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {onClickOutside} from "@vueuse/core";
import {orderBy} from "natural-orderby";
import {Button, Card, InputNumber, InputText, Message} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {useToast} from "primevue/usetoast";
import {onMounted, ref, watch} from "vue";
import {z} from "zod";
import {MonitoringService} from "@/services/data/monitoring-service.ts";
import type {MonitoredPrice, Product} from "@/types/Interfaces.ts";

const props = defineProps<{
  product: Product
}>();

const confirm = useConfirm();
const toast = useToast();


const showDetails = ref(false);
const card = ref(null);
const lastMonitoredPrice = ref();
const productValue = ref(props.product);

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

const initialValues = ref<MonitoredPrice>({
    created: new Date(),
    price: null,
    remarks: null,
    status: null,
    personnelId: null,
    establishmentId: null
});

const resolver = ref(zodResolver(
    z.object({
        price: z.number().min(1, {message: "Price must be a positive number."}),
        remarks: z.string().nullable()
    })
));

onMounted(() => getLastMonitoredPrice());
watch(() => productValue, () => {
    getLastMonitoredPrice();
});

const getLastMonitoredPrice = () => {
    const monitoringStore = useMonitoringStore();
    const monitoredPrices = productValue.value.monitoredPrices?.filter(mp => mp.establishmentId === monitoringStore.activeEstablishment?.id) ?? [];
    const orderedMonitoredPrices = orderBy<MonitoredPrice>(monitoredPrices, [mp => mp.created], ["desc"]);
    if (orderedMonitoredPrices.length > 0) {
        lastMonitoredPrice.value = orderedMonitoredPrices[0].price;
        console.log(lastMonitoredPrice.value);
    }
};

const onFormSubmit = async (form: FormSubmitEvent) => {
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Invalid Price",
            detail: "Please enter a valid price.",
            life: 2000
        });
        return;
    }
    confirm.require({
        message: "Are you sure you want to set the new price?",
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Update"
        },
        accept: async () => {
            const authStore = useAuthStore();
            const monitoringStore = useMonitoringStore();
            const monitoredPrice = form.values as MonitoredPrice;

            monitoredPrice.created = new Date();
            monitoredPrice.productId = props.product.id;
            monitoredPrice.price = monitoredPrice.price!;
            monitoredPrice.personnelId = authStore.userClaims?.nameid;
            monitoredPrice.establishmentId = monitoringStore.activeEstablishment?.id;

            initialValues.value.created = new Date();
          
            const isOnline = await monitoringStore.isOnline;

            if (!isOnline) {
                monitoringStore.pendingMonitoredPrices.push(monitoredPrice);
                productValue.value.monitoredPrices?.push(monitoredPrice);
                showDetails.value = false;
                getLastMonitoredPrice();
                toast.add({
                    severity: "success",
                    summary: "Price Update Cached",
                    detail: "Updates will sync automatically.",
                    life: 2000
                });
            } else {
                try {
                    const response = await MonitoringService.postMonitoredPrice(monitoredPrice);
                    if (!response) {
                        throw new Error("Failed to update price online");
                    }
                    productValue.value.monitoredPrices?.push(monitoredPrice);
                    showDetails.value = false;
                    getLastMonitoredPrice();
                    toast.add({
                        severity: "success",
                        summary: "Price Updated",
                        detail: "Price updated successfully.",
                        life: 2000
                    });
                } catch {
                    monitoringStore.pendingMonitoredPrices.push(monitoredPrice);
                    productValue.value.monitoredPrices?.push(monitoredPrice);
                    showDetails.value = false;
                    getLastMonitoredPrice();
                    toast.add({
                        severity: "error",
                        summary: "Price Not Updated Online",
                        detail: "An error occurred while updating the price online. The update has been cached.",
                        life: 2000
                    });
                }
            }
        }
    });
};
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
              {{ productValue.name }}
            </p>
            <p class="truncate text-sm font-semibold opacity-60">
              {{ productValue.size }}
            </p>
          </div>
          <p class="font-bold text-md">
            {{ lastMonitoredPrice ? formatter.format(lastMonitoredPrice) : "No data" }}
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
                {{ productValue.sku ?? "none" }}
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
            <Form
              :initial-values="initialValues"
              :resolver="resolver"
              v-slot="$form"
              class="flex flex-col gap-4 mt-3"
              @submit="onFormSubmit"
            >
              <div class="flex flex-col gap-2">
                <div class="flex-flex-col gap-1">
                  <label for="price">Current Price</label>
                  <InputNumber
                    autofocus
                    fluid
                    mode="currency"
                    currency="PHP"
                    locale="en-US"
                    name="price"
                    placeholder="0.00"
                    type="number"
                    variant="filled"
                  />
                  <Message
                    v-if="$form.price?.invalid"
                    severity="error"
                    size="small"
                    variant="simple"
                  >
                    {{ $form.price.error?.message }}
                  </Message>
                </div>
                <div class="flex-flex-col gap-1">
                  <label for="remarks">Remarks (optional)</label>
                  <InputText
                    fluid
                    name="remarks"
                    placeholder="Remarks"
                    type="text"
                    variant="filled"
                  />
                  <Message
                    v-if="$form.remarks?.invalid"
                    severity="error"
                    size="small"
                    variant="simple"
                  >
                    {{ $form.remarks.error?.message }}
                  </Message>
                </div>
              </div>
              <Button
                label="Submit"
                type="submit"
              >
                Set Current Price
              </Button>
            </Form>
          </div>
        </TransitionExpand>
      </div>
    </template>
  </Card>
</template>

<style scoped>
</style>