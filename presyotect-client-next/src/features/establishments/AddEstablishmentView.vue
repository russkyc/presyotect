<script lang="ts" setup>
import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {AddressService} from "@services/data/address-service.ts";
import {useActionsStore} from "@stores/actions-store";
import {Button, InputText, Message, MultiSelect, Select} from "primevue";
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
    status: "",
    cityMunicipality: "",
    completeAddress: "",
    categories: [],
    classifications: [],
    subClassifications: [],
    tags: [],
    owners: [],
    ownershipType: "",
    contactPerson: "",
    designation: "",
    website: "",
    contactNumber: "",
    email: "",
    socials: {}
});

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Establishments", url: "/establishments"},
    {label: "Add"}
];

const availableCategories = ref(["Category1", "Category2", "Category3"]);
const availableClassifications = ref(["Classification1", "Classification2"]);
const availableSubClassifications = ref(["SubClassification1", "SubClassification2"]);
const availableTags = ref(["Tag1", "Tag2", "Tag3"]);
const availableStatusTypes = ref(["Closed", "Included", "Unverified"]);
const availableOwnershipTypes = ref(["Type1", "Type2"]);

const resolver = ref(zodResolver(
    z.object({
        name: z.string().min(1, {message: "Establishment name is required."}),
        status: z.string().min(1, {message: "Status is required."}),
        cityMunicipality: z.string().min(1, {message: "City/Municipality is required."}),
        completeAddress: z.string().min(1, {message: "Complete address is required."}),
        categories: z.array(z.string().nullable()).min(1, {message: "At least one category is required."}),
        classifications: z.array(z.string()).min(1, {message: "At least one classification is required."}),
        subClassifications: z.array(z.string()).nullable(),
        tags: z.array(z.string()).nullable(),
        owners: z.array(z.string()).nullable(),
        ownershipType: z.string().optional(),
        contactPerson: z.string().min(1, {message: "Contact person is required."}),
        designation: z.string().optional(),
        website: z.string().optional(),
        contactNumber: z.string().optional(),
        email: z.string().optional(),
        socials: z.record(z.string()).optional()
    })
));

const citiesMunicipalities = ref();

onMounted(async () => {
    const addressDataCityMunicipality = await AddressService.GetCitiesMunicipalitiesByProvince("050500000");
    citiesMunicipalities.value = addressDataCityMunicipality.map((cityMunicipality) => cityMunicipality.name);
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
            const establishment = form.values as Establishment;
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
                <label for="cityMunicipality">City / Municipality</label>
                <Select
                  :options="citiesMunicipalities"
                  fluid
                  name="cityMunicipality"
                  placeholder="Select City or Municipality"
                  variant="filled"
                />
                <Message
                  v-if="$form.cityMunicipality?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.cityMunicipality.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="completeAddress">Complete Address</label>
                <InputText
                  fluid
                  name="completeAddress"
                  placeholder="Complete address"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.completeAddress?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.completeAddress.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="categories">Categories</label>
                <MultiSelect
                  :options="availableCategories"
                  fluid
                  name="categories"
                  placeholder="Select categories"
                  variant="filled"
                />
                <Message
                  v-if="$form.categories?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.categories.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="classifications">Classifications</label>
                <MultiSelect
                  :options="availableClassifications"
                  fluid
                  name="classifications"
                  placeholder="Select classifications"
                  variant="filled"
                />
                <Message
                  v-if="$form.classifications?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.classifications.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="subClassifications">Sub-Classifications</label>
                <MultiSelect
                  :options="availableSubClassifications"
                  fluid
                  name="subClassifications"
                  placeholder="Select sub-classifications"
                  variant="filled"
                />
                <Message
                  v-if="$form.subClassifications?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.subClassifications.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="status">Status</label>
                <Select
                  :options="availableStatusTypes"
                  fluid
                  name="status"
                  placeholder="Select Status"
                  variant="filled"
                />
                <Message
                  v-if="$form.status?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.status.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="tags">Tags</label>
                <MultiSelect
                  :options="availableTags"
                  fluid
                  name="tags"
                  placeholder="Select tags"
                  variant="filled"
                />
                <Message
                  v-if="$form.tags?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.tags.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <h3 class="text-lg font-medium">
                  Ownership
                </h3>
              </div>
              <div class="flex flex-col gap-1">
                <label for="owners">Owners</label>
                <MultiSelect
                  fluid
                  name="owners"
                  placeholder="Enter owners"
                  variant="filled"
                />
                <Message
                  v-if="$form.owners?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.owners.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="ownershipType">Ownership Type</label>
                <Select
                  :options="availableOwnershipTypes"
                  fluid
                  name="ownershipType"
                  placeholder="Select ownership type"
                  variant="filled"
                />
                <Message
                  v-if="$form.ownershipType?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.ownershipType.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <h3 class="text-lg font-medium">
                  Contact Information
                </h3>
              </div>
              <div class="flex flex-col gap-1">
                <label for="contactPerson">Contact Person</label>
                <InputText
                  fluid
                  name="contactPerson"
                  placeholder="Contact person"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.contactPerson?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.contactPerson.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="designation">Designation</label>
                <InputText
                  fluid
                  name="designation"
                  placeholder="Designation"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.designation?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.designation.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="website">Website</label>
                <InputText
                  fluid
                  name="website"
                  placeholder="Website"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.website?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.website.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="contactNumber">Contact Number</label>
                <InputText
                  fluid
                  name="contactNumber"
                  placeholder="Contact number"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.contactNumber?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.contactNumber.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="email">Email</label>
                <InputText
                  fluid
                  name="email"
                  placeholder="Email"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.email?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.email.error?.message }}
                </Message>
              </div>
              <div class="flex flex-col gap-1">
                <label for="socials">Socials</label>
                <InputText
                  fluid
                  name="socials"
                  placeholder="Social media links"
                  type="text"
                  variant="filled"
                />
                <Message
                  v-if="$form.socials?.invalid"
                  severity="error"
                  size="small"
                  variant="simple"
                >
                  {{ $form.socials.error?.message }}
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
