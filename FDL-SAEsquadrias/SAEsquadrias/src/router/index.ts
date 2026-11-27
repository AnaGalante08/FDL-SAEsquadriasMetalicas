import { createRouter, createWebHistory } from 'vue-router'
import CadastroObrasView from '../views/CadastroObrasView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'cadastro-obras',
      component: CadastroObrasView,
    },
  ],
})

export default router
