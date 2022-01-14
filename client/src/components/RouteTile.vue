<template>
    <Tile class="flex flex-col gap-2 p-1">
        <div v-if="route && route.routeDescription">
            <Title text="Beskrivelse" />

            <p>
                <span v-if="route.routeEstTime">
                    Estimeret tid: {{ route.routeEstTime }} {{ route.routeEstTime === 1 ? 'time' : 'timer' }}.
                </span>
                {{ route.routeDescription }}
            </p>
        </div>

        <div>
            <Title text="Starttidspunkt" />

            <p>
                {{ new Date(route.routeStartDate).toLocaleDateString() }}
            </p>
        </div>

        <div>
            <Title text="Startlokation" />

            <p class="flex gap-1">
                <span
                    v-if="
                        route && route.routeStartLocationId && routeLocation(route.routeStartLocationId).locationAddress
                    "
                >
                    {{ routeLocation(route.routeStartLocationId).locationAddress }},
                </span>

                <span v-if="route && route.routeStartLocationId && routeLocation(route.routeStartLocationId).cityName">
                    {{ routeLocation(route.routeStartLocationId).cityName }}
                </span>

                <span
                    v-if="route && route.routeStartLocationId && routeLocation(route.routeStartLocationId).postalCode"
                >
                    {{ routeLocation(route.routeStartLocationId).postalCode }},
                </span>

                <span
                    v-if="route && route.routeStartLocationId && routeLocation(route.routeStartLocationId).countryName"
                >
                    {{ routeLocation(route.routeStartLocationId).countryName }}
                </span>
            </p>
        </div>

        <div>
            <Title text="Slutlokation" />

            <p class="flex gap-1">
                <span
                    v-if="
                        route && route.routeStartLocationId && routeLocation(route.routeEndLocationId).locationAddress
                    "
                >
                    {{ routeLocation(route.routeEndLocationId).locationAddress }},
                </span>

                <span v-if="route && route.routeEndLocationId && routeLocation(route.routeEndLocationId).cityName">
                    {{ routeLocation(route.routeEndLocationId).cityName }}
                </span>
                <span v-if="route && route.routeEndLocationId && routeLocation(route.routeEndLocationId).postalCode">
                    {{ routeLocation(route.routeEndLocationId).postalCode }},
                </span>

                <span v-if="route && route.routeEndLocationId && routeLocation(route.routeEndLocationId).countryName">
                    {{ routeLocation(route.routeEndLocationId).countryName }}
                </span>
            </p>
        </div>

        <Button v-if="buttonText" :text="buttonText" class="mt-1 ml-auto" kind="secondary" @clicked="emitClicked" />
    </Tile>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
    name: 'RouteTile',

    components: {
        Button: () => import('@/components/Button.vue'),
        Tile: () => import('@/components/Tile.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    props: {
        route: {
            type: Object,
            required: true,
        },
        buttonText: {
            type: String,
            default: null,
        },
    },

    computed: {
        ...mapGetters(['getLocations']),
    },

    methods: {
        emitClicked(): void {
            this.$emit('clicked');
        },

        routeLocation(locationId: number): {
            locationAddress: string | null;
            postalCode: string | null;
            cityName: string | null;
            countryName: string | null;
        } {
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
    },
});
</script>
