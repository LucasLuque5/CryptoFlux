<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { transaccionesApi } from '../Services/api.js'

const route = useRoute()
const router = useRouter()
const transaccion = ref(null)

onMounted(async () => {
  try {
    const response = await transaccionesApi.getById(route.params.id)
    transaccion.value = response.data
  } catch (error) {
    alert('Error al cargar la transacción.')
    console.error(error)
  }
})

function formatearFecha(fecha){
  return new Date (fecha).toLocaleString('es-AR',{
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<template>
 <div class="layout-principal">
    <div class="contenedor">
        <h2>Detalle de Transacción</h2>

        <div v-if="transaccion" class="detalle">
          <p><strong>Tipo:</strong> {{ transaccion.action }}</p>
          <p><strong>Criptomoneda:</strong> {{ transaccion.cryptoCode }}</p>
          <p><strong>Cantidad:</strong> {{ transaccion.cryptoAmount }}</p>
          <p><strong>Monto:</strong> {{ transaccion.money }}</p>
          <p><strong>Fecha y hora:</strong> {{formatearFecha(transaccion.dateTime) }}</p>
        </div>

        <div v-else>
          <p>Cargando...</p>
        </div>

        <button @click="router.back()">Volver</button>
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

.layout-principal {
  display: flex;
  justify-content: center; 
  align-items: center;     
  gap: 80px;               
  max-width: 1200px;
  margin: 80px auto;       
  padding: 0 20px;
}
</style>