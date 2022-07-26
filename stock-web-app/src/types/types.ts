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
    changePercent: number,
}

export interface Stock {
    company: Company,
    stockPrice: number,
    currency: string,
    changePoint: number,
    changePercent: number,
    PreviousClose: number,
        open: number,
        marketCap: number,
        dailyRange: {
            low: number,
            high: number
        },
        yearlyRange: {
            low: number,
            high: number,
        },
    dividend: number | undefined,
    dividendYield: number | undefined
} 