<template>
    <div>
        <Title text="Ruter" size="large" class="my-4" />

        <div class="grid grid-cols-8 gap-1">
            <Title text="Startdato" size="small" />

            <Title text="Slutdato" size="small" />

            <Title text="Varighed" size="small" />

            <Title text="Afdeling" size="small" />

            <Title text="Start lokation" size="small" />

            <Title text="Slut lokation" size="small" />

            <Title text="Status" size="small" />

            <div></div>
        </div>

        <div
            v-for="route of getRoutes"
            :key="route.routeId"
            class="grid grid-cols-8 gap-1 w-full py-4 content-center items-center"
            style="border-top: 1px solid gray"
        >
            <p>
                {{ new Date(route.routeStartDate).toLocaleDateString() }}
            </p>

            <p>
                {{ new Date(route.routeEndDate).toLocaleDateString() }}
            </p>

            <p>
                {{ typeof route.routeEstTime === 'number' ? route.routeEstTime : 'N/A' }}

                {{ typeof route.routeEstTime === 'number' && route.routeEstTime === 1 ? 'time' : 'timer' }}
            </p>

            <p>
                {{ routeDepartment(route.departmentId) }}
            </p>

            <div class="flex gap-1">
                <p
                    v-if="route && route.routeStartLocationId && routeLocation(route.routeStartLocationId).postalCode"
                    class="hidden xl:block"
                >
                    {{ routeLocation(route.routeStartLocationId).postalCode }}
                </p>

                <p v-if="route && route.routeStartLocationId && routeLocation(route.routeStartLocationId).countryName">
                    {{ routeLocation(route.routeStartLocationId).countryName }}
                </p>
            </div>

            <div class="flex gap-1">
                <p
                    v-if="route && route.routeEndLocationId && routeLocation(route.routeEndLocationId).countryName"
                    class="hidden xl:block"
                >
                    {{ routeLocation(route.routeEndLocationId).postalCode }}
                </p>

                <p v-if="route && route.routeEndLocationId && routeLocation(route.routeEndLocationId).countryName">
                    {{ routeLocation(route.routeEndLocationId).countryName }}
                </p>
            </div>

            <p>
                {{
                    route && route.routeStatusId
                        ? routeStatusCode(route.routeStatusId)
                        : route && route.userId !== null
                        ? routeStatusCode(1)
                        : routeStatusCode(0)
                }}
            </p>

            <Dropdown
                class="mx-auto"
                :items="[
                    { label: 'Tildel tur', id: route.routeId },
                    { label: 'Ansøgninger', id: route.routeId },
                    { label: 'Rediger', id: route.routeId },
                    { label: 'Slet', id: route.routeId },
                ]"
                @dropdownClick="catchDropdownClick"
            />
        </div>

        <Modal v-if="editRouteModal" @closeModal="switchEditRouteModal">
            <Tile class="w-11/12 lg:w-lg md:max-w-lg mx-auto">
                <div class="w-full flex mb-2">
                    <Title text="Rediger tur" size="medium" />

                    <button class="ml-auto font-black" @click="switchEditRouteModal">X</button>
                </div>

                <form @submit.prevent="saveRoute">
                    <Title text="Tidspunkt" size="small" />

                    <Title text="Startdato" />
                    <input
                        v-model="editRoute.routeStartDate"
                        required
                        type="date"
                        class="mb-4 w-full border border-gray-400 rounded p-1"
                    />

                    <Title text="Slutdato" />
                    <input
                        v-model="editRoute.routeEndDate"
                        required
                        type="date"
                        class="mb-4 w-full border border-gray-400 rounded p-1"
                    />

                    <Title text="Estimeret tid i timer" />
                    <input
                        v-model.number="editRoute.routeEstTime"
                        required
                        type="number"
                        class="mb-4 w-full lg:w-1/4 border border-gray-400 rounded p-1"
                        min="0"
                    />

                    <Title text="Lokation" size="small" />
                    <Title text="Start lokation" />
                    <select
                        v-model="editRoute.routeStartLocationId"
                        required
                        class="mb-4 w-full border border-gray-400 rounded p-1"
                    >
                        <option
                            v-for="(startLocation, startLocationIndex) of getLocations"
                            :key="startLocationIndex"
                            :value="startLocation.locationId"
                        >
                            {{ startLocation.locationPostalCode }} {{ startLocation.cityName }}
                            {{ startLocation.countryName }}
                        </option>
                    </select>

                    <Title text="Slut lokation" />
                    <select
                        v-model="editRoute.routeEndLocationId"
                        required
                        class="mb-4 w-full border border-gray-400 rounded p-1"
                    >
                        <option
                            v-for="(endLocation, endLocationIndex) of getLocations"
                            :key="endLocationIndex"
                            :value="endLocation.locationId"
                        >
                            {{ endLocation.locationPostalCode }} {{ endLocation.cityName }}
                            {{ endLocation.countryName }}
                        </option>
                    </select>

                    <Title text="Afdeling" />
                    <select
                        v-model="editRoute.departmentId"
                        required
                        class="mb-4 w-full border border-gray-400 rounded p-1"
                    >
                        <option
                            v-for="(department, departmentIndex) of getDepartments"
                            :key="departmentIndex"
                            :value="department.departmentId"
                        >
                            {{ department.departmentName }}
                        </option>
                    </select>

                    <div class="flex mb-4">
                        <Title text="Høj prioritet" />
                        <Checkbox :checked="editRoute.routeHighPriority" class="ml-auto" @onCheck="setRoutePriority" />
                    </div>

                    <Title text="Beskrivelse" />
                    <textarea
                        v-model="editRoute.routeDescription"
                        class="mb-4 w-full border border-gray-400 rounded p-1"
                    ></textarea>

                    <div class="w-full">
                        <Button type="submit" text="Gem" class="ml-auto" />
                    </div>
                </form>
            </Tile>
        </Modal>

        <Modal v-if="assignRouteModal" @closeModal="switchAssignRouteModal">
            <Tile class="w-11/12 md:max-w-4xl min-w-fit mx-auto flex flex-col h-full">
                <div class="w-full flex mb-2">
                    <Title text="Chauffører" size="medium" />

                    <button class="ml-auto font-black" @click="switchAssignRouteModal">X</button>
                </div>

                <div class="grid grid-cols-4 gap-4">
                    <Title text="Navn" size="small" />
                    <Title text="Email" size="small" />
                    <Title text="Telefonnummer" size="small" />
                </div>

                <div
                    v-for="(driver, driverIndex) of drivers"
                    :key="driverIndex"
                    class="grid grid-cols-4 gap-4 py-4 content-center items-center"
                    style="border-top: 1px solid gray"
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
                    <Button text="Tildel tur" kind="primary" @clicked="assignUserRoute(driver.userId)" />
                </div>
            </Tile>
        </Modal>

        <Modal v-if="routeSignupModal" @closeModal="switchRouteSignupModal">
            <Tile class="w-11/12 md:max-w-4xl min-w-fit mx-auto flex flex-col h-full">
                <div class="w-full flex mb-2">
                    <Title text="Ansøgninger" size="medium" />

                    <button class="ml-auto font-black" @click="switchRouteSignupModal">X</button>
                </div>

                <div v-if="routeSignups && routeSignups.length">
                    <div class="grid grid-cols-4 gap-4">
                        <Title text="Navn" size="small" />
                        <Title text="Email" size="small" />
                        <Title text="Telefonnummer" size="small" />
                    </div>
                    <div
                        v-for="signUpDriver of routeSignups"
                        :key="signUpDriver.userId"
                        class="grid grid-cols-4 gap-4 py-4 content-center items-center"
                        style="border-top: 1px solid gray"
                    >
                        <p>
                            {{ signUpDriver.userFullName }}
                        </p>

                        <p>
                            {{ signUpDriver.userEmail }}
                        </p>

                        <p>
                            {{ signUpDriver.userPhoneNumber }}
                        </p>

                        <Button text="Tildel tur" kind="primary" @clicked="assignUserRoute(signUpDriver.userId)" />
                    </div>
                </div>

                <div v-else>
                    <Title text="Ingen ansøgninger" />
                </div>
            </Tile>
        </Modal>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters, mapActions } from 'vuex';
