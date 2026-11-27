import { createRouter, createWebHistory } from 'vue-router'
import ListaObrasView from '@/views/ListaObrasView.vue'
import DetalheObraView from '@/views/DetalheObraView.vue'
import CadastroObrasView from '@/views/CadastroObrasView.vue'
import CadastroProdutosView from '@/views/CadastroProdutosView.vue'
import RelatoriosView from '@/views/RelatoriosView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', redirect: '/obras' },
    { path: '/obras', name: 'lista-obras', component: ListaObrasView },
    { path: '/obras/:id', name: 'detalhe-obra', component: DetalheObraView },
    { path: '/cadastro-obras', name: 'cadastro-obras', component: CadastroObrasView },
    { path: '/editar-obra/:id', name: 'editar-obra', component: CadastroObrasView },
    { path: '/cadastro-produtos', name: 'cadastro-produtos', component: CadastroProdutosView },
    { path: '/relatorios', name: 'relatorios', component: RelatoriosView },
  ],
})

export default router
