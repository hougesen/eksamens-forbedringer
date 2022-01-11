<template>
    <label for="toogleA" class="flex items-center cursor-pointer">
        <div class="relative flex items-center cursor-pointer">
            <!-- input -->
            <input id="toogleA" :value="checked" type="checkbox" class="sr-only" @change="emitChange" />
            <!-- line -->
            <div class="w-10 h-4 bg-gray-400 rounded-full shadow-inner"></div>
            <!-- dot -->
            <div class="dot absolute w-6 h-6 bg-white rounded-full shadow -left-1 -top-1 transition"></div>
        </div>
    </label>
</template>

<script lang="ts">
import Vue from 'vue';

// Piv stjålet fra https://codepen.io/lhermann/pen/EBGZRZ

export default Vue.extend({
    name: 'Checkbox',

    props: {
        checked: Boolean,
    },

    methods: {
        emitChange() {
            this.$emit('onCheck', !this.checked);
        },
    },
});
</script>

<style scoped>
/* Toggle A */
input:checked ~ .dot {
    transform: translateX(100%);
    background-color: #48bb78;
}
</style>
