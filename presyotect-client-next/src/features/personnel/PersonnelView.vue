<script lang="ts" setup>
import {FilterMatchMode} from "@primevue/core";
import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import {
    Card,
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
import {PersonnelService} from "@/services/data/personnel-service.ts";
import type {BreadcrumbItem, Personnel} from "@/types/Interfaces.ts";

const breadcrumbs: BreadcrumbItem[] = [
    {label: "Personnel"}
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
const selectedPersonnel = ref();
const personnelCollection = ref();
const filters = ref({
    global: {value: null, matchMode: FilterMatchMode.CONTAINS},
    fullName: {value: null, matchMode: FilterMatchMode.STARTS_WITH}
});

onMounted(() => {
    PersonnelService.getPersonnel().then((response) => {
        personnelCollection.value = response;
    });
});

const onRowContextMenu = (event: DataTableRowContextMenuEvent) => {
    contextMenu.value.show(event.originalEvent);
};

const deletePersonnel = (personnel: Personnel) => {
    confirm.require({
        header: "Delete Personnel",
        message: `This will remove ${personnel.fullName} permanently. Are you sure?`,
        rejectProps: {
            label: "Cancel",
            severity: "secondary"
        },
        acceptProps: {
            label: "Delete",
            severity: "danger"
        },
        accept: async () => {
            if (personnel.id == null) return;
            const deleted = await PersonnelService.deletePersonnel(personnel.id);

            if (!deleted) {
                toast.add({
                    severity: "error",
                    summary: "Unsuccessful Deletion",
                    detail: `${personnel.fullName} cannot be deleted.`,
                    life: 3000
                });
                return;
            }

            toast.add({
                severity: "success",
                summary: "Personnel Deleted",
                detail: `Successfully removed ${personnel.fullName} from personnel.`,
                life: 3000
            });
            personnelCollection.value = personnelCollection.value.filter((p: Personnel) => p.id !== personnel.id);
            selectedPersonnel.value = null;
        }
    });
};

const breakpoints = useBreakpoints(breakpointsTailwind);
const isMobile = breakpoints.smallerOrEqual("sm");

const contextMenu = ref();
const menuModel = ref([
    {
        label: "Edit",
        icon: "pi pi-fw pi-pencil"
    },
    {
        label: "Delete",
        icon: "pi pi-fw pi-times",
        command: () => deletePersonnel(selectedPersonnel.value)
    }
]);

const addPersonnel = () => {
    router.push({path: "/personnel/add"});
};

</script>

<template>
  <Page
    :breadcrumbs="breadcrumbs"
    :show-drawer-toggle="true"
    subtitle="Manage personnel accounts"
    title="Personnel"
  >
    <template #content>
      <template v-if="!isMobile">
        <PageCard v-if="!isMobile">
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
                placeholder="Find Personnel"
                type="text"
                variant="filled"
              />
            </IconField>
          </template>
          <template #card-content>
            <ContextMenu
              ref="contextMenu"
              :model="menuModel"
              @hide="selectedPersonnel = null"
            />
            <DataTable
              v-model:context-menu-selection="selectedPersonnel"
              v-model:filters="filters"
              :global-filter-fields="['fullName', 'nickname', 'assignedEstablishments']"
              :rows="8"
              :rows-per-page-options="[8, 20, 50, 100]"
              :value="personnelCollection"
              context-menu
              paginator
              removable-sort
              table-style="min-width: 50rem"
              @row-contextmenu="onRowContextMenu"
            >
              <Column
                :sortable="true"
                class="w-[10%]"
                field="nickname"
                header="Nickname"
              />
              <Column
                :sortable="true"
                class="w-[20%]"
                field="fullName"
                header="Full Name"
              />
              <Column
                :sortable="true"
                class="w-[15%]"
                field="assignedEstablishments"
                header="Assigned Establishments"
              />
            </DataTable>
          </template>
          <template #card-actions>
            <SplitButton
              :model="items"
              @click="addPersonnel"
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
      <template v-if="isMobile">
        <div class="flex flex-col gap-4 grow">
          <h2 class="font-semibold text-lg">
            Personnel
          </h2>
          <Card class="grow rounded-lg" />
        </div>
      </template>
    </template>
  </Page>
</template>

<style scoped>

</style>