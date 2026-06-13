<script setup>

</script>

<template>
   <nav class="navbar">
    <span class="logo">CryptoFlux</span>
    

      <div class="links">
        <RouterLink to="/">Inicio</RouterLink>
        <RouterLink to="/compra">Compra</RouterLink>
        <RouterLink to="/Historial">Historial de Transacciones</RouterLink>
      </div>
    
  </nav>
  
  <RouterView />
</template>

<style scoped>
.navbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 2rem;
  background-color: #0a3f03;
  color: white;
  position: sticky;
  top: 0;
}

.logo {
  font-size: 1.4rem;
  font-weight: bold;
  color:#ffffff;
  text-shadow: 
  0 0 5px rgb(255, 255, 255),
  0 0 15px rgb(255, 255, 255),
  0 0 35px rgba(212, 212, 212, 0.6);
}

.links {
  display: flex;
  gap: 1.5rem;
}

.links a {
  color: white;
  text-decoration: none;
  font-size: 1rem;
}

.links a:hover {
  color:#046414;
}

</style>
