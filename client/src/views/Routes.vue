<template>
    <component :is="appropriateComponent" v-if="appropriateComponent" />
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    name: 'Routes',

    components: {
        AdminRoutes: () => import('@/views/admin/routes/AdminRoutes.vue'),
        DriverRoutes: () => import('@/views/driver/DriverRoutes.vue'),
    },

    computed: {
        ...mapGetters(['getUserInfo']),

        appropriateComponent(): string {
            if (this.getUserInfo?.userTypeId === 1) {
                return 'AdminRoutes';
            }

            return 'DriverRoutes';
        },
    },
});
</script>
