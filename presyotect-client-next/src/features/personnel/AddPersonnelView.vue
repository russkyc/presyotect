<script lang="ts" setup>
import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {EstablishmentsService} from "@services/data/establishments-service.ts";
import {useActionsStore} from "@stores/actions-store";
import {Button, InputText, Message, MultiSelect, Password} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {useToast} from "primevue/usetoast";
import {onMounted, ref} from "vue";
import {z} from "zod";
import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import {PersonnelService} from "@/services/data/personnel-service.ts";
import type {BreadcrumbItem, Personnel} from "@/types/Interfaces.ts";

const toast = useToast();
const confirm = useConfirm();
const actionsStore = useActionsStore();
const availableEstablishments = ref();

const initialValues = ref<Personnel>({
    password: "", username: "",
    id: "",
    nickname: "",
    fullName: "",
    assignedEstablishments: []
});

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Personnel", url: "/personnel"},
    {label: "Add"}
];

const resolver = ref(zodResolver(
    z.object({
        nickname: z.string().nullable(),
        fullName: z.string().min(1, {message: "Full name is required."}),
        username: z.string().min(1, {message: "Username is required."}),
        password: z.string().min(1, {message: "Password is required."}),
        assignedEstablishments: z.array(z.string()).nullable()
    })
));

onMounted(async () => {
    const establishments = await EstablishmentsService.getEstablishments();
    availableEstablishments.value = establishments.map(e => ({label: e.name, value: e.id}));
    actionsStore.addPendingActions();
});

const onFormSubmit = async (form: FormSubmitEvent) => {
    console.log(form.values);
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Incomplete Personnel Details",
            detail: "Please fill-out all required fields.",
            life: 2000
        });
        return;
    }
    confirm.require({
        message: `Are you sure you want to add ${form.values.fullName} to the personnel list?`,
        header: "Confirmation",
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Add"
        },
        accept: async () => {

            const personnel = form.values as Personnel;
            const response = await PersonnelService.addPersonnel(personnel);
            if (!response) {
                toast.add({
                    severity: "error",
                    summary: "Personnel Not Added",
                    detail: `An error occurred while adding ${form.values.fullName} to personnel.`,
                    life: 2000
                });
                return;
            }
            toast.add({
                severity: "success",
                summary: "Personnel Added",
                detail: `${form.values.fullName} added to personnel.`,
                life: 2000
            });
            actionsStore.clearPendingActions();
            await router.push({path: "/personnel"});
        }
    });
};

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    subtitle="Add new personnel"
    title="Personnel"
  >
    <template #content>
      <PageCard>
        <template #card-title>
          <h3 class="text-lg font-medium">
            Personnel Details
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
                <label for="nickname">Preferred Name / Nickname</label>
                <InputText
                  fluid
                  name="nickname"
                  placeholder="Preferred personnel name or nickname, if any"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.nickname?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.nickname.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="fullName">Full Name</label>
                <InputText
                  fluid
                  name="fullName"
                  placeholder="Full personnel name"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.fullName?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.fullName.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="username">Username</label>
                <InputText
                  fluid
                  name="username"
                  placeholder="a unique username"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.username?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.username.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="password">Password</label>
                <Password
                  fluid
                  name="password"
                  placeholder="A secure and strong password"
                  type="text"
                  variant="filled"
                  toggle-mask
                />
                <Message
                  v-if="$form.password?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.password.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="assignedEstablishments">Assigned Establishments (multiple, optional)</label>
                <MultiSelect
                  :max-selected-labels="3"
                  :options="availableEstablishments"
                  option-value="value"
                  option-label="label"
                  fluid
                  name="assignedEstablishments"
                  placeholder="Select establishments"
                  filter
                  variant="filled"
                />
                <Message
                  v-if="$form.assignedEstablishments?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.assignedEstablishments.error?.message }}
                </Message>
              </div>
            </div>
            <Button
              class="sm:ml-auto"
              label="Submit"
              type="submit"
            >
              Save Personnel
            </Button>
          </Form>
        </template>
      </PageCard>
    </template>
  </Page>
</template>

<style scoped>

</style>
