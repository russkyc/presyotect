<script setup lang="ts">

import {Card} from "primevue";
</script>

<template>
  <Card class="rounded-lg">
    <template #content>
      <div class="flex flex-col gap-3">
        <div class="flex">
          <div class="flex grow">
            <slot name="card-title"></slot>
          </div>
          <div class="flex">
            <slot name="card-actions"></slot>
          </div>
        </div>
        <slot name="card-content"></slot>
      </div>
    </template>
  </Card>
</template>

<style scoped>

</style>