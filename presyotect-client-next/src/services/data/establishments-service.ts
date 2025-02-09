import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios from "axios";
import type {Establishment} from "@/types/Interfaces.ts";

export class EstablishmentsService {

    static async addEstablishment(establishment: Establishment): Promise<Establishment> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/establishments", establishment, axiosConfig);
        return response.data.content;
    }

    static async deleteEstablishment(id: number): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/establishments/${id}`, axiosConfig);
        return response.data.success;
    }

    static async getEstablishments(): Promise<Establishment[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.get("/establishments", axiosConfig);
        return response.data.content;
    }

}
