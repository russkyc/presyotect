import AnalyticsView from "@features/analytics/AnalyticsView.vue";
import LoginView from "@features/auth/LoginView.vue";
import DashboardView from "@features/DashboardView.vue";
import AddEstablishmentView from "@features/establishments/AddEstablishmentView.vue";
import EstablishmentsView from "@features/establishments/EstablishmentsView.vue";
import PriceMonitoringView from "@features/monitoring/PriceMonitoringView.vue";
import NotAuthorizedView from "@features/NotAuthorizedView.vue";
import NotFoundView from "@features/NotFoundView.vue";
import AddPersonnelView from "@features/personnel/AddPersonnelView.vue";
import PersonnelView from "@features/personnel/PersonnelView.vue";
import AddProductView from "@features/products/AddProductView.vue";
import ProductsView from "@features/products/ProductsView.vue";
import SettingsView from "@features/settings/SettingsView.vue";
import {refreshToken} from "@services/auth/auth-service.ts";
import {useActionsStore} from "@stores/actions-store.ts";
import {useAuthStore} from "@stores/auth-store.ts";
import {useConfirm} from "primevue/useconfirm";
import {useToast} from "primevue/usetoast";
import {createRouter, createWebHistory} from "vue-router";
import {Roles, Routes} from "@/types/Constants.ts";

const defaultTitle = "Presyotect";

const routes = [
    {
        name: "dashboard",
        path: Routes.Dashboard,
        component: DashboardView,
        meta: {
            title: "Dashboard",
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: "price-monitoring",
        path: Routes.Monitoring,
        component: PriceMonitoringView,
        meta: {
            title: "Price Monitoring",
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: "products",
        path: Routes.Products,
        children: [
            {
                name: "products-home",
                path: "",
                component: ProductsView,
                meta: {
                    title: "Products",
                    roles: [Roles.Admin, Roles.Personnel]
                }
            },
            {
                path: "add",
                component: AddProductView,
                meta: {
                    title: "Products",
                    roles: [Roles.Admin, Roles.Personnel]
                }
            },
        ]
    },
    {
        name: "establishments",
        path: Routes.Establishments,
        children: [
            {
                name: "establishments-home",
                path: "",
                component: EstablishmentsView,
                meta: {
                    title: "Establishments",
                    roles: [Roles.Admin, Roles.Personnel]
                }
            },
            {
                path: "add",
                component: AddEstablishmentView,
                meta: {
                    title: "Establishments",
                    roles: [Roles.Admin, Roles.Personnel]
                }
            },
        ]
    },
    {
        name: "personnel",
        path: Routes.Personnel,
        children: [
            {
                name: "personnel-home",
                path: "",
                component: PersonnelView,
                meta: {
                    title: "Personnel",
                    roles: [Roles.Admin]
                }
            },
            {
                path: "add",
                component: AddPersonnelView,
                meta: {
                    title: "Personnel",
                    roles: [Roles.Admin]
                }
            }
        ]
    },
    {
        name: "analytics",
        path: Routes.Analytics,
        component: AnalyticsView,
        meta: {
            title: "Analytics",
            roles: [Roles.Admin]
        }
    },
    {
        name: "settings",
        path: Routes.Settings,
        component: SettingsView,
        meta: {
            title: "Settings",
            roles: [Roles.Admin, Roles.Personnel]
        }
    },
    {
        name: "login",
        path: "/login",
        component: LoginView,
        meta: {
            title: "Login"
        }
    },
    {
        name: "not-found",
        path: "/:pathMatch(.*)*",
        component: NotFoundView,
        meta: {
            title: "Not Found"
        }
    },
    {
        name: "not-authorized",
        path: "/not-authorized",
        component: NotAuthorizedView,
        meta: {
            title: "Not Authorized"
        }
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach(async (to, _, next) => {

    const toast = useToast();
    const confirm = useConfirm();
    const authStore = useAuthStore();
    const actionsStore = useActionsStore();

    const title = to.meta.title as string;

    const tokenIsNull = authStore.token === null || authStore.token === undefined;
    const toNameIsLogin = to.name === "login";
    
    if (!tokenIsNull && authStore.isTokenExpired) {
        await refreshToken();
    }

    if (actionsStore.hasPendingActions) {
        confirm.require({
            message: "Leaving this page will discard any unsaved changes. Are you sure you want to leave?",
            header: "Discard Changes",
            rejectProps: {
                label: "Cancel",
                severity: "secondary"
            },
            acceptProps: {
                label: "Leave"
            },
            accept: async () => {
                actionsStore.hasPendingActions = false;
                toast.add({
                    severity: "info",
                    summary: "Changes Discarded",
                    detail: "You have successfully discarded any unsaved changes.",
                    life: 2000
                });
                document.title = title ? `${defaultTitle} - ${title}` : defaultTitle;
                await router.push(to);
            },
            reject: () => {
                return;
            }
        });
        return;
    }

    if (tokenIsNull && !toNameIsLogin) {
        document.title = title ? `${defaultTitle} - Login` : defaultTitle;
        next({name: "login"});
        return;
    }
    if (!tokenIsNull && toNameIsLogin) {
        document.title = title ? `${defaultTitle} - ${title}` : defaultTitle;
        next({name: "dashboard"});
        return;
    }
    if (!tokenIsNull && !toNameIsLogin) {
        const roles = to.meta.roles as string[] | null | undefined;
        if (roles == null || undefined) {
            next();
            return
        }
        if (roles.length === 0) {
            next();
            return;
        }
        if (!authStore.userClaims?.role) {
            next({name: "not-authorized"});
            return;
        }
        const includesRole = roles?.includes(authStore.userClaims.role);
        if (!includesRole && to.name !== "not-authorized") {
            next({name: "not-authorized"});
            return;
        }
    }
    next();
});

export default router;