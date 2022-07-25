import { BasicStock } from "../types/types";

class Backendservice {
    async fetchCompanies() : Promise<BasicStock[]> {
        return await fetch(`${process.env.REACT_APP_BACKEND_HOST}/stocks`).then(res => res.json())
    }
}

export default Backendservice;