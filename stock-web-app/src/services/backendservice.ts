import { Company } from "../types/types";

class Backendservice {
    async fetchCompanies() : Promise<Company[]> {
        return await fetch("https://localhost:3001/companies").then(res => res.json())
    }
}

export default Backendservice;