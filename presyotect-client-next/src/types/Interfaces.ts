export interface BreadcrumbItem {
    label: string;
    url?: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    data: any;
}

// Entity Interfaces
export type Product = {
    id?: number,
    customIdentifier: string | null,
    sku: string | null,
    name: string,
    size: string | null,
    status?: string,
    category: string[] | null,
    classification: string | null,
    srp: number | null
}