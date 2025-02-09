import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios from "axios";
import type {Product} from "@/types/Interfaces.ts";

export class ProductsService {

    static async addProduct(product: Product): Promise<Product> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/products", product, axiosConfig);
        return response.data.content;
    }

    static async deleteProduct(productId: number): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/products/${productId}`, axiosConfig);
        return response.data.success;
    }

    static async getProducts(): Promise<Product[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.get("/products", axiosConfig);
        return response.data.content;
    }

}