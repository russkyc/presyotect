<script lang="ts" setup>

import {defineProps} from "vue";

defineProps<{
  content?: string,
  href: string
}>();

</script>

<template>
  <RouterLink
    :to="href"
    active-class="bg-primary text-[--p-primary-contrast-color]"
    class="p-3 rounded-md flex gap-3 overflow-clip"
  >
    <div class="shrink-0">
      <slot name="icon" />
    </div>
    <p
      v-if="content"
      class="my-auto text-sm font-medium"
    >
      {{ content }}
    </p>
  </RouterLink>
</template>

<style scoped>

</style>