<template>
    <div>
        <Title text="Chauffører" size="large" class="my-4" />

        <div>
            <div class="grid" style="grid-template-columns: 1fr 1fr 1fr 0.2fr">
                <Title text="Navn" size="small" />

                <Title text="Email" size="small" />

                <Title text="Telefonnummer" size="small" />
            </div>

            <div
                v-for="(driver, driverIndex) of drivers"
                :key="driverIndex"
                class="grid grid-cols-4 gap-4 py-4 content-center items-center"
                style="grid-template-columns: 1fr 1fr 1fr 0.2fr; border-top: 1px solid gray"
            >
                <p>
                    {{ driver.userFullName }}
                </p>

                <p>
                    {{ driver.userEmail }}
                </p>

                <p>
                    {{ driver.userPhoneNumber }}
                </p>

                <Dropdown
                    class="mx-auto"
                    :items="[
                        { label: 'Tildel tur', id: driver.userId },
                        { label: 'Ledige dage', id: driver.userId },
                    ]"
                    @dropdownClick="catchDropdownClick"
                />
            </div>
        </div>

        <Modal v-if="driverAvailabilityModal" @closeModal="switchAvailabilityModal">
            <Tile class="w-11/12 md:max-w-4xl min-w-fit mx-auto flex flex-col h-full">
                <div class="w-full flex mb-2">
                    <Title
                        :text="`Chauffør - ${
                            driverAvailabilityModalTab === 1
                                ? 'ledige dage'
                                : `ture ${
                                      routes && routes.length && routes[0] && routes[0].routeStartDate
                                          ? new Date(routes[0].routeStartDate).toLocaleDateString()
                                          : ''
                                  }`
                        }`"
                        size="medium"
                    />

                    <button class="ml-auto font-black" @click="switchAvailabilityModal">X</button>
                </div>

                <div v-if="driverAvailabilityModalTab === 1">
                    <div v-if="driverAvailability && driverAvailability.length">
                        <div class="grid grid-cols-2">
                            <Title text="Dato" size="small" />
                        </div>

                        <div v-for="(date, dateIndex) of driverAvailability" :key="dateIndex" class="grid grid-cols-2">
                            <p>
                                {{ new Date(date.driversAvailableDate).toLocaleDateString() }}
                            </p>

                            <Button text="Se ruter på dato" @clicked="getRoutesByDate(date.driversAvailableDate)" />
                        </div>
                    </div>

                    <div v-else>
                        <Title text="Chauffør har ingen ledige dage" />
                    </div>
                </div>

                <div v-if="driverAvailabilityModalTab === 2">
                    <div v-if="routes && routes.length">
                        <div class="grid grid-cols-7">
                            <Title text="Startdato" />

                            <Title text="Slutdato" />

                            <Title text="Varighed" />

                            <Title text="Start lokation" />

                            <Title text="Slut lokation" />

                            <Title text="Afdeling" />
                        </div>

                        <div
                            v-for="route of routes"
                            :key="route.routeId"
                            class="grid grid-cols-7 w-full content-center items-center"
                        >
                            <p>
                                {{ new Date(route.routeStartDate).toLocaleDateString() }}
                            </p>

                            <p>
                                {{ new Date(route.routeEndDate).toLocaleDateString() }}
                            </p>

                            <p>
                                {{ route.routeEstTime }}
                            </p>

                            <p class="flex gap-1">
                                <span
                                    v-if="
                                        route &&
                                        route.routeStartLocationId &&
                                        routeLocation(route.routeStartLocationId).postalCode
                                    "
                                    class="hidden xl:block"
                                >
                                    {{ routeLocation(route.routeStartLocationId).postalCode }}
                                </span>

                                <span
                                    v-if="
                                        route &&
                                        route.routeStartLocationId &&
                                        routeLocation(route.routeStartLocationId).countryName
                                    "
                                >
                                    {{ routeLocation(route.routeStartLocationId).countryName }}
                                </span>
                            </p>

                            <p class="flex gap-1">
                                <span
                                    v-if="
                                        route &&
                                        route.routeEndLocationId &&
                                        routeLocation(route.routeEndLocationId).postalCode
                                    "
                                    class="hidden xl:block"
                                >
                                    {{ routeLocation(route.routeEndLocationId).postalCode }}
                                </span>

                                <span
                                    v-if="
                                        route &&
                                        route.routeEndLocationId &&
                                        routeLocation(route.routeEndLocationId).countryName
                                    "
                                >
                                    {{ routeLocation(route.routeEndLocationId).countryName }}
                                </span>
                            </p>

                            <p>
                                {{ routeDepartment(route.departmentId) }}
                            </p>

                            <Button text="Tildel tur" @clicked="assignUserRoute(route.routeId)" />
                        </div>
                    </div>

                    <div v-else>
                        <Title text="Ingen ture på den dato" />
                    </div>

                    <Button kind="secondary" text="Tilbage" class="mt-4" @clicked="backModal" />
                </div>
            </Tile>
        </Modal>

        <Modal v-if="assignRouteModal" @closeModal="switchAssignRouteModal">
            <Tile class="w-11/12 md:max-w-4xl min-w-fit mx-auto flex flex-col h-full">
                <div class="w-full flex mb-2">
                    <Title text="Tildel tur" size="medium" />

                    <button class="ml-auto font-black" @click="switchAssignRouteModal">X</button>
                </div>

                <div>
                    <div v-if="filteredRoutes && filteredRoutes.length">
                        <div class="grid grid-cols-7">
                            <Title text="Startdato" />

                            <Title text="Slutdato" />

                            <Title text="Varighed" />

                            <Title text="Start lokation" />

                            <Title text="Slut lokation" />

                            <Title text="Afdeling" />
                        </div>

                        <div
                            v-for="route of filteredRoutes"
                            :key="route.routeId"
                            class="grid grid-cols-7 w-full content-center items-center mb-1"
                        >
                            <p>
                                {{ new Date(route.routeStartDate).toLocaleDateString() }}
                            </p>

                            <p>
                                {{ new Date(route.routeEndDate).toLocaleDateString() }}
                            </p>

                            <p>
                                {{ route.routeEstTime }}
                            </p>

                            <p class="flex gap-1">
                                <span
                                    v-if="
                                        route &&
                                        route.routeStartLocationId &&
                                        routeLocation(route.routeStartLocationId).postalCode
                                    "
                                    class="hidden xl:block"
                                >
                                    {{ routeLocation(route.routeStartLocationId).postalCode }}
                                </span>

                                <span
                                    v-if="
                                        route &&
                                        route.routeStartLocationId &&
                                        routeLocation(route.routeStartLocationId).countryName
                                    "
                                >
                                    {{ routeLocation(route.routeStartLocationId).countryName }}
                                </span>
                            </p>

                            <p class="flex gap-1">
                                <span
                                    v-if="
                                        route &&
                                        route.routeEndLocationId &&
                                        routeLocation(route.routeEndLocationId).postalCode
                                    "
                                    class="hidden xl:block"
                                >
                                    {{ routeLocation(route.routeEndLocationId).postalCode }}
                                </span>

                                <span
                                    v-if="
                                        route &&
                                        route.routeEndLocationId &&
                                        routeLocation(route.routeEndLocationId).countryName
                                    "
                                >
                                    {{ routeLocation(route.routeEndLocationId).countryName }}
                                </span>
                            </p>

                            <p>
                                {{ routeDepartment(route.departmentId) }}
                            </p>

                            <Button text="Tildel" @clicked="assignUserRoute(route.routeId)" />
                        </div>
                    </div>

                    <div v-else>
                        <Title text="Ingen ledige ture" />
                    </div>
                </div>
            </Tile>
        </Modal>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import { mapGetters, mapActions } from 'vuex';

export default Vue.extend({
    name: 'AdminDrivers',

    components: {
        Button: () => import('@/components/Button.vue'),
        Dropdown: () => import('@/components/Dropdown.vue'),
        Modal: () => import('@/components/Modal.vue'),
        Tile: () => import('@/components/Tile.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            drivers: [] as User[],
            currentDriver: null as number | null,

            driverAvailability: [] as DriversAvailable[],
            driverAvailabilityModal: false as boolean,
            driverAvailabilityModalTab: 1 as number,
            routes: [] as Route[],

            assignRouteModal: false as boolean,
        };
    },

    computed: {
        ...mapGetters(['getLocations', 'getDepartments', 'getRoutes']),

        filteredRoutes(): Route[] {
            if (this.getRoutes?.length) {
                return this.getRoutes
                    ?.filter(
                        (r: Route) =>
                            r?.userId === null &&
                            r?.routeStartDate &&
                            new Date(new Date(r?.routeStartDate).setHours(0, 0, 0, 0)) >
                                new Date(new Date().setHours(0, 0, 0, 0))
                    )
                    .sort((a: Route, b: Route) =>
                        a?.routeStartDate && b?.routeStartDate && a?.routeStartDate > b?.routeStartDate ? 1 : -1
                    );
            }

            return [];
        },
    },

    created() {
        this.fetchDrivers();
    },

    methods: {
        ...mapActions(['fetchRoutes']),

        catchDropdownClick(item: { label: string; id: number }): void {
            console.log('catchDropdownClick', item);

            // TODO: clean this shit
            if (item?.label?.toLowerCase() === 'tildel tur') {
                this.switchAssignRouteModal(item.id);
            } else if (item?.label?.toLowerCase() === 'ledige dage') {
                this.switchAvailabilityModal(item.id);
            }
        },

        async fetchDrivers(): Promise<void> {
            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/drivers`;

            this.drivers = await axios
                .get(URL)
                .then((res) => res?.data?.sort((a: User, b: User) => (a?.userFullName > b?.userFullName ? 1 : -1)))
                .catch((error) => {
                    console.error('Error getting drivers', error);
                    return [];
                });
        },

        async switchAvailabilityModal(userId: number | null = null): Promise<void> {
            this.driverAvailabilityModal = !this.driverAvailabilityModal;
            this.currentDriver = userId;

            if (this.driverAvailabilityModal) {
                const HOST = process.env.VUE_APP_API_URL;
                const URL = `${HOST}/api/drivers/available/${userId}`;

                this.driverAvailability = await axios
                    .get(URL)
                    .then((res) => res?.data)
                    .catch((error) => {
                        console.error('getDriverAvailability error', error);
                        return [];
                    });
                return;
            }

            this.driverAvailability = [];
            this.routes = [];
            this.driverAvailabilityModalTab = 1;
        },

        async getRoutesByDate(date: string): Promise<void> {
            const formattedDate = `${new Date(date).getFullYear()}-${
                new Date(date).getMonth() + 1 < 10 ? `0${new Date(date).getMonth() + 1}` : new Date(date).getMonth() + 1
            }-${new Date(date).getDate()}`;
            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/routes/available/${formattedDate}`;

            this.routes = await axios
                .get(URL)
                .then((res) => res?.data)
                .catch((error) => {
                    console.error('getRoutesByDate error', error);
                    return [];
                });

            this.driverAvailabilityModalTab = 2;
        },

        backModal(): void {
            this.driverAvailabilityModalTab = 1;
            this.routes = [];
        },

        routeLocation(locationId: number): {
            postalCode: string | null;
            cityName: string | null;
            countryName: string | null;
        } | null {
            const locationIndex: number = this.getLocations?.findIndex((l: Location) => l?.locationId === locationId);

            if (typeof locationIndex === 'number' && locationIndex !== -1) {
                return {
                    postalCode: this.getLocations[locationIndex]?.locationPostalCode || null,
                    cityName: this.getLocations[locationIndex]?.cityName || null,
                    countryName: this.getLocations[locationIndex]?.countryName || null,
                };
            }

            return {
                postalCode: null,
                cityName: null,
                countryName: null,
            };
        },

        routeDepartment(departmentId: number): string | null {
            const departmentIndex = this.getDepartments?.findIndex((l: Department) => l?.departmentId === departmentId);

            if (typeof departmentIndex === 'number' && departmentIndex !== -1) {
                return this.getDepartments[departmentIndex]?.departmentName?.trim();
            }

            return null;
        },

        async assignUserRoute(routeId: number): Promise<void> {
            if (this.currentDriver) {
                const HOST = process.env.VUE_APP_API_URL;
                const URL = `${HOST}/api/routes/assign/${routeId}/${this.currentDriver}`;

                await axios
                    .patch(URL)
                    .then(() => {
                        this.$swal({
                            icon: 'success',
                            title: `Chauffører sat på ruten succesfuldt`,
                            showConfirmButton: false,
                            timer: 1000,
                        });

                        if (this.driverAvailabilityModal) {
                            this.driverAvailabilityModalTab = 1;
                            this.routes = [];
                        } else if (this.assignRouteModal) {
                            this.fetchRoutes();
                        }
                    })
                    .catch((error) => {
                        console.error('assignUserRoute error', error);

                        this.$swal({
                            icon: 'error',
                            title: 'Der gik noget galt, prøv venligst igen senere',
                            timer: 800,
                            showConfirmButton: false,
                        });
                    });

                return;
            }

            this.$swal({
                icon: 'error',
                title: 'Der gik noget galt, prøv venligst igen senere',
                timer: 800,
                showConfirmButton: false,
            });
        },

        async switchAssignRouteModal(userId: number | null) {
            this.assignRouteModal = !this.assignRouteModal;

            this.currentDriver = userId;

            if (this.assignRouteModal) {
                this.fetchRoutes();
            } else {
                this.currentDriver = null;
            }
        },
    },
});
</script>
