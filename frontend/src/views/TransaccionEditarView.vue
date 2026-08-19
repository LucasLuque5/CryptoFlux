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
    alert('Error al cargar la transacción.')
  }
})

async function guardarCambios() {
  try {
    await transaccionesApi.update(route.params.id,
     {id: parseInt(route.params.id), ...form.value,
        dataTime: new Date(form.value.dateTime).toISOString()
     })

    alert('¡Transacción actualizada correctamente!')
    router.push('/Historial')
    
  } catch (error) {
    alert('Error al actualizar la transacción.')
    console.error(error)
  }
}
</script>

<template>
  <div class="layout-principal">

    <div class="contenedor">
        <h2>Editar Transacción</h2>
  
        <form @submit.prevent="guardarCambios">
  
          <div class="campo">
            <label>Criptomoneda</label>
            <select v-model="form.cryptoCode">
              <option value="btc">Bitcoin (BTC)</option>
              <option value="eth">Ethereum (ETH)</option>
              <option value="usdc">USD Coin (USDC)</option>
            </select>
          </div>
  
          <div class="campo">
            <label>Cantidad</label>
            <input
              v-model="form.cryptoAmount"
              type="number"
              step="0.00000001"
            />
          </div>
  
          <div class="campo">
            <label>Monto (ARS)</label>
            <input v-model="form.money" type="number" step="0.01" />
          </div>
  
          <div class="campo">
            <label>Fecha y hora</label>
            <input v-model="form.dateTime" type="datetime-local" />
          </div>
  
          <button type="submit">Guardar Cambios</button>
          <button type="button" @click="router.back()">Cancelar</button>
  
        </form>
      </div>

       <div class="bienvenida">
      <h1>₿ CryptoFlux</h1>
      <p>Tu billetera de criptomonedas personal</p>
    </div>
  </div>
</template>

<style scoped>
.contenedor {
  width: 100%;
  max-width: 450px;
  margin: 0;               
  padding: 2rem;
  border: 2px solid #046e24;
  border-radius: 12px;
}

.campo {
  display: flex;
  flex-direction: column;
  margin-bottom: 1rem;
}

.layout-principal {
  display: flex;
  justify-content: center; 
  align-items: center;     
  gap: 80px;               
  max-width: 1200px;
  margin: 80px auto;       
  padding: 0 20px;
}

select, input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 1rem;
}

.bienvenida h1 {
  font-size: 3rem;
  color: #046414;
  margin-bottom: 1rem;
}

.bienvenida p {
  font-size: 1.2rem;
  color: #666;
}

</style>