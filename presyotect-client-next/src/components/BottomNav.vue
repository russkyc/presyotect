<script lang="ts" setup>

import {breakpointsTailwind, useBreakpoints} from "@vueuse/core";
import {Toolbar} from "primevue";
import Authorize from "@/components/dynamic/Authorize.vue";
import NavLink from "@/components/NavLink.vue";
import {Roles} from "@/types/Constants.ts";

const breakpoints = useBreakpoints(breakpointsTailwind)
const mdAndSmaller = breakpoints.smallerOrEqual("lg");
const iconSize = 20;

</script>

<template>
  <Toolbar class="col-span-2 px-3 py-2 border-b rounded-none">
    <template #center>
      <div class="flex gap-3">
        <NavLink href="/">
          <template #icon>
            <svg
              :height="iconSize"
              :width="iconSize"
              class="lucide lucide-layout-dashboard shrink-0"
              fill="none"
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <rect
                height="9"
                rx="1"
                width="7"
                x="3"
                y="3"
              />
              <rect
                height="5"
                rx="1"
                width="7"
                x="14"
                y="3"
              />
              <rect
                height="9"
                rx="1"
                width="7"
                x="14"
                y="12"
              />
              <rect
                height="5"
                rx="1"
                width="7"
                x="3"
                y="16"
              />
            </svg>
          </template>
        </NavLink>
        <NavLink href="/price-monitoring">
          <template #icon>
            <svg
              :height="iconSize"
              :width="iconSize"
              class="lucide lucide-trending-up-down"
              fill="none"
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path d="M14.828 14.828 21 21" />
              <path d="M21 16v5h-5" />
              <path d="m21 3-9 9-4-4-6 6" />
              <path d="M21 8V3h-5" />
            </svg>
          </template>
        </NavLink>
        <NavLink href="/products">
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
        </NavLink>
        <NavLink href="/establishments">
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
        </NavLink>
        <Authorize
          v-if="!mdAndSmaller"
          :roles="[Roles.Admin]"
        >
          <NavLink href="/users">
            <template #icon>
              <svg
                :height="iconSize"
                :width="iconSize"
                class="lucide lucide-users"
                fill="none"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" />
                <circle
                  cx="9"
                  cy="7"
                  r="4"
                />
                <path d="M22 21v-2a4 4 0 0 0-3-3.87" />
                <path d="M16 3.13a4 4 0 0 1 0 7.75" />
              </svg>
            </template>
          </NavLink>
        </Authorize>
        <Authorize
          v-if="!mdAndSmaller"
          :roles="[Roles.Admin]"
        >
          <NavLink href="/analytics">
            <template #icon>
              <svg
                :height="iconSize"
                :width="iconSize"
                class="lucide lucide-chart-bar-stacked"
                fill="none"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path d="M11 13v4" />
                <path d="M15 5v4" />
                <path d="M3 3v16a2 2 0 0 0 2 2h16" />
                <rect
                  height="4"
                  rx="1"
                  width="9"
                  x="7"
                  y="13"
                />
                <rect
                  height="4"
                  rx="1"
                  width="12"
                  x="7"
                  y="5"
                />
              </svg>
            </template>
          </NavLink>
        </Authorize>
        <NavLink href="/settings">
          <template #icon>
            <svg
              :height="iconSize"
              :width="iconSize"
              class="lucide lucide-settings-2"
              fill="none"
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path d="M20 7h-9" />
              <path d="M14 17H5" />
              <circle
                cx="17"
                cy="17"
                r="3"
              />
              <circle
                cx="7"
                cy="7"
                r="3"
              />
            </svg>
          </template>
        </NavLink>
      </div>
    </template>
  </Toolbar>
</template>

<style scoped>

</style>