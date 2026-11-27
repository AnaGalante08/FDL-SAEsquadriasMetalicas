<template>
  <div class="app-container">
    <AppSidebar />

    <main class="main-content">
      <h1 class="page-title">Obras</h1>

      <div class="toolbar">
        <div class="search-box">
          <Search :size="18" color="#9CA3AF" class="search-icon" />
          <input
            v-model="termoBusca"
            type="text"
            placeholder="Buscar por cliente, endereço, projeto"
          />
        </div>

        <div class="filter-wrapper">
          <button
            type="button"
            class="btn-filter"
            @click="mostrarFiltroStatus = !mostrarFiltroStatus"
          >
            <Filter :size="18" />
            <span>Filtros</span>
          </button>

          <div v-if="mostrarFiltroStatus" class="filter-dropdown">
            <label>Filtrar por Status:</label>
            <select v-model="statusFiltro" @change="mostrarFiltroStatus = false">
              <option value="">Todos</option>
              <option value="Aguardando liberação">Aguardando liberação</option>
              <option value="Com pendência">Com pendência</option>
              <option value="Em produção">Em produção</option>
              <option value="Em instalação">Em instalação</option>
              <option value="Finalizado">Finalizado</option>
              <option value="Andamento">Andamento</option>
            </select>
          </div>
        </div>
      </div>

      <div class="table-scroll-area">
        <div class="table-card">
          <table class="obras-table">
            <thead>
              <tr>
                <th>Nº Projeto</th>
                <th>Cliente</th>
                <th>Endereço</th>
                <th>Vendedor</th>
                <th>Status</th>
                <th>Prazo</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="obra in obrasFiltradas" :key="obra.id">
                <td class="col-projeto">{{ obra.numeroProjeto }}</td>
                <td>{{ obra.cliente }}</td>
                <td>{{ obra.endereco }}</td>
                <td>{{ obra.vendedor }}</td>
                <td>
                  <span class="status-badge" :class="classeStatus(obra.status)">
                    {{ obra.status }}
                  </span>
                </td>
                <td>{{ obra.status === 'Finalizado' ? '-' : obra.prazo }}</td>
                <td>
                  <button
                    v-if="obra.status !== 'Finalizado'"
                    type="button"
                    class="btn-eye"
                    title="Visualizar detalhes da obra"
                    @click="visualizarObra(obra)"
                  >
                    <Eye :size="18" />
                  </button>
                </td>
              </tr>

              <tr v-if="obrasFiltradas.length === 0">
                <td colspan="7" class="empty-row">Nenhuma obra encontrada.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div class="page-footer">
        <button type="button" class="btn-cadastro-obras" @click="irPara('/cadastro-obras')">
          Cadastro de Obras
        </button>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import AppSidebar from '@/components/AppSidebar.vue'
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import {
  Home,
  Calendar,
  Wrench,
  ClipboardList,
  Users,
  FileText,
  Search,
  Filter,
  Eye,
} from 'lucide-vue-next'

const API_URL = 'http://localhost:5006/api'
const STORAGE_INATIVAS_KEY = 'sa_esquadrias_obras_inativas'
const STORAGE_OBRAS_EDITADAS_KEY = 'sa_esquadrias_obras_editadas'

const router = useRouter()

interface ObraListagem {
  id: number | string
  numeroProjeto: string
  cliente: string
  endereco: string
  vendedor: string
  status: string
  prazo: string
}

const termoBusca = ref('')
const statusFiltro = ref('')
const mostrarFiltroStatus = ref(false)

const obras = ref<ObraListagem[]>([
  {
    id: 15,
    numeroProjeto: '#0015',
    cliente: 'Residencial Aurora',
    endereco: 'Rua das Flores, 365',
    vendedor: 'Geovanna',
    status: 'Aguardando liberação',
    prazo: '15 dias',
  },
  {
    id: 18,
    numeroProjeto: '#0018',
    cliente: 'Edificio Central',
    endereco: 'Av.Principal, 456',
    vendedor: 'Ana Carolina',
    status: 'Com pendência',
    prazo: '20 dias',
  },
  {
    id: 20,
    numeroProjeto: '#0020',
    cliente: 'Cond. Por do sol',
    endereco: 'Rua do Sol, 120',
    vendedor: 'Ana Carolina',
    status: 'Em produção',
    prazo: '25 dias',
  },
  {
    id: 30,
    numeroProjeto: '#0030',
    cliente: 'Residencial Jardim',
    endereco: 'Rua Babuino, 222',
    vendedor: 'Geovanna',
    status: 'Em instalação',
    prazo: '30 dias',
  },
  {
    id: 33,
    numeroProjeto: '#0033',
    cliente: 'Torre de Bela Vista',
    endereco: 'Av. das Palmeiras, 13',
    vendedor: 'Yasmin',
    status: 'Finalizado',
    prazo: '-',
  },
  {
    id: 22,
    numeroProjeto: '#0022',
    cliente: 'Residencial Aurora',
    endereco: 'Rua Antônio, 2213',
    vendedor: 'Yasmin',
    status: 'Finalizado',
    prazo: '-',
  },
])

