<script setup>
import { transaccionesApi } from '../Services/api.js'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const transacciones = ref([])

onMounted(async () => {
  try {
    const response = await transaccionesApi.getAll()
    transacciones.value = response.data
  } catch (error) {
    console.error('Error al cargar transacciones:', error)
  }
})

const router = useRouter()

function ver(id) {
  router.push(`/transaccion/${id}`)
}

function editar(id) {
  router.push(`/transaccion/${id}/editar`)
}

async function borrar(id) {
  if (confirm('¿Estás seguro que querés borrar esta transacción?')) {
    try {
      await transaccionesApi.delete(id)
      transacciones.value = transacciones.value.filter(t => t.id !== id)
    } catch (error) {
      alert('Error al borrar la transacción.')
      console.error(error)
    }
  }
}

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
  <div class="contenedor">
    <h2>Historial de Transacciones</h2>

    <table>
      <thead>
        <tr>
          <th>Tipo</th>
          <th>Criptomoneda</th>
          <th>Cantidad</th>
          <th>Fecha y hora</th>
          <th>Monto</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="transaccion in transacciones" :key="transaccion.id">
          <td>{{ transaccion.action }}</td>
          <td>{{ transaccion.cryptoCode }}</td>
          <td>{{ transaccion.cryptoAmount }}</td>
          <td>{{formatearFecha( transaccion.dateTime) }}</td>
          <td>{{ transaccion.money }}</td>
          <td>
            <button @click="ver(transaccion.id)">Ver</button>
            <button @click="editar(transaccion.id)">Editar</button>
            <button @click="borrar(transaccion.id)">Borrar</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.contenedor {
  max-width: 800px;
  margin: 40px auto;
  padding: 2rem;
}

h2 {
  margin-bottom: 1.5rem;
  color:#046e24;
  text-shadow: 
  0 0 5px rgb(50, 247, 66),
  0 0 15px rgb(0, 0, 0),
  0 0 35px rgb(0, 0, 0);
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  background-color: #1a1a2e;
  color: white;
  padding: 10px;
  text-align: left;
}

td {
  padding: 10px;
  border-bottom: 1px solid #ddd;
}

tr:hover {
  background-color: #f5f5f5;
}
</style>