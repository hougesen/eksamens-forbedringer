<template>
    <div>
        <Title text="Opret tur" size="large" class="text-center mb-8" />

        <Tile class="w-11/12 md:max-w-lg mx-auto">
            <form @submit.prevent="createRoute">
                <Title text="Tidspunkt" size="small" />

                <Title text="Startdato" />
                <input
                    v-model="newRoute.routeStartDate"
                    required
                    type="date"
                    class="mb-4 w-full border border-gray-400 rounded p-1"
                />

                <Title text="Slutdato" />
                <input
                    v-model="newRoute.routeEndDate"
                    required
                    type="date"
                    class="mb-4 w-full border border-gray-400 rounded p-1"
                />

                <Title text="Estimeret tid i timer" />
                <input
                    v-model.number="newRoute.routeEstTime"
                    required
                    type="number"
                    class="mb-4 w-full lg:w-1/4 border border-gray-400 rounded p-1"
                    min="0"
                />

                <Title text="Lokation" size="small" />
                <Title text="Start lokation" />
                <select
                    v-model="newRoute.routeStartLocationId"
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
                    v-model="newRoute.routeEndLocationId"
                    required
                    class="mb-4 w-full border border-gray-400 rounded p-1"
                >
                    <option
                        v-for="(endLocation, endLocationIndex) of getLocations"
                        :key="endLocationIndex"
                        :value="endLocation.locationId"
                    >
                        {{ endLocation.locationPostalCode }} {{ endLocation.cityName }} {{ endLocation.countryName }}
                    </option>
                </select>

                <Title text="Afdeling" />
                <select v-model="newRoute.departmentId" required class="mb-4 w-full border border-gray-400 rounded p-1">
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
                    <Checkbox :checked="newRoute.routeHighPriority" class="ml-auto" @onCheck="setRoutePriority" />
                </div>

                <Title text="Beskrivelse" />
                <textarea
                    v-model="newRoute.routeDescription"
                    class="mb-4 w-full border border-gray-400 rounded p-1"
                ></textarea>

                <div class="w-full">
                    <Button type="submit" text="Opret" class="ml-auto" />
                </div>
            </form>
        </Tile>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';
import axios from 'axios';

export default Vue.extend({
    name: 'AdminCreateRoute',

    components: {
        Button: () => import('@/components/Button.vue'),
        Checkbox: () => import('@/components/Checkbox.vue'),
        Tile: () => import('@/components/Tile.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            newRoute: {
                routeDescription: null,
                routeStartDate: null,
                routeEndDate: null,
                routeStartLocationId: null,
                routeEndLocationId: null,
                routeHighPriority: false,
                routeStatusId: 1,
                departmentId: null,
                routeEstTime: null,
            } as Route,
        };
    },

    computed: {
        ...mapGetters(['getLocations', 'getDepartments']),
    },

    methods: {
        async createRoute() {
            console.log('createRoute');

            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/routes`;

            await axios
                .post(URL, this.newRoute)
                .then(() => {
                    this.$swal({
                        icon: 'success',
                        title: `Rute oprettet succesfuldt`,
                        showConfirmButton: false,
                        timer: 1000,
                    });

                    this.$router.push('/routes');
                })
                .catch((error) => {
                    console.error('Error creating new route', error);

                    this.$swal({
                        icon: 'error',
                        title: 'Der gik noget galt, prøv venligst igen senere',
                        timer: 800,
                        showConfirmButton: false,
                    });
                });
        },

        setRoutePriority(checked: boolean) {
            this.newRoute.routeHighPriority = checked;
        },
    },
});
</script>
