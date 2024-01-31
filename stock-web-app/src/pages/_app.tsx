import {AppProps} from 'next/app';
import Menu from '../components/Menu';
import { MenuItem } from '../types/types';
import "../styles/global.scss";
import Head from 'next/head';

const menuItems : MenuItem[] = [{
  label: "MenuItem1",
  link: "#"
},{
  label: "MenuItem2",
  link: "#"
},{
  label: "MenuItem3",
  link: "#"
}];

function App({Component, pageProps}: AppProps) {
  return (
    <>
      <Head>
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>React App</title>
      </Head>
      <div className="app">
        <header>
          <Menu items={menuItems}/>
        </header>
        <Component {...pageProps} />
      </div>
    </>);
}
export default App;