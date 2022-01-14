<template>
    <div class="flex w-screen max-w-full mb-6 content-center items-center">
        <router-link to="/" class="hidden md:block">
            <Title size="medium" text="Alex Andersen" />
        </router-link>

        <button
            v-if="showBackButton && getUserInfo && getUserInfo.userTypeId === 1"
            class="md:hidden"
            @click="$router.go(-1)"
        >
            <svg xmlns="http://www.w3.org/2000/svg" width="30px" height="30px" viewBox="0 0 512 512">
                <path
                    fill="currentColor"
                    d="M256 504C119 504 8 393 8 256S119 8 256 8s248 111 248 248-111 248-248 248zm28.9-143.6L209.4 288H392c13.3 0 24-10.7 24-24v-16c0-13.3-10.7-24-24-24H209.4l75.5-72.4c9.7-9.3 9.9-24.8.4-34.3l-11-10.9c-9.4-9.4-24.6-9.4-33.9 0L107.7 239c-9.4 9.4-9.4 24.6 0 33.9l132.7 132.7c9.4 9.4 24.6 9.4 33.9 0l11-10.9c9.5-9.5 9.3-25-.4-34.3z"
                />
            </svg>
        </button>

        <div class="hidden md:flex mx-4 gap-6">
            <Button v-if="getUserInfo && getUserInfo.userTypeId === 1" url="/" text="Dashboard" />

            <Button v-if="getUserInfo && getUserInfo.userTypeId === 1" url="/routes/create" text="Opret tur" />

            <Button v-if="getUserInfo && getUserInfo.userTypeId === 1" url="/routes" text="Ture" />

            <Button v-if="getUserInfo && getUserInfo.userTypeId === 1" url="/drivers" text="Chauffører" />
        </div>

        <div class="ml-auto flex">
            <Dropdown :items="[{ label: 'Logud', id: null }]" icon="cog" @dropdownClick="catchDropdownClick">
                <p class="font-medium text-gray-800 text-lg">
                    {{ getUserInfo.userFullName }}
                </p>
            </Dropdown>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapActions } from 'vuex';

export default Vue.extend({
    name: 'TopNav',

    components: {
        Button: () => import('@/components/Button.vue'),
        Dropdown: () => import('@/components/Dropdown.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            path: window.location.pathname as string,
        };
    },

    computed: {
        ...mapGetters(['getUserInfo']),

        showBackButton(): boolean {
            return this.path !== '/';
        },
    },
    watch: {
        $route: {
            handler() {
                this.path = window.location.pathname;
                console.log('route', this.path, window.location.pathname);
            },
        },
    },
    methods: {
        ...mapActions(['logout']),

        catchDropdownClick(item: { label: string; id: number | null }): void {
            if (item?.label?.toLowerCase() === 'logud') {
                this.logout();
            }
        },
    },
});
</script>
