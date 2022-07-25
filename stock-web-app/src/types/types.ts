export interface MenuItem {
    label: string;
    link: string;
}

export interface Company {
    name: string;
    symbol: string;
    exchange: string;
}

export interface BasicStock {
    company: Company,
    stockPrice: number,
    currency: string,
    changePoint: number,
    changePercentage: number,
} 