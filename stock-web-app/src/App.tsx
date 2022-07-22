import './App.css';
import Menu from './components/Menu';
import { MenuItem } from './types/types';

function App() {

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

  return (
    <div className="App">
      <header className="App-header">
        <Menu items={menuItems}/>
      </header>
    </div>
  );
}

export default App;
