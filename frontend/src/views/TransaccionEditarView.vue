<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { transaccionesApi } from '../Services/api.js'

const route = useRoute()
const router = useRouter()

const form = ref({
  cryptoCode: '',
  action: '',
  cryptoAmount: null,
  money: null,
  dateTime: ''
})

onMounted(async () => {
  try {
    const response = await transaccionesApi.getById(route.params.id)
    const t = response.data
    form.value = {
      cryptoCode: t.cryptoCode,
      action: t.action,
      cryptoAmount: t.cryptoAmount,
      money: t.money,
      dateTime: new Date(t.dateTime).toISOString().slice(0, 16)
    }
  } catch (error) {
    alert('Error al cargar la transaccion.')
  }
})

async function guardarCambios() {
  try {
    await transaccionesApi.update(route.params.id,
     { id: parseInt(route.params.id), ...form.value,
        dateTime: new Date(form.value.dateTime).toISOString()
     })
    alert('Transaccion actualizada correctamente!')
    router.push('/historial')
  } catch (error) {
    alert('Error al actualizar la transaccion.')
    console.error(error)
  }
}
</script>

<template>
  <div class="page">
    <div class="card">
      <div class="card-header">
        <div class="card-icon">&#9998;</div>
        <div>
          <h2>Editar Transaccion</h2>
          <p class="card-desc">Modifica los datos de la transaccion #{{ route.params.id }}</p>
        </div>
      </div>

      <form @submit.prevent="guardarCambios" class="form">
        <div class="field">
          <label>Criptomoneda</label>
          <select v-model="form.cryptoCode">
            <option value="btc">Bitcoin (BTC)</option>
            <option value="eth">Ethereum (ETH)</option>
            <option value="usdc">USD Coin (USDC)</option>
          </select>
        </div>

        <div class="field-row">
          <div class="field">
            <label>Cantidad</label>
            <input v-model="form.cryptoAmount" type="number" step="0.00000001" />
          </div>
          <div class="field">
            <label>Monto (ARS)</label>
            <input v-model="form.money" type="number" step="0.01" />
          </div>
        </div>

        <div class="field">
          <label>Fecha y hora</label>
          <input v-model="form.dateTime" type="datetime-local" />
        </div>

        <div class="form-actions">
          <button type="button" @click="router.back()" class="btn-cancel">Cancelar</button>
          <button type="submit" class="btn-save">Guardar Cambios</button>
        </div>
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
  background: rgba(0, 200, 83, 0.1);
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
  border-color: var(--primary);
}

select option {
  background: var(--bg-dark);
  color: var(--text);
}

.form-actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.5rem;
}

.btn-cancel, .btn-save {
  flex: 1;
  padding: 0.75rem;
  border: none;
  border-radius: 10px;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  font-family: inherit;
}

.btn-cancel {
  background: var(--bg-dark);
  color: var(--text);
  border: 1px solid var(--border);
}

.btn-cancel:hover {
  background: var(--bg-card-hover);
}

.btn-save {
  background: var(--primary);
  color: var(--bg-dark);
}

.btn-save:hover {
  background: var(--primary-light);
}
</style>
