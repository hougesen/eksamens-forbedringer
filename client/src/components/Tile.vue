<template>
    <div class="bg-gray-100 rounded p-8 flex flex-col">
        <slot />
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'Tile',
});
</script>
