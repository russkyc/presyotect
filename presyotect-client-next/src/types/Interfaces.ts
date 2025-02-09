export interface BreadcrumbItem {
    label: string;
    url?: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    data: IDecodedToken | string | null;
}

export interface IDecodedToken {
    role: string;
    iat: number;
    exp: number;
    username: string;
}


// Entity Interfaces
export type Product = {
    id?: number,
    key: string | null,
    sku: string | null,
    name: string,
    size: string | null,
    status?: string,
    category: string[] | null,
    classification: string | null,
    srp: number | null
}