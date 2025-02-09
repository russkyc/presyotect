<script lang="ts" setup>

import {Card} from "primevue";
</script>

<template>
  <Card class="rounded-lg">
    <template #content>
      <div class="flex flex-col gap-3">
        <div class="flex gap-3">
          <div class="flex grow">
            <slot name="card-title" />
          </div>
          <div class="flex">
            <slot name="card-actions" />
          </div>
        </div>
        <slot name="card-content" />
      </div>
    </template>
  </Card>
</template>

<style scoped>

</style>