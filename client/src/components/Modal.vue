<template>
    <div
        class="z-50 w-screen h-screen fixed top-0 left-0 flex place-content-center place-items-center max-w-screen"
        style="background-color: rgba(169, 169, 169, 0.5)"
        @click="emitClose"
    >
        <div class="w-full max-w-screen" @click.stop>
            <slot />
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'Modal',

    methods: {
        emitClose(): void {
            this.$emit('closeModal');
        },
    },
});
</script>
