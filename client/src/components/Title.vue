<template>
    <h1 v-if="size === 'large'" class="text-4xl font-black text-gray-800">
        <slot> {{ text }} </slot>
    </h1>

    <h2 v-else-if="size === 'medium'" class="text-2xl font-bold text-gray-800">
        <slot> {{ text }} </slot>
    </h2>

    <h3 v-else-if="size === 'small'" class="text-lg font-bold text-gray-800">
        <slot> {{ text }} </slot>
    </h3>

    <h4 v-else class="text-base font-medium text-gray-800">
        <slot> {{ text }} </slot>
    </h4>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'Title',

    props: {
        text: {
            type: String,
            default: null,
        },
        size: {
            type: String,
            default: null,
        },
    },
});
</script>
