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
    id?: string,
    key: string | null,
    sku: string | null,
    name: string,
    size: string | null,
    status?: string,
    category: string[] | null,
    classification: string | null,
    srp: number | null
}

export interface Establishment {
    id?: string,
    name: string;
    status: string;
    cityMunicipality: string;
    completeAddress: string;
    categories: (string | null)[];
    classifications: string[];
    subClassifications: (string | null)[];
    tags: (string | null)[];
    owners: string[];
    ownershipType?: string;
    contactPerson: string;
    designation?: string;
    website?: string;
    contactNumber?: string;
    email?: string;
    socials?: Record<string, string>;
}