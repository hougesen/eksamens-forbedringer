<template>
    <div class="px-10 py-16 justify-center items-center bg-gray-200 rounded text-center">
        <Title size="medium" :text="value.toString()" />

        <Title size="small" :text="text" />
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'KPI',

    components: {
        Title: () => import('@/components/Title.vue'),
    },

    props: {
        text: {
            type: String,
            required: true,
        },
        value: {
            type: [String, Number],
            required: true,
        },
    },
});
</script>
