<script setup>
</script>

<template>
  <nav class="navbar">
    <RouterLink to="/" class="logo">
      <span class="logo-icon">&#8383;</span>
      <span class="logo-text">CryptoFlux</span>
    </RouterLink>

    <div class="links">
      <RouterLink to="/" class="nav-link">Inicio</RouterLink>
      <RouterLink to="/compra" class="nav-link">Compra</RouterLink>
      <RouterLink to="/venta" class="nav-link">Venta</RouterLink>
      <RouterLink to="/analisis" class="nav-link">Analisis</RouterLink>
      <RouterLink to="/historial" class="nav-link">Historial</RouterLink>
    </div>
  </nav>

  <main class="main-content">
    <RouterView />
  </main>
</template>

<style scoped>
.navbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 2rem;
  height: 64px;
  background: var(--bg-card);
  border-bottom: 1px solid var(--border);
  position: sticky;
  top: 0;
  z-index: 100;
  backdrop-filter: blur(10px);
}

.logo {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  text-decoration: none;
  color: var(--text);
}

.logo-icon {
  font-size: 1.6rem;
  color: var(--primary);
  font-weight: 700;
}

.logo-text {
  font-size: 1.3rem;
  font-weight: 700;
  letter-spacing: -0.5px;
}

.links {
  display: flex;
  gap: 0.25rem;
}

.nav-link {
  padding: 0.5rem 1rem;
  border-radius: 8px;
  color: var(--text-muted);
  font-size: 0.9rem;
  font-weight: 500;
  transition: all 0.2s ease;
  text-decoration: none;
}

.nav-link:hover {
  color: var(--text);
  background: var(--bg-card-hover);
}

.nav-link.router-link-active {
  color: var(--primary);
  background: rgba(0, 200, 83, 0.1);
}

.main-content {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}
</style>
