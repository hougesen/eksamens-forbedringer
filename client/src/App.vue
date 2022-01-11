<template>
    <div class="container mx-auto">
        <TopNav v-if="loggedIn" />

        <div>
            <router-view />
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    components: {
        TopNav: () => import('@/components/TopNav.vue'),
    },

    computed: {
        ...mapGetters(['loggedIn']),
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
