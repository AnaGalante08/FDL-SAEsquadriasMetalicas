<template>
  <aside class="sidebar">
    <div class="logo-container">
      <img src="@/assets/logoSA.png" alt="SA Esquadrias" class="logo-img" />
      <span class="logo-text">S.A</span>
    </div>

    <nav class="sidebar-nav">
      <button
        type="button"
        class="nav-item"
        :class="{ active: isRotaAtiva('/inicio') }"
        title="Inicio"
        @click="irPara('/')"
      >
        <Home :size="20" class="nav-icon" />
        <span class="nav-label">Inicio</span>
      </button>

      <button
        type="button"
        class="nav-item"
        :class="{ active: isRotaAtiva('/projetos') }"
        title="Projetos"
        @click="irPara('/projetos')"
      >
        <Calendar :size="20" class="nav-icon" />
        <span class="nav-label">Projetos</span>
      </button>

      <button
        type="button"
        class="nav-item"
        :class="{ active: isRotaObras }"
        title="Obras"
        @click="irPara('/obras')"
      >
        <Wrench :size="20" class="nav-icon" />
        <span class="nav-label">Obras</span>
      </button>

      <button
        type="button"
        class="nav-item"
        :class="{ active: isRotaAtiva('/pendencias') }"
        title="Pendências"
        @click="irPara('/pendencias')"
      >
        <ClipboardList :size="20" class="nav-icon" />
        <span class="nav-label">Pendências</span>
      </button>

      <button
        type="button"
        class="nav-item"
        :class="{ active: isRotaAtiva('/equipe-externa') }"
        title="Equipe externa"
        @click="irPara('/equipe-externa')"
      >
        <Users :size="20" class="nav-icon" />
        <span class="nav-label">Equipe externa</span>
      </button>

      <button
        type="button"
        class="nav-item"
        :class="{ active: isRotaAtiva('/relatorios') }"
        title="Relatórios"
        @click="irPara('/relatorios')"
      >
        <FileText :size="20" class="nav-icon" />
        <span class="nav-label">Relatórios</span>
      </button>
    </nav>
  </aside>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Home, Calendar, Wrench, ClipboardList, Users, FileText } from 'lucide-vue-next'

const route = useRoute()
const router = useRouter()

const isRotaAtiva = (prefixo: string) => route.path.startsWith(prefixo)

const isRotaObras = computed(() => {
  return (
    route.path.startsWith('/obras') ||
    route.path.startsWith('/cadastro-obras') ||
    route.path.startsWith('/editar-obra') ||
    route.path.startsWith('/cadastro-produtos')
  )
})

const irPara = (rota: string) => {
  if (rota === '/' || isRotaObrasRoute(rota)) {
    router.push(rota)
  } else {
    // Caso a rota futura ainda não tenha sido criada no router, evita quebrar a navegação
    const existe = router.getRoutes().some((r) => r.path === rota)
    if (existe) {
      router.push(rota)
    } else {
      router.push('/obras')
    }
  }
}

const isRotaObrasRoute = (rota: string) =>
  rota === '/obras' || rota === '/cadastro-obras' || rota === '/cadastro-produtos'
</script>

<style scoped>
.sidebar {
  width: 84px;
  background-color: #071126;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  padding: 24px 16px;
  flex-shrink: 0;
  z-index: 30;
  overflow: hidden;
  transition: width 0.25s ease;
  box-shadow: 2px 0 12px rgba(7, 17, 38, 0.14);
}

.sidebar:hover {
  width: 235px;
}

.logo-container {
  margin-bottom: 36px;
  display: flex;
  align-items: center;
  gap: 14px;
  padding-left: 3px;
  width: 100%;
  white-space: nowrap;
}

.logo-img {
  width: 46px;
  height: auto;
  object-fit: contain;
  flex-shrink: 0;
}

.logo-text {
  font-size: 22px;
  font-weight: 400;
  letter-spacing: 3px;
  color: #ffffff;
  opacity: 0;
  transform: translateX(-8px);
  transition:
    opacity 0.2s ease,
    transform 0.2s ease;
  pointer-events: none;
}

.sidebar:hover .logo-text {
  opacity: 1;
  transform: translateX(0);
}

.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 18px;
  width: 100%;
}

.nav-item {
  width: 100%;
  background: transparent;
  border: none;
  color: #cbd5e1;
  cursor: pointer;
  padding: 11px 16px;
  border-radius: 8px;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: flex-start;
  gap: 16px;
  white-space: nowrap;
}

.nav-icon {
  flex-shrink: 0;
}

.nav-label {
  font-size: 14px;
  font-weight: 500;
  color: #f8fafc;
  opacity: 0;
  transform: translateX(-8px);
  transition:
    opacity 0.2s ease,
    transform 0.2s ease;
  pointer-events: none;
}

.sidebar:hover .nav-label {
  opacity: 1;
  transform: translateX(0);
}

.nav-item:hover,
.nav-item.active {
  color: #ffffff;
  background-color: rgba(255, 255, 255, 0.08);
}

/* Responsividade Mobile: barra inferior fixa sem expansão lateral */
@media (max-width: 768px) {
  .sidebar,
  .sidebar:hover {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    width: 100%;
    height: 60px;
    flex-direction: row;
    align-items: center;
    justify-content: space-around;
    padding: 0 12px;
    box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.15);
    z-index: 40;
  }

  .logo-container,
  .nav-label {
    display: none;
  }

  .sidebar-nav {
    flex-direction: row;
    justify-content: space-around;
    gap: 4px;
  }

  .nav-item {
    width: auto;
    justify-content: center;
    padding: 10px;
  }
}
</style>
