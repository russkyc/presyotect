<script setup lang="ts">

import {Card} from "primevue";
import {defineProps} from "vue";

defineProps<{
  title: string,
  content: any,
  subContent: string,
}>();

</script>

<template>
  <Card class="rounded-lg">
    <template #content>
      <div class="flex">
        <div class="grow flex flex-col">
          <h6 class="font-medium text-lg">{{title}}</h6>
          <h4 class="text-3xl font-bold">{{content}}</h4>
          <p class="text-sm font-normal opacity-60">{{subContent}}</p>
        </div>
        <div class="flex grow-0 mb-auto rounded-md bg-[--p-primary-color] text-[--p-primary-contrast-color] p-2">
          <slot name="icon"></slot>
        </div>
      </div>
    </template>
  </Card>
</template>

<style scoped>

</style>