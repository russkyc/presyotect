export interface BreadcrumbItem{
    label: string;
    url?: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    data: any;
}