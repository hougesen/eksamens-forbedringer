import Vue from 'vue';
import Vuex from 'vuex';
import axios from 'axios';

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        userInfo: null as User | null,
        departments: [] as Department[],
        locations: [] as Location[],
        routes: [] as Route[],
    },

    mutations: {
        setUserInfo: (state, userInfo: User): void => {
            console.log('setUserInfo', userInfo);
            state.userInfo = userInfo;
        },

        setRoutes: (state, routes: Route[]): void => {
            console.log('setRoutes', routes);

            state.routes = routes;
        },

        setDepartments: (state, departments: Department[]): void => {
            console.log('setDepartments', departments);

            state.departments = departments;
        },

        setLocations: (state, locations: Location[]): void => {
            console.log('setLocations', locations);

            state.locations = locations;
        },
    },

    actions: {
        initStore: ({ commit, dispatch }): void => {
            console.log('initStore');

            const userInfoLocal: User = JSON.parse(localStorage.getItem('userInfo') || '{}');

            if (userInfoLocal?.userId) {
                commit('setUserInfo', userInfoLocal);

                dispatch('fetchLocations');
                dispatch('fetchDepartments');
                dispatch('fetchRoutes');
            }
        },

        handleLogin: ({ commit, dispatch }, userInfo: User): void => {
            localStorage.setItem('userInfo', JSON.stringify(userInfo));

            commit('setUserInfo', userInfo);

            dispatch('fetchLocations');
            dispatch('fetchDepartments');

            if (userInfo?.userTypeId === 1) {
                dispatch('fetchRoutes');
            } else if (userInfo?.userTypeId === 2) {
                dispatch('fetchUserRoutes', userInfo?.userId);
            }
        },

        fetchRoutes: async ({ commit }): Promise<void> => {
            console.log('fetchRoutes');

            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/routes`;

            const routes: Route[] = await axios
                .get(URL)
                .then((res) => res?.data)
                .catch((error) => {
                    console.error('Error getting routes', error);
                    return null;
                });

            if (routes?.length) {
                commit('setRoutes', routes);
            }
        },

        fetchUserRoutes: async ({ state, commit }): Promise<void> => {
            console.log('fetchUserRoutes');

            if (state?.userInfo?.userId) {
                const HOST = process.env.VUE_APP_API_URL;
                const URL = `${HOST}/api/routes/user/${state.userInfo.userId}`;

                const routes: Route[] = await axios
                    .get(URL)
                    .then((res) => res?.data)
                    .catch((error) => {
                        console.error('Error getting routes', error);
                        return null;
                    });

                if (routes?.length) {
                    commit('setRoutes', routes);
                }
            }
        },

        fetchDepartments: async ({ commit }): Promise<void> => {
            console.log('fetchDepartments');

            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/departments`;

            const departments: Department[] = await axios
                .get(URL)
                .then((res) => res?.data)
                .catch((error) => {
                    console.error('Error getting departments', error);
                    return null;
                });

            if (departments?.length) {
                commit('setDepartments', departments);
            }
        },

        fetchLocations: async ({ commit }): Promise<void> => {
            console.log('fetchLocations');

            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/locations`;

            const locations: Location[] = await axios
                .get(URL)
                .then((res) => res?.data)
                .catch((error) => {
                    console.log('fetchLocations error', error);
                    return null;
                });

            if (locations?.length) {
                commit('setLocations', locations);
            }
        },

        logout: (): void => {
            localStorage.clear();
            window.location.reload();
        },
    },

    getters: {
        getUserInfo: (state): User | null => state?.userInfo,

        getLocations: (state): Location[] => state?.locations,

        getDepartments: (state): Department[] => state?.departments,

        getRoutes: (state): Route[] => state?.routes,
    },
});
