<script lang="ts" setup>

import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios from "axios";
import {Card} from "primevue";
import {onBeforeMount, ref} from "vue";
import OverviewCard from "@/components/OverviewCard.vue";
import Page from "@/components/Page.vue";

const iconSize = 27;
const totalProducts = ref(0);
const totalEstablishments = ref(0);
const axiosConfig = getAxiosConfig();

onBeforeMount(async () => {

    const productsResponse = await axios.get("/products/count", axiosConfig);
    totalProducts.value = productsResponse.data.content;

})

</script>

<template>
  <Page
    subtitle="Presyotect information overview"
    title="Dashboard"
  >
    <template #content>
      <div class="flex flex-col grow gap-4">
        <div class="grid grid-cols:1 md:grid-cols-2 xl:grid-cols-4 gap-4">
          <OverviewCard
            :content="totalProducts"
            sub-content="Monitored Products"
            title="Products"
          >
            <template #icon>
              <svg
                :height="iconSize"
                :width="iconSize"
                class="lucide lucide-package"
                fill="none"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M11 21.73a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73z"
                />
                <path d="M12 22V12" />
                <polyline points="3.29 7 12 12 20.71 7" />
                <path d="m7.5 4.27 9 5.15" />
              </svg>
            </template>
          </OverviewCard>
          <OverviewCard
            :content="totalEstablishments"
            sub-content="Active Establishments"
            title="Establishments"
          >
            <template #icon>
              <svg
                :height="iconSize"
                :width="iconSize"
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
            </template>
          </OverviewCard>
          <OverviewCard
            content="5"
            sub-content="Active Policies"
            title="Policies"
          >
            <template #icon>
              <svg
                :height="iconSize"
                :width="iconSize"
                class="lucide lucide-package"
                fill="none"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M11 21.73a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73z"
                />
                <path d="M12 22V12" />
                <polyline points="3.29 7 12 12 20.71 7" />
                <path d="m7.5 4.27 9 5.15" />
              </svg>
            </template>
          </OverviewCard>
          <OverviewCard
            content="1"
            sub-content="Active Tickets"
            title="Complaints"
          >
            <template #icon>
              <svg
                :height="iconSize"
                :width="iconSize"
                class="lucide lucide-package"
                fill="none"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M11 21.73a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73z"
                />
                <path d="M12 22V12" />
                <polyline points="3.29 7 12 12 20.71 7" />
                <path d="m7.5 4.27 9 5.15" />
              </svg>
            </template>
          </OverviewCard>
        </div>
        <Card class="rounded-lg grow" />
      </div>
    </template>
  </Page>
</template>

<style scoped>

</style>