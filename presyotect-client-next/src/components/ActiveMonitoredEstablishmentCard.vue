<script setup lang="ts">

import {TransitionExpand} from "@limonische/vue3-transition-expand";
import {useMonitoringStore} from "@stores/monitoring-store.ts";
import {onClickOutside} from "@vueuse/core";
import {Button, Card} from "primevue";
import {ref} from "vue";

defineProps<{
  endMonitoring: () => void;
}>();

const monitoringStore = useMonitoringStore();
const showDetails = ref(false);
const reference = ref(null);

const toggleShow = () => {
    showDetails.value = !showDetails.value;
};

onClickOutside(reference, () => {
    showDetails.value = false;
});

</script>

<template>
  <Card
    ref="reference"
    v-if="monitoringStore.activeEstablishment"
    class="rounded-lg [&>.p-card-body]:p-4"
    @click="toggleShow"
  >
    <template #content>
      <div class="flex flex-col">
        <div class="flex flex-row gap-3">
          <div class="flex flex-col overflow-hidden grow my-auto">
            <p class="truncate font-semibold leading-none">
              {{ monitoringStore.activeEstablishment!.name }}
            </p>
            <p class="truncate text-xs opacity-60">
              {{ monitoringStore.activeEstablishment!.categories[0] }}
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
        <TransitionExpand :duration="320">
          <div
            class="flex flex-col mt-3"
            v-if="showDetails"
          >
            <div class="flex gap-4">
              <p class="grow truncate">
                Products Completed
              </p>
              <p class="font-bold">
                0
              </p>
            </div>
            <div class="flex gap-4">
              <p class="grow truncate">
                Pending Monitoring
              </p>
              <p class="font-bold">
                0
              </p>
            </div>
            <div class="flex flex-col gap-3 mt-3">
              <Button @click="endMonitoring">
                End Monitoring
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