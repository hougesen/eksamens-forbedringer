<template>
    <component :is="appropriateComponent" v-if="appropriateComponent" />
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    name: 'Dashboard',

    components: {
        AdminDashboard: () => import('@/views/admin/AdminDashboard.vue'),
        DriverDashboard: () => import('@/views/driver/DriverDashboard.vue'),
    },

    computed: {
        ...mapGetters(['getUserInfo']),

        appropriateComponent(): string {
            if (this.getUserInfo?.type === 1) {
                return 'AdminDashboard';
            }

            return 'DriverDashboard';
        },
    },
});
</script>
