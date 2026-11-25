<script setup>
import { ref, onMounted } from 'vue'
import { carteraApi } from '../Services/api.js'
import { formatearMoneda } from '../utils/formato.js'

const cartera = ref(null)

onMounted(async () => {
  try {
    const response = await carteraApi.obtener()
    cartera.value = response.data
  } catch (e) {
    console.error(e)
  }
})
</script>

<template>
  <div class="home">
    <section class="hero">
      <div class="hero-texto">
        <p class="hero-etiqueta">Sistema de Gestion</p>
        <h1 class="hero-titulo">Crypto<span>Flux</span></h1>
        <p class="hero-descripcion">
          Registra tus compras y ventas.
        </p>
      </div>

      <RouterLink to="/analisis" class="saldo">
        <span class="saldo-label">Billetera Virtual</span>
        <span class="saldo-total">{{ cartera ? formatearMoneda(cartera.totalCartera) : '...' }}</span>
        <ul v-if="cartera" class="saldo-lista">
          <li v-for="item in cartera.criptomonedas" :key="item.cryptoCode">
            <span>{{ item.cryptoCode }}</span>
            <span class="mono">{{ item.cantidadActual }}</span>
          </li>
        </ul>
        <span class="saldo-link">Ver analisis &rarr;</span>
      </RouterLink>
    </section>
  </div>
</template>

<style scoped>
.home {
  display: flex;
  align-items: center;
  min-height: calc(100vh - 130px);
}

.hero {
  width: 100%;
  display: grid;
  grid-template-columns: 1.4fr 1fr;
  gap: 2rem;
  align-items: center;
}

.hero-etiqueta {
  color: var(--primary);
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 3px;
  font-size: 0.85rem;
}

.hero-titulo {
  font-size: 4.5rem;
  line-height: 1.1;
  font-weight: 700;
  letter-spacing: -2px;
  margin: 0.5rem 0 1rem;
}

.hero-titulo span {
  color: var(--primary);
}

.hero-descripcion {
  color: var(--text-muted);
  font-size: 1.1rem;
  max-width: 480px;
}

.saldo {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  padding: 2rem;
  border-radius: 20px;
  background: linear-gradient(135deg, var(--primary), var(--primary-dark));
  color: var(--bg-dark);
  transform: rotate(2deg);
  transition: transform 0.3s;
}

.saldo:hover {
  color: var(--bg-dark);
  transform: rotate(0deg);
}

.saldo-label {
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 2px;
  font-size: 0.8rem;
}

.saldo-total {
  font-family: var(--font-mono);
  font-size: 2rem;
  font-weight: 600;
}

.saldo-lista {
  list-style: none;
  border-top: 1px solid rgba(18, 15, 28, 0.25);
  padding-top: 0.75rem;
}

.saldo-lista li {
  display: flex;
  justify-content: space-between;
  font-weight: 500;
}

.saldo-link {
  margin-top: 0.5rem;
  font-weight: 600;
}

@media (max-width: 800px) {
  .hero {
    grid-template-columns: 1fr;
  }

  .hero-titulo {
    font-size: 3rem;
  }

  .saldo {
    transform: none;
  }
}
</style>
