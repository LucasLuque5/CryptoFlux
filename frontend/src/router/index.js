import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import CompraView from '../views/CompraView.vue'
import HistorialView from '../views/HistorialView.vue'
import TransaccionDetalleView from'../views/TransaccionDetalleView.vue'
import TransaccionEditarView from '../views/TransaccionEditarView.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
  { 
    path: '/',
    name: 'Home',
    component: HomeView
  },
  {
    path: '/Compra',
    name: 'Compra',
    component: CompraView
  },
  {
    path: '/Historial',
    name: 'historial',
    component: HistorialView
  },
  {
    path: '/transaccion/:id',
    name: 'transaccionDetalle',
    component: TransaccionDetalleView
  },
  {
    path: '/transaccion/:id/editar',
    name: 'transaccionEditar',
    component: TransaccionEditarView
  },
  ],
})

export default router
