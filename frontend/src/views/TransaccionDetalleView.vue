<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { transaccionesApi } from '../Services/api.js'
import { formatearFecha, formatearMoneda } from '../utils/formato.js'

const route = useRoute()
const router = useRouter()
const transaccion = ref(null)
const error = ref('')

onMounted(async () => {
  try {
    const response = await transaccionesApi.getById(route.params.id)
    transaccion.value = response.data
  } catch (e) {
    error.value = 'Error al cargar la transaccion.'
    console.error(e)
  }
})
</script>

<template>
  <div class="page-centrada">
    <div class="card">
      <div v-if="transaccion">
        <div class="card-header">
          <div :class="['card-icon', transaccion.action === 'purchase' ? 'green' : 'red']">
            {{ transaccion.action === 'purchase' ? '+' : '−' }}
          </div>
          <div>
            <div class="header-row">
              <h2>Detalle de Transaccion</h2>
              <span :class="['badge', transaccion.action]">{{ transaccion.action === 'purchase' ? 'Compra' : 'Venta' }}</span>
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

        <div class="acciones">
          <button @click="router.back()" class="btn-secundario">Volver</button>
          <RouterLink :to="`/transaccion/${route.params.id}/editar`" class="btn-primario">Editar</RouterLink>
        </div>
      </div>

      <p v-else-if="error" class="mensaje error">{{ error }}</p>

      <div v-else class="state">
        <div class="spinner"></div>
        <p>Cargando...</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.card-icon.green {
  background: var(--success-soft);
  color: var(--success);
}

.card-icon.red {
  background: var(--danger-soft);
  color: var(--danger);
}

.header-row {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.header-row h2 {
  font-size: 1.3rem;
  font-weight: 600;
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
</style>
