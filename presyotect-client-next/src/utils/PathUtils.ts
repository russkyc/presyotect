import {DashboardGroupType} from "@/types/Types.ts";

export function getPageType(path: string): DashboardGroupType {
    if (path.startsWith("/login")) {
        return DashboardGroupType.Auth;
    }
    return DashboardGroupType.Dashboard;
}