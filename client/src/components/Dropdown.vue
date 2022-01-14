<template>
    <div class="relative">
        <button class="flex place-content-center place-items-center gap-2" @click="open = !open">
            <slot />

            <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 512 512"
                class="transform hover:rotate-90"
                :class="open ? 'transform rotate-90' : ''"
                height="20px"
                width="20px"
                v-if="icon === 'cog'"
            >
                <path
                    fill="currentColor"
                    d="m487.4 315.7-42.6-24.6c4.3-23.2 4.3-47 0-70.2l42.6-24.6c4.9-2.8 7.1-8.6 5.5-14-11.1-35.6-30-67.8-54.7-94.6-3.8-4.1-10-5.1-14.8-2.3L380.8 110c-17.9-15.4-38.5-27.3-60.8-35.1V25.8c0-5.6-3.9-10.5-9.4-11.7-36.7-8.2-74.3-7.8-109.2 0-5.5 1.2-9.4 6.1-9.4 11.7V75c-22.2 7.9-42.8 19.8-60.8 35.1L88.7 85.5c-4.9-2.8-11-1.9-14.8 2.3-24.7 26.7-43.6 58.9-54.7 94.6-1.7 5.4.6 11.2 5.5 14L67.3 221c-4.3 23.2-4.3 47 0 70.2l-42.6 24.6c-4.9 2.8-7.1 8.6-5.5 14 11.1 35.6 30 67.8 54.7 94.6 3.8 4.1 10 5.1 14.8 2.3l42.6-24.6c17.9 15.4 38.5 27.3 60.8 35.1v49.2c0 5.6 3.9 10.5 9.4 11.7 36.7 8.2 74.3 7.8 109.2 0 5.5-1.2 9.4-6.1 9.4-11.7v-49.2c22.2-7.9 42.8-19.8 60.8-35.1l42.6 24.6c4.9 2.8 11 1.9 14.8-2.3 24.7-26.7 43.6-58.9 54.7-94.6 1.5-5.5-.7-11.3-5.6-14.1zM256 336c-44.1 0-80-35.9-80-80s35.9-80 80-80 80 35.9 80 80-35.9 80-80 80z"
                />
            </svg>

            <svg
                v-else
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 192 512"
                height="24px"
                width="24px"
                class="transform hover:rotate-90"
                :class="open ? 'transform rotate-90' : ''"
            >
                <path
                    fill="currentColor"
                    d="M96 184c39.8 0 72 32.2 72 72s-32.2 72-72 72-72-32.2-72-72 32.2-72 72-72zM24 80c0 39.8 32.2 72 72 72s72-32.2 72-72S135.8 8 96 8 24 40.2 24 80zm0 352c0 39.8 32.2 72 72 72s72-32.2 72-72-32.2-72-72-72-72 32.2-72 72z"
                />
            </svg>
        </button>

        <div
            v-if="open"
            class="py-4 bg-gray-100 flex flex-col absolute z-50 rounded gap-2 min-w-[10rem]"
            style="left: 50%; transform: translate(-50%, 0); min-width: 10rem"
        >
            <button
                v-for="(item, itemIndex) of items"
                :key="itemIndex"
                class="text-gray-900"
                @click="emitDropdownClick(item)"
            >
                {{ item.label }}
            </button>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'Dropdown',

    props: {
        items: {
            type: Array,
            required: true,
        },

        icon: {
            type: String,
            default: null,
        },
    },

    data() {
        return {
            open: false as boolean,
        };
    },

    methods: {
        emitDropdownClick(item: string): void {
            this.$emit('dropdownClick', item);

            this.open = false;
        },
    },
});
</script>
