<template>
    <div class="flex flex-col gap-4 place-content-center place-items-center w-full h-full" style="min-height: 50vh">
        <router-link
            v-for="(page, pageIndex) of pages"
            :key="pageIndex"
            :to="page.path"
            class="w-1/2 bg-gray-100 h-full px-4 py-6 rounded text-center"
        >
            <Title :text="page.text" :size="windowWidth >= 768 ? 'large' : 'medium'" />
        </router-link>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'DriverDashboard',

    components: {
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            windowWidth: 0 as number,
            pages: [
                {
                    text: 'Mine ture',
                    path: '/routes',
                },
                {
                    text: 'Ledige ture',
                    path: '/routes/available',
                },
            ],
        };
    },

    created() {
        this.windowWidth = window.innerWidth;

        window.addEventListener('resize', () => {
            this.windowWidth = window.innerWidth;
        });
    },
});
</script>
