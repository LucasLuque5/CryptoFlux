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
    alert('Error al cargar la transaccion.')
    console.error(error)
  }
})

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
</script>

<template>
  <div class="page">
    <div class="card">
      <div v-if="transaccion">
        <div class="card-header">
          <div :class="['card-icon', transaccion.action === 'compra' ? 'green' : 'red']">
            {{ transaccion.action === 'compra' ? '&#128176;' : '&#128176;' }}
          </div>
          <div>
            <div class="header-row">
              <h2>Detalle de Transaccion</h2>
              <span :class="['badge', transaccion.action]">{{ transaccion.action }}</span>
            </div>
            <p class="card-desc">ID #{{ transaccion.id }}</p>
          </div>
        </div>

        <div class="detail-grid">
          <div class="detail-item">
            <span class="detail-label">Criptomoneda</span>
            <span class="detail-value crypto">{{ transaccion.cryptoCode.toUpperCase() }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Cantidad</span>
            <span class="detail-value mono">{{ transaccion.cryptoAmount }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Monto</span>
            <span class="detail-value mono">{{ formatearMoneda(transaccion.money) }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Fecha y hora</span>
            <span class="detail-value">{{ formatearFecha(transaccion.dateTime) }}</span>
          </div>
        </div>

        <div class="card-actions">
          <button @click="router.back()" class="btn-back">Volver</button>
          <RouterLink :to="`/transaccion/${route.params.id}/editar`" class="btn-edit">Editar</RouterLink>
        </div>
      </div>

      <div v-else class="state">
        <div class="spinner"></div>
        <p>Cargando...</p>
      </div>
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

.card-icon.green {
  background: rgba(0, 200, 83, 0.1);
}

.card-icon.red {
  background: rgba(248, 81, 73, 0.1);
}

.header-row {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.header-row h2 {
  font-size: 1.3rem;
  font-weight: 600;
  margin: 0;
}

.card-desc {
  font-size: 0.85rem;
  color: var(--text-muted);
  margin: 0.2rem 0 0 0;
}

.badge {
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

.detail-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.25rem;
  margin-bottom: 2rem;
}

.detail-item {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.detail-label {
  font-size: 0.8rem;
  color: var(--text-muted);
  font-weight: 500;
}

.detail-value {
  font-size: 1.1rem;
  font-weight: 600;
}

.detail-value.crypto {
  color: var(--primary);
}

.detail-value.mono {
  font-family: 'SF Mono', 'Fira Code', monospace;
}

.card-actions {
  display: flex;
  gap: 0.75rem;
}

.btn-back, .btn-edit {
  flex: 1;
  padding: 0.7rem;
  border: none;
  border-radius: 10px;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  text-align: center;
  text-decoration: none;
  transition: all 0.2s;
  font-family: inherit;
}

.btn-back {
  background: var(--bg-dark);
  color: var(--text);
  border: 1px solid var(--border);
}

.btn-back:hover {
  background: var(--bg-card-hover);
}

.btn-edit {
  background: var(--primary);
  color: var(--bg-dark);
}

.btn-edit:hover {
  background: var(--primary-light);
}

.state {
  text-align: center;
  padding: 3rem;
  color: var(--text-muted);
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
</style>
