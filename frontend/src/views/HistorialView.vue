<script setup>
import { transaccionesApi } from '../Services/api.js'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { formatearFecha, formatearMoneda } from '../utils/formato.js'

const transacciones = ref([])
const cargando = ref(true)
const error = ref(null)
const errorBorrado = ref('')

onMounted(async () => {
  try {
    const response = await transaccionesApi.getAll()
    transacciones.value = response.data
  } catch (e) {
    error.value = 'Error al cargar las transacciones.'
    console.error(e)
  } finally {
    cargando.value = false
  }
})

const router = useRouter()

function ver(id) {
  router.push(`/transaccion/${id}`)
}

function editar(id) {
  router.push(`/transaccion/${id}/editar`)
}

const idABorrar = ref(null)

function borrar(id) {
  errorBorrado.value = ''
  idABorrar.value = id
}

function cancelarBorrado() {
  idABorrar.value = null
}

async function confirmarBorrado() {
  try {
    await transaccionesApi.delete(idABorrar.value)
    transacciones.value = transacciones.value.filter(t => t.id !== idABorrar.value)
  } catch (e) {
    errorBorrado.value = 'Error al borrar la transaccion.'
    console.error(e)
  } finally {
    idABorrar.value = null
  }
}
</script>

<template>
  <div class="page">
    <div class="page-header">
      <h2>Historial de Transacciones</h2>
      <RouterLink to="/compra" class="btn-new">+ Nueva</RouterLink>
    </div>

    <p v-if="errorBorrado" class="mensaje error">{{ errorBorrado }}</p>

    <div v-if="cargando" class="state">
      <div class="spinner"></div>
      <p>Cargando transacciones...</p>
    </div>

    <div v-else-if="error" class="state error">
      <p>{{ error }}</p>
    </div>

    <div v-else-if="transacciones.length === 0" class="state empty">
      <p>No hay transacciones registradas</p>
      <RouterLink to="/compra" class="btn-link">Registrar primera compra</RouterLink>
    </div>

    <div v-else class="table-wrapper">
      <table>
        <thead>
          <tr>
            <th>Tipo</th>
            <th>Criptomoneda</th>
            <th>Cantidad</th>
            <th>Fecha</th>
            <th>Monto</th>
            <th class="actions-col">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="t in transacciones" :key="t.id">
            <td>
              <span :class="['badge', t.action]">{{ t.action === 'purchase' ? 'Compra' : 'Venta' }}</span>
            </td>
            <td class="crypto-cell">
              <span class="crypto-code">{{ t.cryptoCode.toUpperCase() }}</span>
            </td>
            <td class="mono">{{ t.cryptoAmount }}</td>
            <td class="muted">{{ formatearFecha(t.dateTime) }}</td>
            <td class="mono">{{ formatearMoneda(t.money) }}</td>
            <td>
              <div class="row-actions">
                <button class="btn-action" @click="ver(t.id)">Ver</button>
                <button class="btn-action" @click="editar(t.id)">Editar</button>
                <button class="btn-action danger" @click="borrar(t.id)">Borrar</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="idABorrar" class="modal-fondo">
      <div class="modal">
        <h3>Borrar transaccion</h3>
        <p>Estas seguro que queres borrar la transaccion #{{ idABorrar }}?</p>
        <div class="modal-acciones">
          <button class="btn-action" @click="cancelarBorrado">Cancelar</button>
          <button class="btn-action danger" @click="confirmarBorrado">Borrar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page {
  max-width: 900px;
  margin: 0 auto;
}

.btn-new {
  padding: 0.5rem 1rem;
  background: var(--primary);
  color: var(--bg-dark);
  border-radius: 8px;
  font-size: 0.85rem;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.2s;
}

.btn-new:hover {
  background: var(--primary-light);
}

.table-wrapper {
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 12px;
  overflow: hidden;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  padding: 0.8rem 1rem;
  text-align: left;
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  background: var(--bg-card-hover);
  border-bottom: 1px solid var(--border);
}

td {
  padding: 0.85rem 1rem;
  border-bottom: 1px solid var(--border);
  font-size: 0.9rem;
}

tr:last-child td {
  border-bottom: none;
}

tr:hover td {
  background: var(--bg-card-hover);
}

.crypto-cell {
  font-weight: 600;
}

.crypto-code {
  background: var(--primary-soft);
  color: var(--primary);
  padding: 0.2rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.actions-col {
  text-align: right;
}

.row-actions {
  display: flex;
  gap: 0.4rem;
  justify-content: flex-end;
}

.btn-action {
  padding: 0.35rem 0.7rem;
  border: 1px solid var(--border);
  border-radius: 6px;
  background: var(--bg-dark);
  color: var(--text);
  cursor: pointer;
  font-size: 0.78rem;
  font-weight: 500;
  font-family: inherit;
  transition: all 0.15s;
}

.btn-action:hover {
  background: var(--bg-card-hover);
  border-color: var(--text-muted);
  color: var(--text);
}

.btn-action.danger:hover {
  background: var(--danger-soft);
  border-color: var(--danger);
  color: var(--danger);
}

.modal-fondo {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal {
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 12px;
  padding: 1.5rem;
  max-width: 400px;
  width: 90%;
}

.modal h3 {
  margin-bottom: 0.5rem;
}

.modal p {
  color: var(--text-muted);
  margin-bottom: 1.25rem;
}

.modal-acciones {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>
