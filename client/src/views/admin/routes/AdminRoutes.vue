<template>
    <div>
        <h1>Routes asd</h1>

        <div class="grid grid-cols-7">
            <p>Startdato</p>
            <p>Slutdato</p>
            <p>Varighed</p>
            <p>Afdeling</p>
            <p>Start lokation</p>
            <p>Slut lokation</p>
            <p>Status</p>
            <div></div>
        </div>

        <div v-for="route of getRoutes" :key="route.RouteId" class="grid grid-cols-7 w-full">
            <p>
                {{ new Date(route.RouteStartDate).toLocaleDateString() }}
            </p>

            <p>
                {{ new Date(route.RouteEndDate).toLocaleDateString() }}
            </p>

            <p>
                {{ typeof route.RouteEstTime === 'number' ? route.RouteEstTime : 'N/A' }}

                {{ typeof route.RouteEstTime === 'number' && route.RouteEstTime === 1 ? 'time' : 'timer' }}
            </p>

            <p>
                {{ routeDepartment(route.DepartmentId) }}
            </p>

            <div class="flex gap-1">
                <p class="hidden 2xl:block">
                    {{ routeLocation(route.RouteStartLocationId).postalCode }}
                </p>
                <p class="hidden xl:block">
                    {{ routeLocation(route.RouteStartLocationId).cityName }}
                </p>
                <p>
                    {{ routeLocation(route.RouteStartLocationId).countryName }}
                </p>
            </div>

            <div class="flex gap-1">
                <p class="hidden 2xl:block">
                    {{ routeLocation(route.RouteEndLocationId).postalCode }}
                </p>
                <p class="hidden xl:block">
                    {{ routeLocation(route.RouteEndLocationId).cityName }}
                </p>
                <p>
                    {{ routeLocation(route.RouteEndLocationId).countryName }}
                </p>
            </div>

            <p>
                {{ routeStatusCode(route.RouteStatusId) }}
            </p>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    name: 'Routes',

    computed: {
        ...mapGetters(['getRoutes', 'getLocations', 'getDepartments']),
    },

    methods: {
        routeLocation(locationId: number): {
            postalCode: string;
            cityName: string;
            countryName: string;
        } | null {
            const locationIndex: number = this.getLocations?.findIndex((l: Location) => l?.LocationId === locationId);

            if (typeof locationIndex === 'number' && locationIndex !== -1) {
                return {
                    postalCode: this.getLocations[locationIndex]?.LocationPostalCode,
                    cityName: this.getLocations[locationIndex]?.CityName,
                    countryName: this.getLocations[locationIndex]?.CountryName,
                };
            }

            return null;
        },

        routeStatusCode(statusId: number): string | null {
            if (statusId === 0) return 'missing-driver';
            if (statusId === 1) return 'pending';
            if (statusId === 3) return 'completed';
            return null;
        },

        routeDepartment(departmentId: number): string | null {
            const departmentIndex = this.getDepartments?.findIndex((l: Department) => l?.DepartmentId === departmentId);

            if (typeof departmentIndex === 'number' && departmentIndex !== -1) {
                return this.getDepartments[departmentIndex]?.DepartmentName?.trim();
            }

            return null;
        },
    },
});
</script>
