<script setup lang="ts">

import {defineProps} from "vue";

defineProps<{
  content?: string,
  href: string
}>();

</script>

<template>
  <RouterLink class="p-3 rounded-md flex gap-3 overflow-clip" activeClass="bg-primary text-[--p-primary-contrast-color]" :to="href">
    <div class="shrink-0"><slot name="icon"></slot></div>
    <p class="my-auto text-sm font-medium" v-if="content">{{ content }}</p>
  </RouterLink>
</template>

<style scoped>

</style>