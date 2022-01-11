import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        userInfo: {
            UserFullName: 'Mads Hougesen',
            UserTypeId: 1,
        } as User,

        departments: [
            {
                DepartmentId: 1,
                DepartmentName: 'Odense',
            },
            {
                DepartmentId: 2,
                DepartmentName: 'Aarhus',
            },
        ] as Department[],

        locations: [
            {
                LocationId: 1,
                LocationAddress: 'Kranvejen 77',
                LocationPostalCode: '5000',
                CityId: 1,
                CityName: 'Odense',
                CountryId: 1,
                CountryName: 'DNK',
            },
            {
                LocationId: 2,
                LocationAddress: 'Guldborgsundsvej 14',
                LocationPostalCode: '4800',
                CityId: 2,
                CityName: 'Nykøbing Falster',
                CountryId: 1,
                CountryName: 'DNK',
            },
        ] as Location[],

        routes: [
            {
                RouteId: 1,
                RouteDescription: 'Lorem 1',
                RouteStartDate: new Date(),
                RouteEndDate: new Date(),
                RouteStartLocationId: 2,
                RouteEndLocationId: 1,
                RouteHighPriority: false,
                RouteStatusId: 0,
                UserId: undefined,
                DepartmentId: 1,
                RouteEstTime: 8,
            },
            {
                RouteId: 2,
                RouteDescription: 'Lorem 2 ',
                RouteStartDate: new Date(),
                RouteEndDate: new Date(),
                RouteStartLocationId: 2,
                RouteEndLocationId: 1,
                RouteHighPriority: false,
                RouteStatusId: 0,
                UserId: undefined,
                DepartmentId: 1,
                RouteEstTime: 0,
            },
        ] as Route[],
    },

    mutations: {},

    actions: {},

    getters: {
        getUserInfo: (state): User => state.userInfo,

        getLocations: (state): Location[] => state.locations,

        getDepartments: (state): Department[] => state.departments,

        getRoutes: (state): Route[] => state.routes,

        // TODO: REMOVE THIS TEST VALUE
        loggedIn: (): boolean => true,
    },
});
