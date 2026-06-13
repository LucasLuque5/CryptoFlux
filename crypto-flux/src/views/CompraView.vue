<script setup>
import { transaccionesApi } from '../Services/api.js'
import { reactive } from 'vue';

const form = reactive({
    cripto: '',
    cantidad: null,
    money: null,
    fecha: '',
    
})

async function enviarFormulario() {

  if(!form.cantidad || form.cantidad < 0 || !form.cripto || !form.fecha || form.money < 0) 
  {
   
    alert(`Complete todos los campos por favor e
    Ingrese un monto mayor a 0`)
    return;

  }
  try
   {
    await transaccionesApi.create({
      cryptoCode: form.cripto,
      action: 'compra',
      cryptoAmount: form.cantidad,
      money: form.money,
      dateTime: new Date(form.fecha).toISOString()
    })

    alert('¡Compra registrada correctamente!')
    
    form.cripto = ''
    form.cantidad = null
    form.fecha = ''

  } catch (error) {
    alert('Error al registrar la compra.')
    console.error(error)
  }

}
</script>

<template>
  <div class="layout-principal">
    
    <div class="contenedor">
      <h2>Compra</h2>
      <form @submit.prevent="enviarFormulario">

        <div class="campo">
          <label>Criptomoneda</label>
          <select v-model="form.cripto">
            <option value="">-- Selecciona --</option>
            <option value="btc">Bitcoin (BTC)</option>
            <option value="eth">Ethereum (ETH)</option>
            <option value="usdc">USD Coin (USDC)</option>
          </select>
        </div> 
        
        <div class="campo">
          <label>Cantidad</label>
          <input
            v-model="form.cantidad"
            type="number"
            step="0.00000001"
            placeholder="ej: 0.00070"
          />
        </div>
        <div class="campo">
          <label>Monto (ARS)</label>
          <input 
          v-model="form.money"
          type="number"
          step="0.01"
          placeholder="ej: 150000"/>
        </div>
        <div class="campo">
          <label>Fecha y hora</label>
          <input v-model="form.fecha" type="datetime-local" />
        </div>
        
        <button type="submit">Registrar</button>
      </form>
    </div>

    <div class="bienvenida">
      <h1>₿ CryptoFlux</h1>
      <p>Tu billetera de criptomonedas personal</p>
    </div>

  </div>
</template>


<style scoped>

.layout-principal {
  display: flex;
  justify-content: center; 
  align-items: center;     
  gap: 80px;               
  max-width: 1200px;
  margin: 80px auto;       
  padding: 0 20px;
}


.bienvenida {
  text-align: center;      
  max-width: 480px;
  margin: 0;               
}

.bienvenida h1 {
  font-size: 3rem;
  color: #046414;
  margin-bottom: 1rem;
}

.bienvenida p {
  font-size: 1.2rem;
  color: #666;
}

.contenedor {
  width: 100%;
  max-width: 450px;
  margin: 0;               
  padding: 2rem;
  border: 2px solid #046e24;
  border-radius: 12px;
}

.campo {
  display: flex;
  flex-direction: column;
  margin-bottom: 1rem;
}

label {
  margin-bottom: 4px;
  font-weight: bold;
  font-size: 0.9rem;
}

select, input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 1rem;
}

button {
  width: 100%;
  padding: 10px;
  background: #046414;
  color: rgb(255, 255, 255);
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
}
</style>