<script lang="ts" setup>
import {FilterMatchMode} from "@primevue/core";
import {
    Chip,
    Column,
    ContextMenu,
    DataTable,
    type DataTableRowContextMenuEvent,
    IconField,
    InputIcon,
    InputText,
    SplitButton,
    useToast
} from "primevue";
import {useConfirm} from "primevue/useconfirm";
import {onMounted, ref} from "vue";
import Page from "@/components/Page.vue";
import PageCard from "@/components/PageCard.vue";
import router from "@/router.ts";
import {EstablishmentsService} from "@/services/data/establishments-service.ts";
import type {BreadcrumbItem, Establishment} from "@/types/Interfaces.ts";

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Establishments"}
];

const items = [
    {
        label: "Import",
        command: () => {
            // Import Command
        }
    }
];

const confirm = useConfirm();
const toast = useToast();
const selectedEstablishment = ref();
const establishments = ref();
const filters = ref({
    global: {value: null, matchMode: FilterMatchMode.CONTAINS},
    name: {value: null, matchMode: FilterMatchMode.STARTS_WITH}
});

onMounted(() => {
    EstablishmentsService.getEstablishments().then((response) => {
        establishments.value = response;
    });
});

const onRowContextMenu = (event: DataTableRowContextMenuEvent) => {
    contextMenu.value.show(event.originalEvent);
};

const deleteEstablishment = (establishment: Establishment) => {
    confirm.require({
        header: "Delete Establishment",
        message: `This will remove ${establishment.name} permanently. Are you sure?`,
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Delete",
            severity: "danger"
        },
        accept: async () => {
            if (establishment.id == null) return;
            const deleted = await EstablishmentsService.deleteEstablishment(establishment.id);

            if (!deleted) {
                toast.add({
                    severity: "error",
                    summary: "Unsuccessful Deletion",
                    detail: `${establishment.name} cannot be deleted.`,
                    life: 3000
                });
                return;
            }

            toast.add({
                severity: "success",
                summary: "Establishment Deleted",
                detail: `Successfully removed ${establishment.name} from establishments.`,
                life: 3000
            });
            establishments.value = establishments.value.filter((e: Establishment) => e.id !== establishment.id);
            selectedEstablishment.value = null;
        }
    });
};

const contextMenu = ref();
const menuModel = ref([
    {
        label: "Edit",
        icon: "pi pi-fw pi-pencil"
    },
    {
        label: "Delete",
        icon: "pi pi-fw pi-times",
        command: () => deleteEstablishment(selectedEstablishment.value)
    }
]);

const addEstablishment = () => {
    router.push({path: "/establishments/add"});
};

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    :show-drawer-toggle="true"
    subtitle="Manage monitored establishments"
    title="Establishments"
  >
    <template #content>
      <PageCard>
        <template #card-title>
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
              v-model="filters['global'].value"
              class="max-sm:grow md:w-80"
              fluid
              name="search"
              placeholder="Find Establishment"
              type="text"
              variant="filled"
            />
          </IconField>
        </template>
        <template #card-content>
          <ContextMenu
            ref="contextMenu"
            :model="menuModel"
            @hide="selectedEstablishment = null"
          />
          <DataTable
            v-model:context-menu-selection="selectedEstablishment"
            v-model:filters="filters"
            :global-filter-fields="['name', 'location', 'type']"
            :rows="8"
            :rows-per-page-options="[8, 20, 50, 100]"
            :value="establishments"
            context-menu
            paginator
            removable-sort
            table-style="min-width: 50rem"
            @row-contextmenu="onRowContextMenu"
          >
            <Column
              :sortable="true"
              class="w-[15%]"
              field="name"
              header="Name"
            />
            <Column
              :sortable="true"
              class="w-[10%]"
              field="cityMunicipality"
              header="City / Municipality"
            />
            <Column
              :sortable="true"
              class="w-[15%]"
              field="completeAddress"
              header="Full Address"
            />
            <Column
              :sortable="true"
              class="w-[5%]"
              field="categories"
              header="Categories"
            >
              <template #body="slotProps">
                <div
                  v-if="slotProps.data.categories && slotProps.data.categories.length > 0"
                  class="flex gap-2"
                >
                  <Chip
                    class="py-1 font-medium text-xs px-2 rounded-full text-[--p-primary-color] bg-[--p-highlight-background]"
                  >
                    {{ slotProps.data.categories[0] }}
                  </Chip>
                  <Chip
                    v-if="slotProps.data.categories.length > 1"
                    class="py-1 px-1.5 font-medium text-xs rounded-full  text-[--p-primary-color] bg-[--p-highlight-background]"
                  >
                    +{{ slotProps.data.categories.length - 1 }}
                  </Chip>
                </div>
              </template>
            </Column>
            <Column
              :sortable="true"
              class="w-[5%]"
              field="classifications"
              header="Classifications"
            >
              <template #body="slotProps">
                <div
                  v-if="slotProps.data.classifications && slotProps.data.classifications.length > 0"
                  class="flex gap-2"
                >
                  <Chip
                    class="py-1 font-medium text-xs px-2 rounded-full text-[--p-primary-color] bg-[--p-highlight-background]"
                  >
                    {{ slotProps.data.classifications[0] }}
                  </Chip>
                  <Chip
                    v-if="slotProps.data.classifications.length > 1"
                    class="py-1 px-1.5 font-medium text-xs rounded-full  text-[--p-primary-color] bg-[--p-highlight-background]"
                  >
                    +{{ slotProps.data.category.length - 1 }}
                  </Chip>
                </div>
              </template>
            </Column>
          </DataTable>
        </template>
        <template #card-actions>
          <SplitButton
            :model="items"
            @click="addEstablishment"
          >
            <div class="flex gap-2">
              <svg
                class="my-auto lucide lucide-plus max-sm:hidden"
                fill="none"
                height="20"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                viewBox="0 0 24 24"
                width="20"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path d="M5 12h14" />
                <path d="M12 5v14" />
              </svg>
              <p class="my-auto">
                Add
              </p>
            </div>
          </SplitButton>
        </template>
      </PageCard>
    </template>
  </Page>
</template>

<style scoped>

</style>