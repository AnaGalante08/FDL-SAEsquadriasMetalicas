import { createRouter, createWebHistory } from 'vue-router'
import CadastroObrasView from '@/views/CadastroObrasView.vue'
import CadastroProdutosView from '@/views/CadastroProdutosView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'cadastro-obras',
      component: CadastroObrasView,
    },
    {
      path: '/cadastro-produtos',
      name: 'cadastro-produtos',
      component: CadastroProdutosView,
    },
  ],
})

export default router
