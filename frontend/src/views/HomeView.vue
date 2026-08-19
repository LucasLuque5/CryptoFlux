<script setup>
</script>

<template>
  <div class="home">
    <div class="hero">
      <div class="hero-badge">Sistema de gestion de criptomonedas</div>
      <h1 class="hero-title">
        <span class="hero-icon">&#8383;</span>
        CryptoFlux
      </h1>
      <p class="hero-subtitle">
        Registra tus compras y ventas de criptomonedas, consulta precios en tiempo real y analiza el estado de tu cartera.
      </p>
    </div>
  </div>
</template>

<style scoped>
.home {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 100px);
}

.hero {
  text-align: center;
  max-width: 700px;
}

.hero-badge {
  display: inline-block;
  padding: 0.4rem 1rem;
  background: rgba(0, 200, 83, 0.1);
  border: 1px solid rgba(0, 200, 83, 0.2);
  border-radius: 100px;
  font-size: 0.8rem;
  color: var(--primary);
  font-weight: 500;
  margin-bottom: 1.5rem;
  letter-spacing: 0.5px;
}

.hero-title {
  font-size: 3.5rem;
  font-weight: 700;
  margin-bottom: 1rem;
  letter-spacing: -1px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
}

.hero-icon {
  color: var(--primary);
  font-size: 3rem;
}

.hero-subtitle {
  font-size: 1.15rem;
  color: var(--text-muted);
  line-height: 1.7;
}
</style>
