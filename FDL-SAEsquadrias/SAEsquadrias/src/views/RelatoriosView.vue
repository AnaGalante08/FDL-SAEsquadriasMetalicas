<template>
  <div class="app-container">
    <AppSidebar />

    <main class="main-content">
      <header class="page-header">
        <div>
          <h1 class="page-title">Relatórios de Campo</h1>
          <p class="page-subtitle">
            Acompanhe fotos, status rápidos e ocorrências enviadas pelas equipes externas.
          </p>
        </div>

        <button type="button" class="btn-export" @click="imprimirRelatorio">
          <Printer :size="16" />
          <span>Imprimir / PDF</span>
        </button>
      </header>

      <!-- Cards de resumo clicáveis -->
      <section class="kpi-grid">
        <button
          type="button"
          class="kpi-card"
          :class="{ selected: statusFiltro === '' }"
          @click="statusFiltro = ''"
        >
          <span class="kpi-label">Total de Registros</span>
          <strong class="kpi-value">{{ relatorios.length }}</strong>
        </button>

        <button
          type="button"
          class="kpi-card kpi-ok"
          :class="{ selected: statusFiltro === 'TUDO OK' }"
          @click="alternarFiltroStatus('TUDO OK')"
        >
          <span class="kpi-label">Tudo OK</span>
          <strong class="kpi-value">{{ contagemPorStatus('TUDO OK') }}</strong>
        </button>

        <button
          type="button"
          class="kpi-card kpi-atencao"
          :class="{ selected: statusFiltro === 'ATENÇÃO' }"
          @click="alternarFiltroStatus('ATENÇÃO')"
        >
          <span class="kpi-label">Atenção</span>
          <strong class="kpi-value">{{ contagemPorStatus('ATENÇÃO') }}</strong>
        </button>

        <button
          type="button"
          class="kpi-card kpi-problema"
          :class="{ selected: statusFiltro === 'PROBLEMA' }"
          @click="alternarFiltroStatus('PROBLEMA')"
        >
          <span class="kpi-label">Com Problema</span>
          <strong class="kpi-value">{{ contagemPorStatus('PROBLEMA') }}</strong>
        </button>
      </section>

      <!-- Barra de busca e filtros -->
      <div class="toolbar">
        <div class="search-box">
          <Search :size="18" color="#9CA3AF" class="search-icon" />
          <input
            v-model="termoBusca"
            type="text"
            placeholder="Buscar por projeto, cliente, técnico ou observação..."
          />
        </div>

        <div class="filters-group">
          <select v-model="statusFiltro" class="filter-select" aria-label="Filtrar por status">
            <option value="">Todos os status</option>
            <option value="TUDO OK">Tudo OK</option>
            <option value="ATENÇÃO">Atenção</option>
            <option value="PROBLEMA">Problema</option>
          </select>

          <input
            v-model="dataFiltro"
            type="date"
            class="filter-date"
            aria-label="Filtrar por data"
          />

          <button
            v-if="termoBusca || statusFiltro || dataFiltro"
            type="button"
            class="btn-clear-filters"
            @click="limparFiltros"
          >
            Limpar
          </button>
        </div>
      </div>

      <!-- Tabela de relatórios -->
      <div class="table-scroll-area">
        <div class="table-card">
          <table class="reports-table">
            <thead>
              <tr>
                <th>Projeto / Obra</th>
                <th>Data / Hora</th>
                <th>Equipe / Técnico</th>
                <th>Fotos</th>
                <th>Status Rápido</th>
                <th>Observações</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="rel in relatoriosFiltrados" :key="rel.id">
                <td class="col-projeto">
                  <strong>{{ rel.numeroProjeto }}</strong>
                  <span class="sub-cliente">{{ rel.cliente }}</span>
                </td>
                <td>
                  <span>{{ formatarDataBr(rel.data) }}</span>
                  <span class="sub-hora">{{ rel.hora }}</span>
                </td>
                <td>{{ rel.tecnico }}</td>
                <td>
                  <span class="photo-count-pill">
                    <Camera :size="14" />
                    <span>{{ rel.fotos.length }}</span>
                  </span>
                </td>
                <td>
                  <span class="status-pill" :class="classeStatusRapido(rel.statusRapido)">
                    <CheckCircle2 v-if="rel.statusRapido === 'TUDO OK'" :size="14" />
                    <AlertCircle v-else-if="rel.statusRapido === 'ATENÇÃO'" :size="14" />
                    <XCircle v-else :size="14" />
                    <span>{{ rel.statusRapido }}</span>
                  </span>
                </td>
                <td class="col-obs" :title="rel.observacoes">
                  {{ rel.observacoes || 'Sem observações.' }}
                </td>
                <td>
                  <button
                    type="button"
                    class="btn-view-report"
                    title="Abrir detalhes do relatório"
                    @click="abrirModalRelatorio(rel)"
                  >
                    <Eye :size="18" />
                  </button>
                </td>
              </tr>

              <tr v-if="relatoriosFiltrados.length === 0">
                <td colspan="7" class="empty-row">
                  Nenhum relatório de campo encontrado para os filtros selecionados.
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </main>

    <!-- Modal de Detalhes do Relatório -->
    <div v-if="relatorioSelecionado" class="modal-overlay" @click.self="fecharModalRelatorio">
      <div class="modal-card">
        <header class="modal-header">
          <div>
            <span class="modal-tag">Registro de Campo</span>
            <h2 class="modal-title">
              {{ relatorioSelecionado.numeroProjeto }} - {{ relatorioSelecionado.cliente }}
            </h2>
          </div>

          <button type="button" class="btn-close-modal" @click="fecharModalRelatorio">
            <X :size="20" />
          </button>
        </header>

        <div class="modal-body">
          <div class="meta-strip">
            <div class="meta-item">
              <span class="meta-label">Técnico / Equipe</span>
              <strong>{{ relatorioSelecionado.tecnico }}</strong>
            </div>

            <div class="meta-item">
              <span class="meta-label">Data e Hora</span>
              <strong>
                {{ formatarDataBr(relatorioSelecionado.data) }} às {{ relatorioSelecionado.hora }}
              </strong>
            </div>

            <div class="meta-item">
              <span class="meta-label">Status Rápido</span>
              <span
                class="status-pill"
                :class="classeStatusRapido(relatorioSelecionado.statusRapido)"
              >
                <CheckCircle2 v-if="relatorioSelecionado.statusRapido === 'TUDO OK'" :size="14" />
                <AlertCircle
                  v-else-if="relatorioSelecionado.statusRapido === 'ATENÇÃO'"
                  :size="14"
                />
                <XCircle v-else :size="14" />
                <span>{{ relatorioSelecionado.statusRapido }}</span>
              </span>
            </div>
          </div>

          <div class="modal-section">
            <h3 class="modal-section-title">
              1. Fotos do serviço ({{ relatorioSelecionado.fotos.length }})
            </h3>

            <div v-if="relatorioSelecionado.fotos.length > 0" class="photos-grid">
              <button
                v-for="(foto, idx) in relatorioSelecionado.fotos"
                :key="idx"
                type="button"
                class="photo-thumb-btn"
                @click="fotoAmpliada = foto"
              >
                <img :src="foto.url" :alt="foto.legenda || `Foto ${idx + 1}`" />
                <span v-if="foto.legenda" class="photo-caption">{{ foto.legenda }}</span>
              </button>
            </div>
            <p v-else class="empty-modal-text">Nenhuma foto anexada neste envio.</p>
          </div>

          <div class="modal-section">
            <h3 class="modal-section-title">2. Observações da equipe</h3>
            <div class="obs-box">
              {{ relatorioSelecionado.observacoes || 'Nenhuma observação informada.' }}
            </div>
          </div>
        </div>

        <footer class="modal-footer">
          <button type="button" class="btn-secondary" @click="fecharModalRelatorio">Fechar</button>
          <button
            v-if="relatorioSelecionado.obraId"
            type="button"
            class="btn-primary"
            @click="irParaObra(relatorioSelecionado.obraId)"
          >
            <span>Ver Obra</span>
            <ExternalLink :size="15" />
          </button>
        </footer>
      </div>
    </div>

    <!-- Lightbox para ampliar foto -->
    <div v-if="fotoAmpliada" class="lightbox-overlay" @click="fotoAmpliada = null">
      <div class="lightbox-content" @click.stop>
        <button type="button" class="lightbox-close" @click="fotoAmpliada = null">
          <X :size="22" />
        </button>
        <img :src="fotoAmpliada.url" :alt="fotoAmpliada.legenda || 'Foto ampliada'" />
        <p v-if="fotoAmpliada.legenda" class="lightbox-caption">{{ fotoAmpliada.legenda }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import {
  Search,
  Camera,
  Eye,
  Printer,
  CheckCircle2,
  AlertCircle,
  XCircle,
  X,
  ExternalLink,
} from 'lucide-vue-next'
import AppSidebar from '@/components/AppSidebar.vue'

const API_URL = 'http://localhost:5006/api'
const STORAGE_RELATORIOS_KEY = 'sa_esquadrias_relatorios_campo'

const router = useRouter()

type StatusRapido = 'TUDO OK' | 'ATENÇÃO' | 'PROBLEMA'

interface FotoRelatorio {
  url: string
  legenda?: string
}

interface RelatorioCampo {
  id: number | string
  obraId?: number | string
  numeroProjeto: string
  cliente: string
  tecnico: string
  data: string // YYYY-MM-DD
  hora: string
  statusRapido: StatusRapido
  observacoes: string
  fotos: FotoRelatorio[]
}

const termoBusca = ref('')
const statusFiltro = ref<StatusRapido | ''>('')
const dataFiltro = ref('')
const relatorioSelecionado = ref<RelatorioCampo | null>(null)
const fotoAmpliada = ref<FotoRelatorio | null>(null)

// Exemplos baseados na tela da equipe de campo (para visualização até a API retornar dados reais)
const relatorios = ref<RelatorioCampo[]>([])

const contagemPorStatus = (status: StatusRapido) =>
  relatorios.value.filter((r) => r.statusRapido === status).length

const alternarFiltroStatus = (status: StatusRapido) => {
  statusFiltro.value = statusFiltro.value === status ? '' : status
}

const limparFiltros = () => {
  termoBusca.value = ''
  statusFiltro.value = ''
  dataFiltro.value = ''
}

const relatoriosFiltrados = computed(() => {
  const termo = termoBusca.value.trim().toLowerCase()

  return relatorios.value.filter((rel) => {
    const bateStatus = !statusFiltro.value || rel.statusRapido === statusFiltro.value
    const bateData = !dataFiltro.value || rel.data === dataFiltro.value
    const bateBusca =
      !termo ||
      rel.numeroProjeto.toLowerCase().includes(termo) ||
      rel.cliente.toLowerCase().includes(termo) ||
      rel.tecnico.toLowerCase().includes(termo) ||
      rel.observacoes.toLowerCase().includes(termo)

    return bateStatus && bateData && bateBusca
  })
})

const classeStatusRapido = (status: StatusRapido) => {
  if (status === 'TUDO OK') return 'pill-ok'
  if (status === 'ATENÇÃO') return 'pill-atencao'
  return 'pill-problema'
}

const formatarDataBr = (isoDate: string) => {
  if (!isoDate || !isoDate.includes('-')) return isoDate || '-'
  const [ano, mes, dia] = isoDate.split('-')
  return `${dia}/${mes}/${ano}`
}

const abrirModalRelatorio = (rel: RelatorioCampo) => {
  relatorioSelecionado.value = rel
}

const fecharModalRelatorio = () => {
  relatorioSelecionado.value = null
  fotoAmpliada.value = null
}

const irParaObra = (obraId: number | string) => {
  fecharModalRelatorio()
  router.push(`/obras/${obraId}`)
}

const imprimirRelatorio = () => {
  window.print()
}

const normalizarStatus = (valor: string): StatusRapido => {
  const s = (valor || '').toUpperCase()
  if (s.includes('OK')) return 'TUDO OK'
  if (s.includes('ATEN')) return 'ATENÇÃO'
  return 'PROBLEMA'
}

onMounted(async () => {
  // Carrega registros salvos localmente (caso a tela mobile da equipe externa salve no localStorage)
  try {
    const locaisRaw = localStorage.getItem(STORAGE_RELATORIOS_KEY)
    if (locaisRaw) {
      const listaLocal = JSON.parse(locaisRaw)
      if (Array.isArray(listaLocal) && listaLocal.length > 0) {
        relatorios.value = [...listaLocal, ...relatorios.value]
      }
    }
  } catch {
    // ignora falha de parse
  }

  // Tenta buscar relatórios reais da API ASP.NET Core
  try {
    const response = await fetch(`${API_URL}/Relatorios`)
    if (response.ok) {
      const dados = await response.json()
      if (Array.isArray(dados) && dados.length > 0) {
        relatorios.value = dados.map((item: any, idx: number) => ({
          id: item.id ?? idx + 1,
          obraId: item.obraId ?? item.obra?.id,
          numeroProjeto:
            item.numeroProjeto ??
            item.obra?.numeroProjeto ??
            `#${String(item.obraId ?? idx + 1).padStart(4, '0')}`,
          cliente: item.clienteNome ?? item.obra?.cliente?.nome ?? 'Cliente não informado',
          tecnico: item.tecnico ?? item.responsavel ?? 'Equipe Externa',
          data: item.data ? String(item.data).slice(0, 10) : '2026-11-27',
          hora: item.hora ?? '14:00',
          statusRapido: normalizarStatus(item.statusRapido ?? item.status),
          observacoes: item.observacoes ?? '',
          fotos: Array.isArray(item.fotos)
            ? item.fotos.map((f: any) => (typeof f === 'string' ? { url: f } : f))
            : [],
        }))
      }
    }
  } catch (error) {
    console.warn('API de relatórios indisponível no momento; exibindo registros locais.', error)
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

.main-content {
  flex: 1;
  width: 100%;
  height: 100vh;
  padding: 28px 56px 24px 56px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.page-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 20px;
  flex-shrink: 0;
}

.page-title {
  font-size: 26px;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 4px 0;
}

.page-subtitle {
  font-size: 13px;
  color: #64748b;
  margin: 0;
}

.btn-export {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 8px;
  padding: 10px 18px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  white-space: nowrap;
  transition: background-color 0.2s;
}

.btn-export:hover {
  background-color: #172554;
}

/* KPIs */
.kpi-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
  margin-bottom: 20px;
  flex-shrink: 0;
}

.kpi-card {
  background-color: #ffffff;
  border: 1px solid #d1d5db;
  border-radius: 12px;
  padding: 14px 18px;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 6px;
  cursor: pointer;
  text-align: left;
  transition: all 0.2s;
}

.kpi-card:hover {
  border-color: #94a3b8;
  transform: translateY(-1px);
}

.kpi-card.selected {
  border-width: 2px;
  border-color: #1e3a8a;
}

.kpi-ok {
  background-color: #f0fdf4;
  border-color: #bbf7d0;
}

.kpi-atencao {
  background-color: #fefce8;
  border-color: #fef08a;
}

.kpi-problema {
  background-color: #fef2f2;
  border-color: #fecaca;
}

.kpi-label {
  font-size: 12px;
  font-weight: 600;
  color: #475569;
}

.kpi-value {
  font-size: 24px;
  font-weight: 700;
  color: #0f172a;
}

.kpi-ok .kpi-value {
  color: #15803d;
}

.kpi-atencao .kpi-value {
  color: #b45309;
}

.kpi-problema .kpi-value {
  color: #dc2626;
}

/* Toolbar */
.toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 20px;
  flex-shrink: 0;
}

.search-box {
  position: relative;
  flex: 1;
  max-width: 540px;
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
  height: 42px;
  padding: 0 16px 0 42px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background-color: #ffffff;
  font-size: 13px;
  color: #1e293b;
  outline: none;
}

.search-box input:focus {
  border-color: #1e3a8a;
}

.filters-group {
  display: flex;
  align-items: center;
  gap: 10px;
}

.filter-select,
.filter-date {
  height: 42px;
  padding: 0 12px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background-color: #ffffff;
  font-size: 13px;
  color: #1e293b;
  outline: none;
}

.btn-clear-filters {
  height: 42px;
  padding: 0 14px;
  background-color: #ffffff;
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  font-size: 12px;
  font-weight: 600;
  color: #475569;
  cursor: pointer;
}

.btn-clear-filters:hover {
  background-color: #f1f5f9;
}

/* Tabela */
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

.reports-table {
  width: 100%;
  min-width: 820px;
  border-collapse: collapse;
  text-align: center;
}

.reports-table th {
  position: sticky;
  top: 0;
  z-index: 2;
  padding: 14px 12px;
  font-size: 14px;
  font-weight: 700;
  color: #0f172a;
  border-bottom: 1px solid #d1d5db;
  border-right: 1px solid #e5e7eb;
  background-color: #ffffff;
}

.reports-table th:last-child {
  border-right: none;
}

.reports-table td {
  padding: 14px 12px;
  font-size: 13px;
  color: #4b5563;
  border-bottom: 1px solid #e5e7eb;
  border-right: 1px solid #f1f5f9;
  vertical-align: middle;
}

.reports-table td:last-child {
  border-right: none;
}

.reports-table tbody tr:last-child td {
  border-bottom: none;
}

.col-projeto {
  text-align: left;
  padding-left: 18px !important;
}

.col-projeto strong {
  display: block;
  color: #0f172a;
  font-size: 13px;
}

.sub-cliente,
.sub-hora {
  display: block;
  font-size: 12px;
  color: #6b7280;
}

.photo-count-pill {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  background-color: #f1f5f9;
  color: #334155;
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
}

.status-pill {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 5px 12px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 700;
  white-space: nowrap;
}

.pill-ok {
  background-color: #9ef08a;
  color: #14532d;
}

.pill-atencao {
  background-color: #fde68a;
  color: #78350f;
}

.pill-problema {
  background-color: #ef4444;
  color: #ffffff;
}

.col-obs {
  max-width: 240px;
  text-align: left;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.btn-view-report {
  background: transparent;
  border: none;
  color: #4b5563;
  cursor: pointer;
  padding: 6px;
  border-radius: 6px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  transition:
    color 0.2s,
    background-color 0.2s;
}

.btn-view-report:hover {
  color: #1e3a8a;
  background-color: #eff6ff;
}

.empty-row {
  text-align: center;
  color: #94a3b8;
  padding: 32px !important;
}

/* Modal */
.modal-overlay {
  position: fixed;
  inset: 0;
  background-color: rgba(15, 23, 42, 0.55);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 16px;
  z-index: 50;
}

.modal-card {
  width: 100%;
  max-width: 680px;
  max-height: 90dvh;
  background-color: #ffffff;
  border-radius: 14px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  box-shadow: 0 20px 35px rgba(15, 23, 42, 0.25);
}

.modal-header {
  padding: 18px 24px;
  background-color: #071126;
  color: #ffffff;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
}

.modal-tag {
  display: block;
  font-size: 11px;
  text-transform: uppercase;
  letter-spacing: 1px;
  color: #93c5fd;
  margin-bottom: 2px;
}

.modal-title {
  font-size: 18px;
  font-weight: 700;
  margin: 0;
}

.btn-close-modal {
  background: transparent;
  border: none;
  color: #cbd5e1;
  cursor: pointer;
  padding: 4px;
  display: flex;
}

.btn-close-modal:hover {
  color: #ffffff;
}

.modal-body {
  padding: 22px 24px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.meta-strip {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 12px;
  background-color: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  padding: 12px 16px;
}

.meta-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.meta-label {
  font-size: 11px;
  font-weight: 600;
  color: #64748b;
}

.meta-item strong {
  font-size: 13px;
  color: #0f172a;
}

.modal-section-title {
  font-size: 14px;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 10px 0;
}

.photos-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 12px;
}

.photo-thumb-btn {
  background: #f8fafc;
  border: 1px solid #cbd5e1;
  border-radius: 10px;
  padding: 0;
  overflow: hidden;
  cursor: zoom-in;
  display: flex;
  flex-direction: column;
  text-align: left;
  transition: border-color 0.2s;
}

.photo-thumb-btn:hover {
  border-color: #1e3a8a;
}

.photo-thumb-btn img {
  width: 100%;
  height: 170px;
  object-fit: cover;
  display: block;
}

.photo-caption {
  padding: 8px 10px;
  font-size: 12px;
  color: #475569;
}

.obs-box {
  background-color: #f8fafc;
  border: 1px solid #cbd5e1;
  border-radius: 10px;
  padding: 14px 16px;
  font-size: 13px;
  line-height: 1.5;
  color: #1e293b;
}

.empty-modal-text {
  font-size: 13px;
  color: #64748b;
  margin: 0;
}

.modal-footer {
  padding: 14px 24px;
  border-top: 1px solid #e2e8f0;
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.btn-secondary {
  background-color: #ffffff;
  color: #1e3a8a;
  border: 1px solid #1e3a8a;
  border-radius: 6px;
  padding: 8px 18px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
}

.btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 8px 18px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
}

/* Lightbox */
.lightbox-overlay {
  position: fixed;
  inset: 0;
  background-color: rgba(0, 0, 0, 0.85);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  z-index: 60;
}

.lightbox-content {
  position: relative;
  max-width: 90vw;
  max-height: 90dvh;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.lightbox-content img {
  max-width: 100%;
  max-height: 80dvh;
  border-radius: 8px;
  object-fit: contain;
}

.lightbox-close {
  position: absolute;
  top: -36px;
  right: 0;
  background: transparent;
  border: none;
  color: #ffffff;
  cursor: pointer;
}

.lightbox-caption {
  color: #f8fafc;
  font-size: 13px;
  margin-top: 10px;
}

/* Responsividade Tablet e Mobile */
@media (max-width: 1024px) {
  .main-content {
    padding: 24px;
  }

  .kpi-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 768px) {
  .app-container {
    display: block;
    height: 100dvh;
    overflow-y: auto;
    -webkit-overflow-scrolling: touch;
  }

  .main-content {
    display: block;
    height: auto;
    min-height: 100dvh;
    padding: 20px 16px 96px 16px;
    overflow: visible;
  }

  .page-header {
    flex-direction: column;
    align-items: stretch;
  }

  .btn-export {
    justify-content: center;
  }

  .kpi-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 10px;
  }

  .toolbar {
    flex-direction: column;
    align-items: stretch;
    gap: 10px;
  }

  .search-box {
    max-width: 100%;
  }

  .filters-group {
    flex-direction: column;
    align-items: stretch;
  }

  .table-scroll-area {
    overflow: visible;
  }

  .meta-strip {
    grid-template-columns: 1fr;
  }

  .photos-grid {
    grid-template-columns: 1fr;
  }

  .modal-footer {
    flex-direction: column-reverse;
  }

  .btn-secondary,
  .btn-primary {
    width: 100%;
    justify-content: center;
  }
}

@media print {
  .btn-export,
  .toolbar,
  .kpi-grid {
    display: none;
  }
}
</style>
