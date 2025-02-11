import {getAxiosConfig} from "@utils/ApiUtils.ts";
import axios from "axios";
import type {Personnel} from "@/types/Interfaces.ts";

export class PersonnelService {

    static async addPersonnel(personnel: Personnel): Promise<Personnel> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.post("/personnel", personnel, axiosConfig);
        return response.data.content;
    }

    static async deletePersonnel(id: string): Promise<boolean> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.delete(`/personnel/${id}`, axiosConfig);
        return response.data.success;
    }

    static async getPersonnel(): Promise<Personnel[]> {
        const axiosConfig = getAxiosConfig();
        const response = await axios.get("/personnel", axiosConfig);
        return response.data.content;
    }

}
