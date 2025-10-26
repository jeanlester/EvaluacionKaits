import { useEffect, useState } from 'react'
import { api } from '../api/client'

type Detail = { productId:number; productDescription:string; quantity:number; unitPrice:number; subtotal:number }
type Order = { id:number; orderDate:string; customer:string; total:number; details: Detail[] }

export function OrderList() {
  const [orders, setOrders] = useState<Order[]>([])

  // Función de carga reutilizable
  const load = async () => {
    const r = await api.get('/orders')
    setOrders(r.data)
  }

  useEffect(() => {
    load()

    const handler = () => load()
    window.addEventListener('orders:refresh', handler)

    return () => window.removeEventListener('orders:refresh', handler)
  }, [])

  return (
    <div className="card mt-6">
      <h2 className="text-lg font-semibold mb-3">🧾 Pedidos registrados</h2>
      {orders.length === 0 ? (
        <p className="help">Aún no hay pedidos.</p>
      ) : (
        <div className="overflow-x-auto">
          <table className="min-w-full table-auto">
            <thead>
              <tr className="text-left text-gray-700">
                <th className="py-2 pr-4">ID</th>
                <th className="py-2 pr-4">Fecha</th>
                <th className="py-2 pr-4">Cliente</th>
                <th className="py-2 pr-4">Total</th>
              </tr>
            </thead>
            <tbody>
              {orders.map(o => (
                <tr key={o.id} className="border-t align-top">
                  <td className="py-2 pr-4">{o.id}</td>
                  <td className="py-2 pr-4">{new Date(o.orderDate).toLocaleString()}</td>
                  <td className="py-2 pr-4">{o.customer}</td>
                  <td className="py-2 pr-4 font-semibold">{o.total.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}
