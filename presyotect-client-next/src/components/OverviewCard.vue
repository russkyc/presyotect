<script lang="ts" setup>

import {Card} from "primevue";

defineProps<{
  title: string,
  content: unknown,
  subContent: string,
}>();

</script>

<template>
  <Card class="rounded-lg">
    <template #content>
      <div class="flex">
        <div class="grow flex flex-col">
          <h6 class="font-medium text-md sm:text-lg">
            {{ title }}
          </h6>
          <h4 class="text-2xl sm:text-3xl font-bold">
            {{ content }}
          </h4>
          <p class="max-sm:hidden text-sm font-normal opacity-60">
            {{ subContent }}
          </p>
        </div>
        <div class="flex grow-0 mb-auto rounded-md bg-[--p-primary-color] text-[--p-primary-contrast-color] p-2">
          <slot name="icon" />
        </div>
      </div>
    </template>
  </Card>
</template>

<style scoped>

</style>