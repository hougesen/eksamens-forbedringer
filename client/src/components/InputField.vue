<template>
    <div>
        <label
            v-if="label"
            class="block text-gray-700 text-sm font-bold mb-2"
            :class="labelStyling"
            :for="label.toLowerCase()"
        >
            {{ label }}
        </label>

        <input
            v-model="value"
            :name="label ? label.toLowerCase() : null"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            :type="inputType"
            :required="required"
            autocomplete="off"
            @change="emitValue"
        />
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'InputField',

    props: {
        label: {
            type: String,
            default: null,
        },
        existingValue: {
            type: String,
            default: null,
        },
        labelSize: {
            type: String,
            default: null,
        },
        inputType: {
            type: String,
            default: 'text',
        },
        required: {
            type: Boolean,
            default: false,
        },
        autocomplete: {
            type: String,
            default: null,
        },
    },

    data() {
        return {
            value: '' as string,
        };
    },

    computed: {
        labelStyling(): string {
            if (this.labelSize === 'large') {
                return 'text-2xl';
            }
            if (this.labelSize === 'medium') {
                return 'text-xl';
            }

            if (this.labelSize === 'small') {
                return 'text-lg';
            }

            return 'text-base';
        },
    },

    created() {
        if (this.existingValue) {
            this.value = this.existingValue;
        }
    },

    methods: {
        emitValue() {
            this.$emit('onChange', this.value);
        },
    },
});
</script>