const irPara = (rota: string) => {
  router.push(rota)
}

const obterIdsInativos = (): string[] => {
  try {
    const raw = localStorage.getItem(STORAGE_INATIVAS_KEY)
    return raw ? JSON.parse(raw) : []
  } catch {
    return []
  }
}

const obrasFiltradas = computed(() => {
  const termo = termoBusca.value.trim().toLowerCase()
  const inativos = obterIdsInativos()

  return obras.value.filter((obra) => {
    const estaAtiva = !inativos.includes(String(obra.id))
    const bateStatus = !statusFiltro.value || obra.status === statusFiltro.value
    const bateBusca =
      !termo ||
      obra.numeroProjeto.toLowerCase().includes(termo) ||
      obra.cliente.toLowerCase().includes(termo) ||
      obra.endereco.toLowerCase().includes(termo) ||
      obra.vendedor.toLowerCase().includes(termo)

    return estaAtiva && bateStatus && bateBusca
  })
})

const classeStatus = (status: string) => {
  const normalizado = status.toLowerCase()
  if (normalizado.includes('pendência') || normalizado.includes('pendencia')) {
    return 'badge-red'
  }
  if (
    normalizado.includes('instalação') ||
    normalizado.includes('instalacao') ||
    normalizado.includes('finalizado') ||
    normalizado.includes('concluído')
  ) {
    return 'badge-green'
  }
  return 'badge-yellow'
}

const visualizarObra = (obra: ObraListagem) => {
  router.push(`/obras/${obra.id}`)
}

onMounted(async () => {
  try {
    const response = await fetch(`${API_URL}/Obras`)
    if (response.ok) {
      const dados = await response.json()
      if (Array.isArray(dados) && dados.length > 0) {
        obras.value = dados
          .filter((item: any) => item.ativo !== false)
          .map((item: any, idx: number) => ({
            id: item.id ?? idx + 1,
            numeroProjeto: item.numeroProjeto ?? `#${String(item.id ?? idx + 1).padStart(4, '0')}`,
            cliente: item.cliente?.nome ?? item.clienteNome ?? 'Cliente não informado',
            endereco: item.rua ? `${item.rua}, ${item.numero ?? ''}` : (item.endereco ?? '-'),
            vendedor: item.vendedor?.nome ?? item.vendedorNome ?? '-',
            status: item.status ?? 'Andamento',
            prazo: item.prazo ?? '-',
          }))
      }
    }
  } catch (error) {
    console.warn('API de obras indisponível no momento; exibindo dados locais.', error)
  }

  // Sincroniza edições feitas na tela de edição
  try {
    const editadasRaw = localStorage.getItem(STORAGE_OBRAS_EDITADAS_KEY)
    if (editadasRaw) {
      const mapa = JSON.parse(editadasRaw)
      obras.value = obras.value.map((item) => {
        const ed = mapa[String(item.id)]
        if (!ed) return item
        return {
          ...item,
          cliente: ed.clienteNome ?? item.cliente,
          endereco: ed.rua ? `${ed.rua}, ${ed.numero ?? ''}` : item.endereco,
          vendedor: ed.vendedor ?? item.vendedor,
          status: ed.status ?? item.status,
          prazo: ed.prazo ?? item.prazo,
        }
      })
    }
  } catch {
    // ignora
  }
})
</script>

<style scoped>
* {
  box-sizing: border-box;
  font-family:
    'Inter',
    -apple-system,
    BlinkMacSystemFont,
    'Segoe UI',
    Roboto,
    sans-serif;
}

.app-container {
  display: flex;
  height: 100vh;
  overflow: hidden;
  background-color: #f8f9fa;
}

.sidebar {
  width: 84px;
  background-color: #071126;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 24px;
  flex-shrink: 0;
  z-index: 30;
}

.logo-container {
  margin-bottom: 36px;
  display: flex;
  justify-content: center;
}

.logo-img {
  width: 46px;
  height: auto;
  object-fit: contain;
}

.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 24px;
  width: 100%;
  align-items: center;
}

