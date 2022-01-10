<template>
    <router-link
        v-if="buttonType === 'internal'"
        :to="url"
        class="text-gray-800 hover:text-gray-700 font-medium text-lg"
    >
        {{ text }}
    </router-link>

    <a
        v-else-if="buttonType === 'external'"
        :href="url"
        rel="noopener noreferrer"
        target="_blank"
        class="text-gray-800 hover:text-gray-700 font-medium text-lg"
    >
        {{ text }}
    </a>

    <button
        v-else
        :type="type"
        @click="emitClick"
        class="px-6 h-10 bg-green-700 border border-green-700 rounded text-white hover:bg-transparent hover:text-gray-700"
    >
        {{ text }}
    </button>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'Button',

    props: {
        text: {
            type: String,
            default: null,
        },
        type: {
            type: String,
            default: 'button',
        },
        url: {
            type: String,
            default: null,
        },
        kind: {
            type: String,
            default: 'primary',
        },
    },

    computed: {
        buttonType(): string {
            if (this.url?.indexOf('http') !== -1) {
                return 'external';
            }

            if (this.url) {
                return 'internal';
            }

            return 'button';
        },
    },

    methods: {
        emitClick(): void {
            this.$emit('clicked');
        },
    },
});
</script>
