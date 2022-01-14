<template>
    <div class="container mx-auto px-2 pt-2">
        <TopNav v-if="getUserInfo" />

        <div class="w-full h-full">
            <router-view />
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapActions } from 'vuex';

export default Vue.extend({
    components: {
        TopNav: () => import('@/components/TopNav.vue'),
    },

    computed: {
        ...mapGetters(['getUserInfo']),
    },

    created() {
        this.initStore();
    },

    methods: {
        ...mapActions(['initStore']),
    },
});
</script>

<style>
* {
    margin: 0;
    padding: 0;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    transition: 0.4s;
    line-height: 1.6;
}

*::before,
*::after {
    margin: 0;
    padding: 0;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    line-height: 1.6;
}

body,
html {
    scroll-behavior: smooth;
    height: 100%;
    padding-top: 0;
    width: 100%;
}
</style>
