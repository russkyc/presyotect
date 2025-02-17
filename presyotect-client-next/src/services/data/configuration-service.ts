import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios from "axios";

export class ConfigurationService {

    static async addCategory(category: string): Promise<string[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/configuration/categories", category, axiosConfig);
        return response.data.content;
    }

    static async deleteCategory(categoryName: string): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/configuration/categories?category=${categoryName}`, axiosConfig);
        return response.data.success;
    }

    static async getCategories(): Promise<string[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.get("/configuration/categories", axiosConfig);
        return response.data.content;
    }

    static async addClassification(category: string): Promise<string[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/configuration/categories", category, axiosConfig);
        return response.data.content;
    }

    static async deleteClassification(categoryName: string): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/configuration/categories?category=${categoryName}`, axiosConfig);
        return response.data.success;
    }

    static async getClassifications(): Promise<string[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.get("/configuration/categories", axiosConfig);
        return response.data.content;
    }
}