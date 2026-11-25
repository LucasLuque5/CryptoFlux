<script setup>
import { reactive, ref } from 'vue'
import { transaccionesApi } from '../Services/api.js'

const props = defineProps({
  accion: { type: String, required: true }
})

const esCompra = props.accion === 'purchase'
const titulo = esCompra ? 'Registrar Compra' : 'Registrar Venta'

const form = reactive({
  cripto: '',
  cantidad: null,
  fecha: ''
})

const mensaje = ref('')
const tipoMensaje = ref('')

function mostrarMensaje(texto, tipo) {
  mensaje.value = texto
  tipoMensaje.value = tipo
}

async function enviarFormulario() {
  if (!form.cripto || !form.fecha || !form.cantidad || form.cantidad <= 0) {
    mostrarMensaje('Completa todos los campos. La cantidad debe ser mayor a 0.', 'error')
    return
  }

  try {
    await transaccionesApi.create({
      cryptoCode: form.cripto,
      action: props.accion,
      cryptoAmount: form.cantidad,
      dateTime: form.fecha
    })
    mostrarMensaje(esCompra ? 'Compra registrada correctamente.' : 'Venta registrada correctamente.', 'exito')
    form.cripto = ''
    form.cantidad = null
    form.fecha = ''
  } catch (error) {
    mostrarMensaje(error.response?.data?.message || 'Error al registrar la transaccion.', 'error')
    console.error(error)
  }
}
</script>

<template>
  <div class="page-centrada">
    <div :class="['card', accion]">
      <div class="card-header">
        <div class="card-icon">{{ esCompra ? '+' : '−' }}</div>
        <div>
          <h2>{{ titulo }}</h2>
          <p class="card-desc">El monto se calcula automaticamente segun el precio actual</p>
        </div>
      </div>

      <form @submit.prevent="enviarFormulario" class="form">
        <div class="field">
          <label>Criptomoneda</label>
          <select v-model="form.cripto">
            <option value="">Seleccionar criptomoneda</option>
            <option value="btc">Bitcoin (BTC)</option>
            <option value="eth">Ethereum (ETH)</option>
            <option value="usdc">USD Coin (USDC)</option>
          </select>
        </div>

        <div class="field-row">
          <div class="field">
            <label>Cantidad</label>
            <input v-model="form.cantidad" type="number" step="0.00000001" placeholder="ej: 0.00070" />
          </div>
          <div class="field">
            <label>Fecha y hora</label>
            <input v-model="form.fecha" type="datetime-local" />
          </div>
        </div>

        <p v-if="mensaje" :class="['mensaje', tipoMensaje]">{{ mensaje }}</p>

        <button type="submit" class="btn-submit">{{ titulo }}</button>
      </form>
    </div>
  </div>
</template>

<style scoped>
.card {
  --color: var(--success);
  --color-soft: var(--success-soft);
  border-top: 3px solid var(--color);
}

.card.sale {
  --color: var(--danger);
  --color-soft: var(--danger-soft);
}

.card-icon {
  background: var(--color-soft);
  color: var(--color);
}

select:focus, input:focus {
  border-color: var(--color);
}

.btn-submit {
  padding: 0.8rem;
  border: none;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  margin-top: 0.5rem;
  font-family: inherit;
  background: var(--color);
  color: var(--bg-dark);
}

.btn-submit:hover {
  opacity: 0.85;
}
</style>
