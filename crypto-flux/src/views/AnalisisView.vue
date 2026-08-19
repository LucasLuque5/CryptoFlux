<script setup>
import { ref, onMounted } from 'vue'
import { carteraApi } from '../Services/api.js'

const cartera = ref(null)
const cargando = ref(true)
const error = ref(null)

onMounted(async () => {
  try {
    const response = await carteraApi.obtener()
    cartera.value = response.data
  } catch (e) {
    error.value = 'Error al cargar los datos de la cartera.'
    console.error(e)
  } finally {
    cargando.value = false
  }
})

function formatearMoneda(valor) {
  return new Intl.NumberFormat('es-AR', {
    style: 'currency',
    currency: 'ARS'
  }).format(valor)
}

function formatearCantidad(cantidad) {
  return new Intl.NumberFormat('es-AR', {
    minimumFractionDigits: 8,
    maximumFractionDigits: 8
  }).format(cantidad)
}

function nombreCripto(codigo) {
  const nombres = {
    BTC: 'Bitcoin',
    ETH: 'Ethereum',
    USDC: 'USD Coin',
    USDT: 'Tether',
    DAI: 'Dai',
    BNB: 'BNB',
    SOL: 'Solana',
    XRP: 'Ripple',
    ADA: 'Cardano',
    DOGE: 'Dogecoin'
  }
  return nombres[codigo.toUpperCase()] || codigo
}

function colorCripto(codigo) {
  const colores = {
    BTC: '#f7931a',
    ETH: '#627eea',
    USDC: '#2775ca',
    USDT: '#26a17b',
    DAI: '#f5ac37',
    SOL: '#9945ff',
    XRP: '#23292f',
    ADA: '#0033ad',
    DOGE: '#c2a633',
    BNB: '#f3ba2f'
  }
  return colores[codigo.toUpperCase()] || '#00c853'
}
</script>

<template>
  <div class="page">
    <div class="page-header">
      <h2>Analisis de Cartera</h2>
    </div>

    <div v-if="cargando" class="state">
      <div class="spinner"></div>
      <p>Cargando datos de la cartera...</p>
    </div>

    <div v-else-if="error" class="state error">
      <p>{{ error }}</p>
    </div>

    <div v-else-if="cartera && cartera.criptomonedas.length === 0" class="state empty">
      <div class="empty-icon">&#128176;</div>
      <p>No tenes criptomonedas en tu cartera</p>
      <RouterLink to="/compra" class="btn-link">Registrar primera compra</RouterLink>
    </div>

    <div v-else>
      <div class="total-card">
        <div class="total-label">Valor total de la cartera</div>
        <div class="total-amount">{{ formatearMoneda(cartera.totalCartera) }}</div>
        <div class="total-count">{{ cartera.criptomonedas.length }} criptomonedas</div>
      </div>

      <div class="crypto-list">
        <div v-for="item in cartera.criptomonedas" :key="item.cryptoCode" class="crypto-card">
          <div class="crypto-left">
            <div class="crypto-avatar" :style="{ background: colorCripto(item.cryptoCode) + '20', color: colorCripto(item.cryptoCode) }">
              {{ item.cryptoCode.toUpperCase().charAt(0) }}
            </div>
            <div class="crypto-info">
              <span class="crypto-name">{{ nombreCripto(item.cryptoCode) }}</span>
              <span class="crypto-symbol">{{ item.cryptoCode.toUpperCase() }}</span>
            </div>
          </div>
          <div class="crypto-right">
            <div class="crypto-amount mono">{{ formatearCantidad(item.cantidadActual) }}</div>
            <div class="crypto-price muted">{{ formatearMoneda(item.precioActual) }}/u</div>
          </div>
          <div class="crypto-value">
            <span class="value-label">Valor</span>
            <span class="value-amount mono">{{ formatearMoneda(item.valorEnDinero) }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.page {
  max-width: 700px;
  margin: 0 auto;
}

.page-header {
  margin-bottom: 1.5rem;
}

.page-header h2 {
  font-size: 1.4rem;
  font-weight: 600;
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

.total-card {
  text-align: center;
  padding: 2rem;
  background: linear-gradient(135deg, #0d1117, #161b22);
  border: 1px solid var(--primary);
  border-radius: 16px;
  margin-bottom: 1.5rem;
  position: relative;
  overflow: hidden;
}

.total-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, var(--primary), var(--primary-light));
}

.total-label {
  font-size: 0.85rem;
  color: var(--text-muted);
  margin-bottom: 0.5rem;
}

.total-amount {
  font-size: 2.5rem;
  font-weight: 700;
  color: var(--primary);
  font-family: 'SF Mono', 'Fira Code', monospace;
}

.total-count {
  font-size: 0.8rem;
  color: var(--text-muted);
  margin-top: 0.5rem;
}

.crypto-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.crypto-card {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.25rem;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 12px;
  transition: all 0.2s;
}

.crypto-card:hover {
  border-color: var(--text-muted);
  transform: translateX(2px);
}

.crypto-left {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex: 1;
}

.crypto-avatar {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 1.1rem;
  flex-shrink: 0;
}

.crypto-info {
  display: flex;
  flex-direction: column;
}

.crypto-name {
  font-weight: 600;
  font-size: 0.95rem;
}

.crypto-symbol {
  font-size: 0.8rem;
  color: var(--text-muted);
}

.crypto-right {
  text-align: right;
  min-width: 140px;
}

.crypto-amount {
  font-weight: 600;
  font-size: 0.9rem;
}

.crypto-price {
  font-size: 0.8rem;
}

.crypto-value {
  text-align: right;
  min-width: 150px;
  padding-left: 1rem;
  border-left: 1px solid var(--border);
}

.value-label {
  display: block;
  font-size: 0.7rem;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-bottom: 0.2rem;
}

.value-amount {
  font-weight: 700;
  font-size: 0.95rem;
  color: var(--primary);
}

.mono {
  font-family: 'SF Mono', 'Fira Code', monospace;
}

.muted {
  color: var(--text-muted);
}
</style>
