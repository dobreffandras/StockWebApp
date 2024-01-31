import Document, { Head, Html, Main, NextScript, } from "next/document"

export default class StockWebAppDocument extends Document {
    render(){
        return (
            <Html lang="en">
                <Head>
                    <meta charSet="utf-8" />
                    <link rel="icon" href="/favicon.ico" />
                    <meta name="theme-color" content="#000000" />
                    <link rel="manifest" href="/manifest.json" />
                </Head>
                <body>
                    <Main />
                    <NextScript/>
                </body>
            </Html>
        );
    }
}