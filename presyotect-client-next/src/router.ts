import {createRouter, createWebHistory} from 'vue-router';
import {useAuthStore} from "@features/auth/auth-store.ts";
import {Roles} from "@/types/Constants.ts";

import DashboardView from '@pages/dashboard/DashboardView.vue';
import LoginView from "@pages/auth/LoginView.vue";
import PriceMonitoringView from "@pages/dashboard/PriceMonitoringView.vue";
import NotFoundView from "@pages/status/NotFoundView.vue";
import ProductsView from "@pages/dashboard/products/ProductsView.vue";
import EstablishmentsView from "@pages/dashboard/EstablishmentsView.vue";
import UsersView from "@pages/dashboard/UsersView.vue";
import SettingsView from "@pages/dashboard/SettingsView.vue";
import AnalyticsView from "@pages/dashboard/AnalyticsView.vue";
import NotAuthorizedView from "@pages/status/NotAuthorizedView.vue";
import AddProductView from "@pages/dashboard/products/AddProductView.vue";

const defaultTitle = 'Presyotect';

const routes = [
    {
        name: 'dashboard',
        path: '/',
        component: DashboardView,
        meta: {
            title: 'Dashboard',
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: 'price-monitoring',
        path: '/price-monitoring',
        component: PriceMonitoringView,
        meta: {
            title: 'Price Monitoring',
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: 'products',
        path: '/products',
        children: [
            {
                path: '', component: ProductsView,
                meta: {
                    title: 'Products',
                    roles: [Roles.Admin, Roles.Personnel]
                }
            },
            {
                path: 'add', component: AddProductView,
                meta: {
                    title: 'Products',
                    roles: [Roles.Admin]
                }
            },
        ]
    },
    {
        name: 'establishments',
        path: '/establishments',
        component: EstablishmentsView,
        meta: {
            title: 'Establishments',
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: 'users',
        path: '/users',
        component: UsersView,
        meta: {
            title: 'Users',
            roles: [Roles.Admin]
        }
    },
    {
        name: 'analytics',
        path: '/analytics',
        component: AnalyticsView,
        meta: {
            title: 'Analytics',
            roles: [Roles.Admin]
        }
    },
    {
        name: 'settings',
        path: '/settings',
        component: SettingsView,
        meta: {
            title: 'Settings',
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: 'login',
        path: '/login',
        component: LoginView,
        meta: {
            title: 'Login'
        }
    },
    {
        name: 'not-found',
        path: '/:pathMatch(.*)*',
        component: NotFoundView,
        meta: {
            title: 'Not Found'
        }
    },
    {
        name: 'not-authorized',
        path: '/not-authorized',
        component: NotAuthorizedView,
        meta: {
            title: 'Not Authorized'
        }
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach(async (to, _, next) => {

    const title = to.meta.title as string;
    const authStore = useAuthStore();

    const tokenIsNull = authStore.token === null || authStore.token === undefined;
    const toNameIsLogin = to.name === 'login';

    if (tokenIsNull && !toNameIsLogin) {
        document.title = title ? `${defaultTitle} - ${title}` : defaultTitle;
        next({name: 'login'});
        return;
    }
    if (!tokenIsNull && toNameIsLogin) {
        document.title = title ? `${defaultTitle} - ${title}` : defaultTitle;
        next({name: 'dashboard'});
        return;
    }
    if (!tokenIsNull && !toNameIsLogin) {
        const roles = to.meta.roles as string[] | null | undefined;
        if (roles == null || undefined) {
            next();
            return
        }
        const includesRole = roles?.includes(authStore.userRoles);
        document.title = title ? `${defaultTitle} - ${title}` : defaultTitle;
        if (!includesRole && to.name !== 'not-authorized') {
            next({name: 'not-authorized'});
            return;
        }
    }
    next();
});

export default router;