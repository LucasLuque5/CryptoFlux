<script setup>
import { transaccionesApi } from '../Services/api.js'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const transacciones = ref([])
const cargando = ref(true)
const error = ref(null)

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

async function borrar(id) {
  if (confirm('Estas seguro que queres borrar esta transaccion?')) {
    try {
      await transaccionesApi.delete(id)
      transacciones.value = transacciones.value.filter(t => t.id !== id)
    } catch (e) {
      alert('Error al borrar la transaccion.')
      console.error(e)
    }
  }
}

function formatearFecha(fecha) {
  return new Date(fecha).toLocaleString('es-AR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

function formatearMoneda(valor) {
  return new Intl.NumberFormat('es-AR', {
    style: 'currency',
    currency: 'ARS'
  }).format(valor)
}

function tipoBadge(action) {
  return action === 'compra' ? 'compra' : 'venta'
}
</script>

<template>
  <div class="page">
    <div class="page-header">
      <h2>Historial de Transacciones</h2>
      <RouterLink to="/compra" class="btn-new">+ Nueva</RouterLink>
    </div>

    <div v-if="cargando" class="state">
      <div class="spinner"></div>
      <p>Cargando transacciones...</p>
    </div>

    <div v-else-if="error" class="state error">
      <p>{{ error }}</p>
    </div>

    <div v-else-if="transacciones.length === 0" class="state empty">
      <div class="empty-icon">&#128203;</div>
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
              <span :class="['badge', tipoBadge(t.action)]">{{ t.action }}</span>
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
  </div>
</template>

<style scoped>
.page {
  max-width: 900px;
  margin: 0 auto;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.page-header h2 {
  font-size: 1.4rem;
  font-weight: 600;
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

.state {
  text-align: center;
  padding: 4rem 2rem;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 12px;
  color: var(--text-muted);
}

.state.error {
  color: var(--danger);
  background: rgba(248, 81, 73, 0.05);
  border-color: rgba(248, 81, 73, 0.2);
}

.empty-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.btn-link {
  display: inline-block;
  margin-top: 1rem;
  padding: 0.5rem 1rem;
  background: var(--primary);
  color: var(--bg-dark);
  border-radius: 8px;
  font-size: 0.85rem;
  font-weight: 600;
  text-decoration: none;
}

.spinner {
  width: 32px;
  height: 32px;
  border: 3px solid var(--border);
  border-top-color: var(--primary);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  to { transform: rotate(360deg); }
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

.mono {
  font-family: 'SF Mono', 'Fira Code', monospace;
}

.muted {
  color: var(--text-muted);
}

.crypto-cell {
  font-weight: 600;
}

.crypto-code {
  background: rgba(0, 200, 83, 0.1);
  color: var(--primary);
  padding: 0.2rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.badge {
  display: inline-block;
  padding: 0.2rem 0.6rem;
  border-radius: 100px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: capitalize;
}

.badge.compra {
  background: rgba(0, 200, 83, 0.1);
  color: var(--primary);
}

.badge.venta {
  background: rgba(248, 81, 73, 0.1);
  color: var(--danger);
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
  background: rgba(248, 81, 73, 0.15);
  border-color: var(--danger);
  color: var(--danger);
}
</style>
