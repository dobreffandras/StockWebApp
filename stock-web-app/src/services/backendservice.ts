import { Company } from "../types/types";

class Backendservice {
    async fetchCompanies() : Promise<Company[]> {
        return await fetch(`${process.env.REACT_APP_BACKEND_HOST}/companies`).then(res => res.json())
    }
}

export default Backendservice;