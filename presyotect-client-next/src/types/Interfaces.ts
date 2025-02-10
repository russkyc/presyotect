import type {JwtPayload} from "jwt-decode";

export interface BreadcrumbItem {
    label: string;
    url?: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    data: TokenPayload | string | null;
}

export interface TokenPayload extends JwtPayload{
    role: string;
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

export type Establishment = {
    id?: number,
    name: string,
    location: string,
    type: string
};