import axios from 'axios';

export default Vue.extend({
    name: 'Routes',

    components: {
        Button: () => import('@/components/Button.vue'),
        Checkbox: () => import('@/components/Checkbox.vue'),
        Dropdown: () => import('@/components/Dropdown.vue'),
        Modal: () => import('@/components/Modal.vue'),
        Tile: () => import('@/components/Tile.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            drivers: [] as User[],
            assignRouteModal: false as boolean,
            currentRoute: null as number | null,

            editRouteModal: false as boolean,
            editRoute: {} as Route | null,

            routeSignupModal: false as boolean,
            routeSignups: [] as User[] | null,
        };
    },

    computed: {
        ...mapGetters(['getRoutes', 'getLocations', 'getDepartments']),
    },

    created() {
        this.fetchRoutes();
    },

    methods: {
        ...mapActions(['fetchRoutes']),

        routeLocation(locationId: number): {
            locationAddress: string | null;
            postalCode: string | null;
            cityName: string | null;
            countryName: string | null;
        } | null {
            const locationIndex: number = this.getLocations?.findIndex((l: Location) => l?.locationId === locationId);

            if (typeof locationIndex === 'number' && locationIndex !== -1) {
                return {
                    locationAddress: this.getLocations[locationIndex]?.locationAddress || null,
                    postalCode: this.getLocations[locationIndex]?.locationPostalCode || null,
                    cityName: this.getLocations[locationIndex]?.cityName || null,
                    countryName: this.getLocations[locationIndex]?.countryName || null,
                };
            }

            return {
                locationAddress: null,
                postalCode: null,
                cityName: null,
                countryName: null,
            };
        },

        routeStatusCode(statusId: number): string | null {
            if (statusId === 1) return 'missing-driver';
            if (statusId === 2) return 'pending';
            if (statusId === 3) return 'completed';
            return null;
        },

        routeDepartment(departmentId: number): string | null {
            const departmentIndex = this.getDepartments?.findIndex((l: Department) => l?.departmentId === departmentId);

            if (typeof departmentIndex === 'number' && departmentIndex !== -1) {
                return this.getDepartments[departmentIndex]?.departmentName?.trim();
            }

            return null;
        },

        async deleteRoute(routeId: number): Promise<void> {
            console.log('deleteRoute', routeId);

            if (typeof routeId === 'number') {
                const confirmation = await this.$swal({
                    title: `Er du sikker på du vil slette denne rute?`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#c13859',
                    confirmButtonText: 'Ja',
                    cancelButtonText: 'Nej',
                });

                if (confirmation?.isConfirmed) {
                    const HOST = process.env.VUE_APP_API_URL;
                    const URL = `${HOST}/api/routes/${routeId}`;

                    await axios
                        .delete(URL)
                        .then(() => {
                            this.$swal({
                                icon: 'success',
                                title: `Rute slettet succesfuldt`,
                                showConfirmButton: false,
                                timer: 1000,
                            });

                            this.fetchRoutes();
                        })
                        .catch((error) => {
                            console.error('deleteRoute error', error);
                            this.$swal({
                                icon: 'error',
                                title: 'Der gik noget galt, prøv venligst igen senere',
                                timer: 800,
                                showConfirmButton: false,
                            });
                        });
                }
            } else {
                this.$swal({
                    icon: 'error',
                    title: 'Der gik noget galt, prøv venligst igen senere',
                    timer: 800,
                    showConfirmButton: false,
                });
            }
        },

        async switchEditRouteModal(routeId: number | null = null): Promise<void> {
            console.log('switchEditRouteModal', routeId);

            if (!this.editRouteModal) {
                const HOST = process.env.VUE_APP_API_URL;
                const URL = `${HOST}/api/routes/${routeId}`;

                const route = await axios
                    .get(URL)
                    .then((res) => res?.data?.route)
                    .catch((error) => {
                        console.error('switchEditRouteModal error', error);

                        this.$swal({
                            icon: 'error',
                            title: 'Der gik noget galt, prøv venligst igen senere',
                            timer: 800,
                            showConfirmButton: false,
                        });
                        return null;
                    });

                if (route) {
                    this.editRoute = {
                        departmentId: route?.departmentId,
                        routeDescription: route?.routeDescription,
                        routeStartDate: route?.routeEndDate ? this.formatDate(new Date(route?.routeEndDate)) : null,
                        routeEndDate: route?.routeEndDate ? this.formatDate(new Date(route?.routeEndDate)) : null,
                        routeEndLocationId: route?.routeEndLocationId,
                        routeEstTime: route?.routeEstTime,
                        routeHighPriority: route?.routeHighPriority || false,
                        routeId: route.routeId,
                        routeStartLocationId: route?.routeStartLocationId,
                        routeStatusId: route?.routeStatusId,
                        userId: route?.userId,
                        signUpDrivers: route?.signUpDrivers,
                    };

                    this.editRouteModal = !this.editRouteModal;
                    return;
                }
            } else {
                this.editRoute = null;
            }
            this.editRouteModal = !this.editRouteModal;

            this.fetchRoutes();
        },

        formatDate(d: Date): string {
            console.log('formatDate', d);

            return `${d.getFullYear()}-${d.getMonth() + 1 < 10 ? `0${d.getMonth() + 1}` : d.getMonth() + 1}`;
        },

        catchDropdownClick(item: { label: string; id: number }): void {
            console.log('catchDropdownClick', item);

            // TODO: clean this shit
            if (item?.label?.toLowerCase() === 'tildel tur') {
                this.switchAssignRouteModal(item.id);
            } else if (item?.label?.toLowerCase() === 'rediger') {
                this.switchEditRouteModal(item.id);
            } else if (item?.label?.toLowerCase() === 'slet') {
                this.deleteRoute(item.id);
            } else if (item?.label?.toLowerCase() === 'ansøgninger') {
                this.switchRouteSignupModal(item.id);
            }
        },

        switchAssignRouteModal(routeId: number | null = null): void {
            console.log('switchAssignRouteModal');

            this.assignRouteModal = !this.assignRouteModal;
            this.currentRoute = routeId;

            if (this.assignRouteModal) {
                this.fetchDrivers();
            } else {
                this.fetchRoutes();
            }
        },
        setRoutePriority(checked: boolean) {
            console.log('setRoutePriority');

            if (this.editRoute) {
                this.editRoute.routeHighPriority = checked;
            }
        },

        async saveRoute() {
            console.log('saveRoute');

            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/routes/${this.editRoute?.routeId}`;

            await axios
                .put(URL, this.editRoute)
                .then(() => {
                    this.$swal({
                        icon: 'success',
                        title: `Rute opdatere succesfuldt`,
                        showConfirmButton: false,
                        timer: 1000,
                    });
                })
                .catch((error) => {
                    console.error('saveRoute error', error);
                    this.$swal({
                        icon: 'error',
                        title: 'Der gik noget galt, prøv venligst igen senere',
                        timer: 800,
                        showConfirmButton: false,
                    });
                });

            this.switchEditRouteModal();
        },

        async fetchDrivers(): Promise<void> {
            console.log('fetchDrivers');

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

        async assignUserRoute(userId: number): Promise<void> {
            console.log('assignUserRoute', userId);

            if (userId && this.currentRoute) {
                const HOST = process.env.VUE_APP_API_URL;
                const URL = `${HOST}/api/routes/assign/${this.currentRoute}/${userId}`;

                await axios
                    .patch(URL)
                    .then(() => {
                        this.$swal({
                            icon: 'success',
                            title: `Chauffører sat på ruten succesfuldt`,
                            showConfirmButton: false,
                            timer: 1000,
                        });

                        this.routeSignupModal = false;
                        this.routeSignups = null;
                        this.assignRouteModal = false;
                        this.currentRoute = null;
                        this.drivers = [];

                        this.fetchRoutes();
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

        switchRouteSignupModal(routeId: number): void {
            this.routeSignupModal = !this.routeSignupModal;
            this.currentRoute = routeId;

            if (this.routeSignupModal) {
                this.fetchRouteSignups();
            } else {
                this.routeSignups = null;
            }

            this.fetchRoutes();
        },

        async fetchRouteSignups(): Promise<void> {
            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/signups/route/${this.currentRoute}`;

            this.routeSignups = await axios
                .get(URL)
                .then((res) => res?.data)
                .catch((error) => {
                    console.error('fetchRouteSignups error', error);
                    this.$swal({
                        icon: 'error',
                        title: 'Der gik noget galt, prøv venligst igen senere',
                        timer: 800,
                        showConfirmButton: false,
                    });

                    return [];
                });
        },
    },
});
</script>
