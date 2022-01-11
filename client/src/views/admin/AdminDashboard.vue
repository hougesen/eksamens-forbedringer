<template>
    <div>
        <h1>Admin dashboard</h1>

        <div class="grid grid-cols-3 gap-4">
            <KPI text="Ruter denne uge" :value="Math.floor(Math.random() * 100000)" />

            <KPI text="Ruter uden chauffør" :value="Math.floor(Math.random() * 100000)" />

            <KPI text="Ledige chauffører dage" :value="Math.floor(Math.random() * 100000)" />
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'AdminDashboard',

    components: {
        KPI: () => import('@/components/KPI.vue'),
    },
});
</script>
