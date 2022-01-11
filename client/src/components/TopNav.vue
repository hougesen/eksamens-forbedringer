<template>
    <div class="flex w-screen max-w-full" style="align-content: center; align-items: center">
        <router-link to="/">
            <Title size="medium" text="Alex Andersen" />
        </router-link>

        <div class="mx-4 flex gap-6">
            <Button url="/" text="Dashboard" />
            <Button url="/routes/create" text="Opret tur" />
            <Button url="/routes" text="Ture" />
            <Button url="/drivers" text="Chauffører" />
        </div>

        <div class="ml-auto">
            <p class="font-medium text-gray-800 text-lg">
                {{ getUserInfo.UserFullName }}
            </p>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    name: 'TopNav',

    components: {
        Title: () => import('@/components/Title.vue'),
        Button: () => import('@/components/Button.vue'),
    },

    computed: {
        ...mapGetters(['getUserInfo']),
    },
});
</script>
