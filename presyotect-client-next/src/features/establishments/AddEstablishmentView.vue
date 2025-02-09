<script lang="ts" setup>
import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {useActionsStore} from "@stores/actions-store";
import {Button, InputText, Message, Select} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {useToast} from "primevue/usetoast";
import {onMounted, ref} from "vue";
import {z} from "zod";
import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import {EstablishmentsService} from "@/services/data/establishments-service.ts";
import type {BreadcrumbItem, Establishment} from "@/types/Interfaces.ts";

const toast = useToast();
const confirm = useConfirm();
const actionsStore = useActionsStore();

const initialValues = ref<Establishment>({
    name: "",
    location: "",
    type: ""
});

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Establishments", url: "/establishments"},
    {label: "Add"}
];

const availableTypes = ref([
    "Restaurant",
    "Store",
    "Warehouse",
    "Office"
]);

const resolver = ref(zodResolver(
    z.object({
        name: z.string().min(1, {message: "Establishment name is required."}),
        location: z.string().min(1, {message: "Location is required."}),
        type: z.string().min(1, {message: "Type is required."})
    })
));

onMounted(() => {
    actionsStore.addPendingActions();
});

const onFormSubmit = async (form: FormSubmitEvent) => {
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Incomplete Establishment Details",
            detail: "Please fill-out all required fields.",
            life: 2000
        });
        return;
    }
    confirm.require({
        message: `Are you sure you want to add ${form.values.name} to the establishments list?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Add"
        },
        accept: async () => {
            const establishment = {
                name: form.values.name,
                location: form.values.location,
                type: form.values.type
            };
            const response = await EstablishmentsService.addEstablishment(establishment);
            if (!response) {
                toast.add({
                    severity: "error",
                    summary: "Establishment Not Added",
                    detail: `An error occurred while adding ${form.values.name} to establishments.`,
                    life: 2000
                });
                return;
            }
            toast.add({
                severity: "success",
                summary: "Establishment Added",
                detail: `${form.values.name} added to establishments.`,
                life: 2000
            });
            actionsStore.clearPendingActions();
            await router.push({path: "/establishments"});
        }
    });
};
</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="Add new establishment"
    title="Establishments"
  >
    <template #content>
      <PageCard>
        <template #card-title>
          <h3 class="text-lg font-medium">
            Establishment Details
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
                <label for="name">Establishment Name</label>
                <InputText
                  fluid
                  name="name"
                  placeholder="Full establishment name"
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
                <label for="location">Location</label>
                <InputText
                  fluid
                  name="location"
                  placeholder="Location of the establishment"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.location?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.location.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="type">Type</label>
                <Select
                  :options="availableTypes"
                  fluid
                  name="type"
                  placeholder="Select type"
                  variant="filled"
                />
                <Message
                  v-if="$form.type?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.type.error?.message }}
                </Message>
              </div>
            </div>
            <Button
              class="sm:ml-auto"
              label="Submit"
              type="submit"
            >
              Save Establishment
            </Button>
          </Form>
        </template>
      </PageCard>
    </template>
  </Page>
</template>

<style scoped>

</style>
