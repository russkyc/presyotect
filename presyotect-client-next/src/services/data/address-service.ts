import axios from "axios";
import type {CityMunicipality, Province} from "@/types/Interfaces.ts";

export class AddressService {

    static async getProvinces(): Promise<Province[]> {
        const response = await axios.get("/data/provinces.json");
        return response.data as Province[];
    }
    
    static async GetCitiesMunicipalitiesByProvince(provinceCode: string): Promise<CityMunicipality[]> {
        const response = await axios.get("/data/cities-municipalities.json");
        const citiesMunicipalities = response.data as CityMunicipality[];
        const provinceCitiesMunicipalities = citiesMunicipalities.filter(c => c.provinceCode === provinceCode);
        console.log(`${provinceCode}: ${provinceCitiesMunicipalities}`);
        return provinceCitiesMunicipalities;
    }
}