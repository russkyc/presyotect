import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios, {type AxiosResponse} from "axios";
import type {Category, Classification} from "@/types/Interfaces.ts";

export class ConfigurationService {

    static async addCategory(category: Category): Promise<Category> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/categories", category, axiosConfig);
        return response.data.content;
    }

    static async deleteCategory(id: string): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/categories/${id}`, axiosConfig);
        return response.data.success;
    }

    static async getCategories(group?: string): Promise<Category[]> {
        const axiosConfig = getAxiosConfig();
        let response:AxiosResponse;
        if (group){
            response = await axios.get(`/categories?group=${group}`, axiosConfig);
        } else {
            response = await axios.get("/categories", axiosConfig);
        }
        return response.data.content;
    }

    static async addClassification(classification: Classification): Promise<Classification> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/classifications", classification, axiosConfig);
        return response.data.content;
    }

    static async deleteClassification(id: string): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/classifications/${id}`, axiosConfig);
        return response.data.success;
    }

    static async getClassifications(): Promise<Classification[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.get("/classifications", axiosConfig);
        return response.data.content;
    }
}