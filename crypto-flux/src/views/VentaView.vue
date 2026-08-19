<script setup>
import { transaccionesApi } from '../Services/api.js'
import { reactive } from 'vue';

const form = reactive({
    cripto: '',
    cantidad: null,
    fecha: '',
})

async function enviarFormulario() {
  if(!form.cantidad || form.cantidad <= 0 || !form.cripto || !form.fecha) {
    alert('Complete todos los campos correctamente.\nLa cantidad debe ser mayor a 0.')
    return;
  }
  try {
    await transaccionesApi.create({
      cryptoCode: form.cripto,
      action: 'venta',
      cryptoAmount: form.cantidad,
      money: 0,
      dateTime: new Date(form.fecha).toISOString()
    })
    alert('Venta registrada correctamente!')
    form.cripto = ''
    form.cantidad = null
    form.fecha = ''
  } catch (error) {
    const msg = error.response?.data?.message || 'Error al registrar la venta.'
    alert(msg)
    console.error(error)
  }
}
</script>

<template>
  <div class="page">
    <div class="card">
      <div class="card-header">
        <div class="card-icon red">&#128176;</div>
        <div>
          <h2>Registrar Venta</h2>
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

        <div class="field">
          <label>Cantidad</label>
          <input v-model="form.cantidad" type="number" step="0.00000001" placeholder="ej: 0.00070" />
        </div>

        <div class="field">
          <label>Fecha y hora</label>
          <input v-model="form.fecha" type="datetime-local" />
        </div>

        <button type="submit" class="btn-submit red">Registrar Venta</button>
      </form>
    </div>
  </div>
</template>

<style scoped>
.page {
  display: flex;
  justify-content: center;
  padding-top: 2rem;
}

.card {
  width: 100%;
  max-width: 520px;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 16px;
  padding: 2rem;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 2rem;
}

.card-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  flex-shrink: 0;
}

.card-icon.red {
  background: rgba(248, 81, 73, 0.1);
}

.card-header h2 {
  font-size: 1.3rem;
  font-weight: 600;
  margin: 0;
}

.card-desc {
  font-size: 0.85rem;
  color: var(--text-muted);
  margin: 0.2rem 0 0 0;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.field-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

label {
  font-size: 0.85rem;
  font-weight: 500;
  color: var(--text-muted);
}

select, input {
  padding: 0.7rem 0.9rem;
  background: var(--bg-dark);
  border: 1px solid var(--border);
  border-radius: 8px;
  font-size: 0.95rem;
  color: var(--text);
  font-family: inherit;
  transition: border-color 0.2s;
}

select:focus, input:focus {
  outline: none;
  border-color: var(--danger);
}

select option {
  background: var(--bg-dark);
  color: var(--text);
}

.btn-submit {
  padding: 0.8rem;
  border: none;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  margin-top: 0.5rem;
  font-family: inherit;
}

.btn-submit.red {
  background: var(--danger);
  color: white;
}

.btn-submit.red:hover {
  background: var(--danger-dark);
  transform: translateY(-1px);
}
</style>
