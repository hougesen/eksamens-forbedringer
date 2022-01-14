<template>
    <div>
        <div class="mb-4 grid grid-cols-3">
            <Button
                text="Kommende ture"
                :kind="currentTab === 1 ? 'primary' : 'secondary'"
                class="mr-auto"
                @clicked="currentTab = 1"
            />

            <Button
                text="Afsluttede ture"
                :kind="currentTab === 2 ? 'primary' : 'link'"
                class="m-auto"
                @clicked="currentTab = 2"
            />

            <Button
                text="Ledige ture"
                :kind="currentTab === 3 ? 'primary' : 'link'"
                class="ml-auto"
                @clicked="currentTab = 3"
            />
        </div>

        <div v-if="currentTab === 1" class="mt-4">
            <div v-if="pendingRoutes && pendingRoutes.length" class="flex flex-col gap-4">
                <RouteTile
                    v-for="pendingRoute of pendingRoutes"
                    :key="pendingRoute.routeId"
                    :route="pendingRoute"
                    button-text="Marker som færdig"
                    @clicked="markRouteCompleted(pendingRoute.routeId)"
                />
            </div>

            <div v-else>
                <Title text="Ingen afsluttede ture" />
            </div>
        </div>

        <div v-else-if="currentTab === 2">
            <div v-if="completedRoutes && completedRoutes.length" class="flex flex-col gap-2">
                <RouteTile
                    v-for="completedRoute of completedRoutes"
                    :key="completedRoute.routeId"
                    :route="completedRoute"
                />
            </div>

            <div v-else>
                <Title text="Ingen afsluttede ture" />
            </div>
        </div>

        <div v-else-if="currentTab === 3">
            <div v-if="availableRoutes && availableRoutes.length" class="flex flex-col gap-2">
                <RouteTile
                    v-for="availableRoute of availableRoutes"
                    :key="availableRoute.routeId"
                    :route="availableRoute"
                    :button-text="signedUpRouteIds.indexOf(availableRoute.routeId) === -1 ? 'Ansøgt' : 'Ansøg om rute'"
                    @clicked="signUpForRoute(availableRoute.routeId)"
                />
            </div>

            <div v-else>
                <Title text="Ingen ledige ture" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapActions } from 'vuex';
import axios from 'axios';

// eslint-disable-next-line no-shadow
enum RouteTabs {
    UserRoutes = 1,
    Completed = 2,
    Available = 3,
}

export default Vue.extend({
    name: 'DriverRoutes',

    components: {
        Button: () => import('@/components/Button.vue'),
        RouteTile: () => import('@/components/RouteTile.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            currentTab: 1 as RouteTabs,
            signedUpRouteIds: [] as number[],
        };
    },

    computed: {
        ...mapGetters(['getUserInfo', 'getRoutes', 'getLocations']),

        pendingRoutes(): Route[] {
            return this.getRoutes?.filter(
                (r: Route) => r?.userId === this.getUserInfo?.userId && r?.routeStatusId !== 3
            );
        },

        completedRoutes(): Route[] {
            return this.getRoutes?.filter(
                (r: Route) => r?.userId === this.getUserInfo?.userId && r?.routeStatusId === 3
            );
        },

        availableRoutes(): Route[] {
            return this.getRoutes?.filter((r: Route) => r?.userId === null);
        },
    },

    created() {
        this.fetchRoutes();
        if (this.getUserInfo?.userId) {
            this.getSignedUpRoutes();
        }
    },

    methods: {
        ...mapActions(['fetchRoutes']),

        async getSignedUpRoutes() {
            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/signups/user/${this.getUserInfo?.userId}`;

            const signups = await axios
                .get(URL)
                .then((res) => res?.data)
                .catch((error) => {
                    console.error('getSignedUpRoutes error', error);
                    return [];
                });

            if (signups?.length) {
                this.signedUpRouteIds = signups?.map((s: SignUpDriver) => s?.routeId);
            }
        },

        async markRouteCompleted(routeId: number): Promise<void> {
            console.log('markRouteCompleted');

            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/routes/completed/${routeId}`;

            await axios
                .patch(URL)
                .then(() => {
                    this.$swal({
                        icon: 'success',
                        title: `Rute markeret som færdig`,
                        showConfirmButton: false,
                        timer: 1000,
                    });
                })
                .catch((error) => {
                    console.error('markRouteCompleted error', error);

                    this.$swal({
                        icon: 'error',
                        title: 'Der gik noget galt, prøv venligst igen senere',
                        timer: 800,
                        showConfirmButton: false,
                    });
                });

            this.fetchRoutes();
        },

        async signUpForRoute(routeId: number): Promise<void> {
            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/signups/${routeId}/${this.getUserInfo.userId}`;

            await axios
                .post(URL)
                .then(() => {
                    this.$swal({
                        icon: 'success',
                        title: `Du har nu ansøgt om ruten`,
                        showConfirmButton: false,
                        timer: 1000,
                    });
                })
                .catch((error) => {
                    console.error('signUpForRoute error', error);

                    this.$swal({
                        icon: 'error',
                        title: 'Der gik noget galt, prøv venligst igen senere',
                        timer: 800,
                        showConfirmButton: false,
                    });
                });

            this.getSignedUpRoutes();
        },
    },
});
</script>
