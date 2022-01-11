<template>
    <div>
        <Title text="Opret tur" size="large" class="text-center mb-8" />

        <form class="w-11/12 md:max-w-lg mx-auto bg-gray-100 rounded p-8 flex flex-col" @submit.prevent="createRoute">
            <Title text="Tidspunkt" size="small" />

            <Title text="Startdato" />
            <input
                v-model="newRoute.RouteStartDate"
                required
                type="date"
                class="mb-4 w-full border border-gray-400 rounded p-1"
            />

            <Title text="Slutdato" />
            <input
                v-model="newRoute.RouteEndDate"
                required
                type="date"
                class="mb-4 w-full border border-gray-400 rounded p-1"
            />

            <Title text="Estimeret tid i timer" />
            <input
                v-model.number="newRoute.RouteEstTime"
                required
                type="number"
                class="mb-4 w-full lg:w-1/4 border border-gray-400 rounded p-1"
                min="0"
            />

            <Title text="Lokation" size="small" />
            <Title text="Start lokation" />
            <select
                v-model="newRoute.RouteStartLocationId"
                required
                class="mb-4 w-full border border-gray-400 rounded p-1"
            >
                <option
                    v-for="(startLocation, startLocationIndex) of getLocations"
                    :key="startLocationIndex"
                    :value="startLocation.LocationId"
                >
                    {{ startLocation.LocationPostalCode }} {{ startLocation.CityName }} {{ startLocation.CountryName }}
                </option>
            </select>

            <Title text="Slut lokation" />
            <select
                v-model="newRoute.RouteEndLocationId"
                required
                class="mb-4 w-full border border-gray-400 rounded p-1"
            >
                <option
                    v-for="(endLocation, endLocationIndex) of getLocations"
                    :key="endLocationIndex"
                    :value="endLocation.LocationId"
                >
                    {{ endLocation.LocationPostalCode }} {{ endLocation.CityName }} {{ endLocation.CountryName }}
                </option>
            </select>

            <Title text="Afdeling" />
            <select v-model="newRoute.DepartmentId" required class="mb-4 w-full border border-gray-400 rounded p-1">
                <option
                    v-for="(department, departmentIndex) of getDepartments"
                    :key="departmentIndex"
                    :value="department.DepartmentId"
                >
                    {{ department.DepartmentName }}
                </option>
            </select>

            <div class="flex mb-4">
                <Title text="Høj prioritet" />
                <Checkbox :checked="newRoute.RouteHighPriority" class="ml-auto" @onCheck="setRoutePriority" />
            </div>

            <Title text="Beskrivelse" />
            <textarea
                v-model="newRoute.RouteDescription"
                class="mb-4 w-full border border-gray-400 rounded p-1"
            ></textarea>

            <div class="w-full">
                <Button type="submit" text="Opret" class="ml-auto" />
            </div>
        </form>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    name: 'AdminCreateRoute',

    components: {
        Button: () => import('@/components/Button.vue'),
        Title: () => import('@/components/Title.vue'),
        Checkbox: () => import('@/components/Checkbox.vue'),
    },

    data() {
        return {
            newRoute: {
                RouteId: undefined,
                RouteDescription: undefined,
                RouteStartDate: undefined,
                RouteEndDate: undefined,
                RouteStartLocationId: undefined,
                RouteEndLocationId: undefined,
                RouteHighPriority: false,
                RouteStatusId: 0,
                UserId: undefined,
                DepartmentId: undefined,
                RouteEstTime: undefined,
            } as Route,
        };
    },

    computed: {
        ...mapGetters(['getLocations', 'getDepartments']),
    },

    methods: {
        async createRoute() {
            console.log('createRoute');

            const success = true;

            if (success) {
                this.$swal({
                    icon: 'success',
                    title: `Rute oprettet succesfuldt`,
                    showConfirmButton: false,
                    timer: 1000,
                });
            } else {
                this.$swal({
                    icon: 'error',
                    title: 'Der gik noget galt, prøv venligst igen senere',
                    timer: 800,
                    showConfirmButton: false,
                });
            }
        },

        setRoutePriority(checked: boolean) {
            this.newRoute.RouteHighPriority = checked;
        },
    },
});
</script>
