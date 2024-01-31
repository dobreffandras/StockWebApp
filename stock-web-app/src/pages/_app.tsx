import {AppProps} from 'next/app';
import Menu from '../components/Menu';
import { MenuItem } from '../types/types';
import "../styles/global.scss";

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
    <div className="app">
    <header>
      <Menu items={menuItems}/>
    </header>
    <Component {...pageProps} />
  </div>);
}
export default App;