.nav-item {
  background: transparent;
  border: none;
  color: #cbd5e1;
  cursor: pointer;
  padding: 10px;
  border-radius: 8px;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.nav-item:hover,
.nav-item.active {
  color: #ffffff;
  background-color: rgba(255, 255, 255, 0.08);
}

.main-content {
  flex: 1;
  width: 100%;
  height: 100vh;
  padding: 32px 64px 24px 64px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.page-title {
  font-size: 26px;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 20px 0;
  flex-shrink: 0;
}

.toolbar {
  display: flex;
  align-items: center;
  gap: 24px;
  margin-bottom: 28px;
  flex-shrink: 0;
}

.search-box {
  position: relative;
  flex: 1;
  max-width: 620px;
  display: flex;
  align-items: center;
}

.search-icon {
  position: absolute;
  left: 14px;
  pointer-events: none;
}

.search-box input {
  width: 100%;
  height: 44px;
  padding: 0 16px 0 42px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background-color: #ffffff;
  font-size: 13px;
  color: #1e293b;
  outline: none;
  transition: border-color 0.2s;
}

.search-box input::placeholder {
  color: #9ca3af;
}

.search-box input:focus {
  border-color: #1e3a8a;
}

.filter-wrapper {
  position: relative;
}

.btn-filter {
  height: 44px;
  padding: 0 20px;
  background-color: #ffffff;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  font-weight: 600;
  color: #0f172a;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-filter:hover {
  background-color: #f1f5f9;
}

.filter-dropdown {
  position: absolute;
  top: 50px;
  right: 0;
  background-color: #ffffff;
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  padding: 12px;
  box-shadow: 0 10px 20px rgba(15, 23, 42, 0.12);
  z-index: 20;
  min-width: 210px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.filter-dropdown label {
  font-size: 12px;
  font-weight: 600;
  color: #475569;
}

.filter-dropdown select {
  padding: 6px 10px;
  border: 1px solid #cbd5e1;
  border-radius: 6px;
  font-size: 13px;
  color: #1e293b;
}

.table-scroll-area {
  flex: 1;
  overflow-y: auto;
  min-height: 0;
  padding-bottom: 12px;
}

.table-card {
  background-color: #ffffff;
  border: 1px solid #d1d5db;
  border-radius: 10px;
  overflow-x: auto;
}

.obras-table {
  width: 100%;
  min-width: 720px;
  border-collapse: collapse;
  text-align: center;
}

.obras-table th {
  position: sticky;
  top: 0;
  z-index: 2;
  padding: 16px 14px;
  font-size: 15px;
  font-weight: 700;
  color: #0f172a;
  border-bottom: 1px solid #d1d5db;
  border-right: 1px solid #d1d5db;
  background-color: #ffffff;
}

.obras-table th:last-child {
  border-right: none;
}

.obras-table td {
  padding: 16px 14px;
  font-size: 13px;
  color: #4b5563;
  border-bottom: 1px solid #e5e7eb;
  border-right: 1px solid #e5e7eb;
  vertical-align: middle;
}

.obras-table td:last-child {
  border-right: none;
}

.obras-table tbody tr:last-child td {
  border-bottom: none;
}

.col-projeto {
  font-weight: 500;
  color: #374151;
}

.status-badge {
  display: inline-block;
  padding: 5px 14px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 700;
  white-space: nowrap;
}

.badge-yellow {
  background-color: #fef08a;
  color: #1e293b;
}

.badge-red {
  background-color: #fecaca;
  color: #1e293b;
}

.badge-green {
  background-color: #bbf7d0;
  color: #1e293b;
}

.btn-eye {
  background: transparent;
  border: none;
  color: #4b5563;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 4px;
  border-radius: 6px;
  transition: color 0.2s;
}

.btn-eye:hover {
  color: #0f172a;
}

.empty-row {
  text-align: center;
  color: #94a3b8;
  padding: 28px !important;
}

.page-footer {
  display: flex;
  justify-content: flex-end;
  padding-top: 14px;
  flex-shrink: 0;
}

.btn-cadastro-obras {
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 10px 20px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-cadastro-obras:hover {
  background-color: #172554;
}

@media (max-width: 960px) {
  .main-content {
    padding: 24px;
  }
}

@media (max-width: 768px) {
  .app-container {
    display: block;
    height: 100dvh;
    overflow-y: auto;
    -webkit-overflow-scrolling: touch;
  }

  .sidebar {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    width: 100%;
    height: 60px;
    flex-direction: row;
    justify-content: space-around;
    padding: 0 12px;
    box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.15);
    z-index: 40;
  }

  .logo-container {
    display: none;
  }

  .sidebar-nav {
    flex-direction: row;
    justify-content: space-around;
    gap: 4px;
  }

  .main-content {
    display: block;
    height: auto;
    min-height: 100dvh;
    padding: 20px 16px 96px 16px;
    overflow: visible;
  }

  .page-title {
    font-size: 22px;
    margin-bottom: 16px;
  }

  .toolbar {
    flex-direction: column;
    align-items: stretch;
    gap: 12px;
    margin-bottom: 20px;
  }

  .search-box {
    max-width: 100%;
  }

  .btn-filter {
    width: 100%;
    justify-content: center;
  }

  .filter-dropdown {
    left: 0;
    right: 0;
    width: 100%;
  }

  .table-scroll-area {
    overflow: visible;
  }

  .page-footer {
    padding-top: 12px;
  }

  .btn-cadastro-obras {
    width: 100%;
    padding: 12px;
    font-size: 14px;
  }
}
</style>
