// frontend/src/components/OrderForm.tsx
import { useEffect, useMemo, useState } from 'react'
import { useForm, useFieldArray } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { api } from '../api/client'
import { SuccessModal } from './SuccessModal'

const itemSchema = z.object({
  productId: z.number().min(1, 'Seleccione producto'),
  quantity: z.number().min(1, 'Cantidad > 0'),
  unitPrice: z.number().min(0.01, 'Precio > 0'),
})

const schema = z.object({
  customerId: z.number().min(1, 'Seleccione cliente'),
  items: z.array(itemSchema).min(1, 'Agregue al menos un producto'),
})

type FormData = z.infer<typeof schema>

export function OrderForm() {
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
    watch,
    setValue,
    reset,
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { customerId: 0, items: [{ productId: 0, quantity: 1, unitPrice: 0 }] },
  })

  const { fields, append, remove } = useFieldArray({ control, name: 'items' })

  const [customers, setCustomers] = useState<any[]>([])
  const [products, setProducts] = useState<any[]>([])

  const [modalOpen, setModalOpen] = useState(false)
  const [modalMsg, setModalMsg] = useState('')

  useEffect(() => {
    api.get('/lookups/customers').then((r) => setCustomers(r.data))
    api.get('/lookups/products').then((r) => setProducts(r.data))
  }, [])

  // Autocompletar precio con el unitPrice del producto al seleccionar
  const items = watch('items')
  useEffect(() => {
    items.forEach((it, idx) => {
      if (it.productId && it.unitPrice === 0) {
        const prod = products.find((p: any) => p.id === it.productId)
        if (prod) setValue(`items.${idx}.unitPrice`, prod.unitPrice)
      }
    })
  }, [items, products, setValue])

  const total = useMemo(
    () => items.reduce((acc: number, it: any) => acc + it.quantity * it.unitPrice, 0),
    [items]
  )

  const onSubmit = async (data: FormData) => {
    const res = await api.post('/orders', data)

    const createdId = Number(res.data?.orderId)
    const customer = customers.find((c: any) => c.id === data.customerId)
    const prettyTotal = total.toFixed(2)

    setModalMsg(
      `✅ Pedido #${createdId} registrado para ${customer?.fullName ?? 'cliente'}.`
    )
    setModalOpen(true)

    reset({ customerId: 0, items: [{ productId: 0, quantity: 1, unitPrice: 0 }] })

    // Notificar al listado de pedidos para que se refresque
    window.dispatchEvent(new CustomEvent('orders:refresh'))
  }

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)} className="card space-y-6">
        <div className="grid sm:grid-cols-2 gap-4">
          <label className="label">Cliente</label>
          <div>
            <select className="select" {...register('customerId', { valueAsNumber: true })}>
              <option value={0}>-- Seleccione --</option>
              {customers.map((c: any) => (
                <option key={c.id} value={c.id}>
                  {c.fullName} ({c.dni})
                </option>
              ))}
            </select>
            {errors.customerId && <p className="error mt-1">{errors.customerId.message}</p>}
          </div>
        </div>

        <div>
          <div className="flex items-center justify-between mb-3">
            <h3 className="text-lg font-semibold">Productos</h3>
            <button
              type="button"
              className="button"
              onClick={() => append({ productId: 0, quantity: 1, unitPrice: 0 })}
            >
              + Agregar producto
            </button>
          </div>

          <div className="space-y-3">
            {fields.map((f, idx) => (
              <div
                key={f.id}
                // 4 columnas explícitas: 6fr | 2fr | 3fr | auto (botón)
                className="grid md:[grid-template-columns:minmax(0,6fr)_minmax(0,2fr)_minmax(0,3fr)_auto]
                           md:auto-cols-fr md:grid-flow-col gap-4 items-end
                           bg-gray-50 border border-gray-200 rounded-xl p-3"
              >
                {/* Producto */}
                <div>
                  <label className="label">Producto</label>
                  <select
                    className="select"
                    {...register(`items.${idx}.productId` as const, { valueAsNumber: true })}
                  >
                    <option value={0}>-- Seleccione --</option>
                    {products.map((p: any) => (
                      <option key={p.id} value={p.id}>
                        {p.description}
                      </option>
                    ))}
                  </select>
                  {errors.items?.[idx]?.productId && (
                    <p className="error mt-1">
                      {(errors.items?.[idx]?.productId as any)?.message}
                    </p>
                  )}
                </div>

                {/* Cantidad */}
                <div>
                  <label className="label">Cantidad</label>
                  <input
                    className="input"
                    type="number"
                    min="1"
                    step="1"
                    {...register(`items.${idx}.quantity` as const, { valueAsNumber: true })}
                  />
                  {errors.items?.[idx]?.quantity && (
                    <p className="error mt-1">
                      {(errors.items?.[idx]?.quantity as any)?.message}
                    </p>
                  )}
                </div>

                {/* Precio */}
                <div>
                  <label className="label">Precio</label>
                  <input
                    className="input"
                    type="number"
                    min="0.01"
                    step="0.01"
                    {...register(`items.${idx}.unitPrice` as const, { valueAsNumber: true })}
                  />
                  {errors.items?.[idx]?.unitPrice && (
                    <p className="error mt-1">
                      {(errors.items?.[idx]?.unitPrice as any)?.message}
                    </p>
                  )}
                </div>

                {/* Eliminar */}
                <div className="flex items-end justify-end md:ml-2">
                  <button
                    type="button"
                    className="button-secondary whitespace-nowrap"
                    onClick={() => remove(idx)}
                  >
                    Eliminar
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="flex items-center justify-between">
          <div>
            <p className="font-semibold">
              Total (estimado cliente):{' '}
              <span className="text-indigo-600">{total.toFixed(2)}</span>
            </p>
            <p className="help">El total definitivo se calcula en el servidor.</p>
          </div>
          <button className="button" type="submit">
            Registrar Pedido
          </button>
        </div>
      </form>

      <SuccessModal
        open={modalOpen}
        title="¡Pedido registrado!"
        message={modalMsg}
        onClose={() => setModalOpen(false)}
      />
    </>
  )
}
