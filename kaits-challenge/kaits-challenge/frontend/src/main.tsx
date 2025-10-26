import React from 'react'
import { createRoot } from 'react-dom/client'
import './styles/index.css'
import { OrderForm } from './components/OrderForm'
import { OrderList } from './components/OrderList'

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <div className="container my-8">
      <h1 className="text-3xl font-bold mb-6">Kaits – Registrar Pedido</h1>
      <OrderForm />
      <OrderList />
    </div>
  </React.StrictMode>
)
