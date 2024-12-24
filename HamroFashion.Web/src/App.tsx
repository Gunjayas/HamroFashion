import * as React from 'react';
import { Route, Routes } from "react-router-dom";
import HomePage from './components/Home/HomePage';
import ProductForm from './components/Product/ProductForm';
import SignInPage from './components/Members/SignInPage';
import SignUpPage from './components/Members/SignUpPage';
import ProductDetailPage from './components/Product/ProductDetailPage';
import CartDetailPage from './components/Product/CartDetailPage';

const App: React.FC = () => {
return <div>
    <div>
      <Routes>
        <Route path="/" element={<HomePage />}> </Route>
        <Route path='/product/:productId/form' element={<ProductForm />}></Route>
        <Route path='/signin' element={<SignInPage />}></Route>
        <Route path='/signup' element={<SignUpPage />}></Route>
        <Route path='/product/:productId/detail' element={<ProductDetailPage />}></Route>
        <Route path='/cart/detail' element={<CartDetailPage />}></Route>
      </Routes>
    </div>
    
   </div>
}

export default App;

