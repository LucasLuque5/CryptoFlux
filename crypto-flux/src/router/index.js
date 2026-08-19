import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import CompraView from '../views/CompraView.vue'
import VentaView from '../views/VentaView.vue'
import HistorialView from '../views/HistorialView.vue'
import TransaccionDetalleView from'../views/TransaccionDetalleView.vue'
import TransaccionEditarView from '../views/TransaccionEditarView.vue'
import AnalisisView from '../views/AnalisisView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
  { 
    path: '/',
    name: 'Home',
    component: HomeView
  },
  {
    path: '/compra',
    name: 'Compra',
    component: CompraView
  },
  {
    path:'/venta',
    name:'Venta',
    component: VentaView
  },
  {
    path: '/historial',
    name: 'Historial',
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
  {
    path: '/analisis',
    name: 'Analisis',
    component: AnalisisView
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    redirect: '/'
  },
  ],
})

export default router
