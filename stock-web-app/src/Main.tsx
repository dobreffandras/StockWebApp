import { Routes, Route } from 'react-router-dom';

import Dashboard from './components/Dashboard';
import StockDetails from './components/StockDetails';

const Main = () => {
  return (
    <Routes>
      <Route path='/' element={<Dashboard />}></Route>
      <Route path='/:symbol' element={<StockDetails />}></Route>
    </Routes>
  );
}

export default Main;