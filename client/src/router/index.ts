import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import store from '@/store/index';

Vue.use(VueRouter);

const routePaths = [
    {
        path: '/',
        name: 'Dashboard',
        meta: {
            title: 'Dashboard',
            requiresAuth: true,
        },
    },
    {
        path: '/login',
        name: 'Login',
        meta: {
            title: 'Login',
            requiresAuth: false,
        },
    },
    {
        path: '/routes/',
        folders: 'admin/routes/',
        name: 'AdminRoutes',
        meta: {
            title: 'Ture',
            requiresAuth: true,
            userType: 1,
        },
    },
    {
        path: '/routes/create',
        folders: 'admin/routes/',
        name: 'AdminCreateRoute',
        meta: {
            title: 'Opret tur',
            requiresAuth: true,
            userType: 1,
        },
    },
    {
        path: '/drivers',
        folders: 'admin/',
        name: 'AdminDrivers',
        meta: {
            title: 'Chauffører',
            requiresAuth: true,
            userType: 1,
        },
    },
];

const routes: Array<RouteConfig> = routePaths.map((route) => ({
    ...route,
    component: () => import(/* webpackChunkName: "[request]" */ `@/views/${route?.folders || ''}${route.name}.vue`),
}));
const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [
        ...routes,
        {
            path: '*',
            name: 'NotFound',
            redirect: { path: '/' },
        },
    ],
});

// TODO: Actually make this secure
router.beforeEach((to, from, next) => {
    const autheticated = store.getters.loggedIn;
    console.log(to);
    if (!autheticated && to.matched.some((record) => record.meta.requiresAuth)) {
        next({
            path: '/login',
            params: { nextUrl: to.fullPath },
        });

        window.scrollTo(0, 0);
        return;
    }

    if (autheticated && to?.fullPath?.toLowerCase() === '/login') {
        next({
            path: '/',
            params: { nextUrl: to.fullPath },
        });
        window.scrollTo(0, 0);
        return;
    }

    if (to?.meta?.userType && to?.meta?.userType !== store?.getters?.getUserInfo?.type) {
        console.log('ASDASD');
        next({
            path: '/',
            params: { nextUrl: to.fullPath },
        });
    }

    next();
    window.scrollTo(0, 0);
});

export default router;
