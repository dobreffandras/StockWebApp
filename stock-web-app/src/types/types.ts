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
    price: number,
    currency: string,
    changePoint: number,
    changePercent: number,
    previousClose: number,
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

export type NotLoaded = { type: "notloaded"};
export type LoadingInProgress = { type: "loadinginprogress"};
export type Loaded<T> = {type: "loaded", data: T};
export type LoadingFailed = {type: "loadingfailed", error: string};

export type Loadable<T> = NotLoaded | LoadingInProgress | Loaded<T> | LoadingFailed;