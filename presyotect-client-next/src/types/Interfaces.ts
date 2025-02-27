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
    nameid: string,
    role: string;
    username: string;
}

// PSGC

// Type for provinces.json
export interface Province {
    code: string;
    name: string;
    regionCode: string;
    islandGroupCode: string;
    psgc10DigitCode: string;
}

// Type for cities-municipalities.json
export interface CityMunicipality {
    code: string;
    name: string;
    oldName: string;
    isCapital: boolean;
    isCity: boolean;
    isMunicipality: boolean;
    provinceCode: string;
    districtCode: boolean;
    regionCode: string;
    islandGroupCode: string;
    psgc10DigitCode: string;
}

// Entity Interfaces
export interface Product {
    id?: string,
    key: string | null,
    sku?: string | null,
    name: string,
    size: string | null,
    status?: string,
    category?: string[] | null,
    classification: string | null,
    srp?: number | null,
    monitoredPrices?: MonitoredPrice[] | null
}

export interface MonitoredEstablishment {
    id?: string,
    name: string;
    cityMunicipality: string;
    completeAddress: string;
    categories: (string | null)[];
    classifications: string[];
    subClassifications?: string[] | null;
}

export interface Establishment {
    id?: string,
    name: string;
    status: string;
    cityMunicipality: string;
    completeAddress: string;
    categories: (string | null)[];
    classifications: string[];
    subClassifications?: string[] | null;
    tags?: string[] | null;
    owners: string[];
    ownershipType?: string;
    contactPerson: string;
    designation?: string;
    website?: string;
    contactNumber?: string;
    email?: string;
    socials?: Record<string, string>;
}

export interface Personnel {
    id?: string;
    nickname: string;
    fullName: string;
    username: string;
    password: string;
    assignedEstablishments?: string[] | null;
}

export interface Category {
    id?: string | null;
    name: string;
    group: string;
    shortName?: string | null;
}

export interface Classification {
    id?: string | null;
    name: string;
    shortName?: string | null;
}

/*
* Price Monitoring Types
*/

export interface MonitoredPrice {
    created?: Date | null;
    productId?: string | null;
    productName?: string | null;
    productSize?: string | null;
    productCategories?: string[] | null;
    productClassification?: string | null;
    personnelId?: string | null;
    establishmentId?: string | null;
    establishmentName?: string | null;
    cityMunicipality?: string | null;
    price: number | null;
    remarks?: string | null;
    status?: string | null;
}

export interface PriceMonitoringRecordPrice {
    establishmentId: string;
    establishmentName: string;
    price: string;
    recorded: Date;
    status: string;
    cityMunicipality?: string;
}

export interface PriceMonitoringRecord {
    productId: string;
    productName: string;
    productSize: string;
    productClassification: string;
    productCategories?: string[] | null;
    monitoredPrices?: PriceMonitoringRecordPrice[] | null
}