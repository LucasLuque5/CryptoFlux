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

const error = ref('')

onMounted(async () => {
  try {
    const response = await transaccionesApi.getById(route.params.id)
    const t = response.data
    form.value = {
      cryptoCode: t.cryptoCode,
      action: t.action,
      cryptoAmount: t.cryptoAmount,
      money: t.money,
      dateTime: t.dateTime.slice(0, 16)
    }
  } catch (e) {
    error.value = 'Error al cargar la transaccion.'
    console.error(e)
  }
})

async function guardarCambios() {
  error.value = ''
  try {
    await transaccionesApi.update(route.params.id, form.value)
    router.push('/historial')
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al actualizar la transaccion.'
    console.error(e)
  }
}
</script>

<template>
  <div class="page-centrada">
    <div class="card">
      <div class="card-header">
        <div>
          <h2>Editar Transaccion</h2>
          <p class="card-desc">Modifica los datos de la transaccion #{{ route.params.id }}</p>
        </div>
      </div>

      <form @submit.prevent="guardarCambios" class="form">
        <div class="field">
          <label>Tipo</label>
          <select v-model="form.action">
            <option value="purchase">Compra</option>
            <option value="sale">Venta</option>
          </select>
        </div>

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

        <p v-if="error" class="mensaje error">{{ error }}</p>

        <div class="acciones">
          <button type="button" @click="router.back()" class="btn-secundario">Cancelar</button>
          <button type="submit" class="btn-primario">Guardar Cambios</button>
        </div>
      </form>
    </div>
  </div>
</template>

