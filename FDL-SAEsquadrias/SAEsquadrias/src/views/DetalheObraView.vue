<template>
  <div class="app-container">
    <AppSidebar />

    <main class="main-content">
      <header class="top-bar">
        <div class="top-bar-left">
          <button type="button" class="btn-voltar" @click="irPara('/obras')">
            <ArrowLeft :size="18" />
            <span>Voltar</span>
          </button>

          <div class="obra-header-info">
            <h1 class="obra-titulo">
              <template v-if="abaAtiva === 'historico'">
                Projeto{{ obra.numeroProjeto }}
                <span class="obra-subtitulo-cliente">- {{ obra.clienteNome }}</span>
              </template>
              <template v-else> Obra &nbsp;{{ obra.numeroProjeto }} </template>
            </h1>
            <span
              v-if="abaAtiva === 'gerais'"
              class="obra-status"
              :class="classeCorStatus(obra.status)"
            >
              {{ obra.status }}
            </span>
          </div>
        </div>

        <div class="top-bar-actions">
          <button
            type="button"
            class="btn-excluir-obra"
            :disabled="excluindo"
            @click="inativarObra"
          >
            <Trash2 :size="16" />
            <span>{{ excluindo ? 'Excluindo...' : 'Excluir' }}</span>
          </button>

          <button type="button" class="btn-editar-obra" @click="irParaEditar">
            <Pencil :size="16" />
            <span>Editar</span>
          </button>
        </div>
      </header>

      <div class="content-scroll-area">
        <section class="details-card" :class="{ 'history-mode': abaAtiva === 'historico' }">
          <div class="tabs-bar">
            <button
              type="button"
              class="tab-btn"
              :class="{ active: abaAtiva === 'gerais' }"
              @click="abaAtiva = 'gerais'"
            >
              Dados Gerais
            </button>
            <button
              type="button"
              class="tab-btn"
              :class="{ active: abaAtiva === 'historico' }"
              @click="abaAtiva = 'historico'"
            >
              Histórico
            </button>
          </div>

          <!-- ABA DADOS GERAIS -->
          <div v-if="abaAtiva === 'gerais'" class="tab-content">
            <div class="info-columns">
              <div class="info-col">
                <h2 class="col-heading">Dados da obra</h2>

                <div class="info-item">
                  <span class="info-label">Endereço</span>
                  <span class="info-value">{{ obra.endereco }}</span>
                </div>

                <div class="info-item">
                  <span class="info-label">Vendedor</span>
                  <span class="info-value">{{ obra.vendedor }}</span>
                </div>

                <div class="info-item">
                  <span class="info-label">Prazo de entrega</span>
                  <span class="info-value">{{ obra.prazoEntrega }}</span>
                </div>
              </div>

              <div class="info-col">
                <h2 class="col-heading">Dados do cliente</h2>

                <div class="info-item">
                  <span class="info-label">Nome</span>
                  <span class="info-value">{{ obra.clienteNome }}</span>
                </div>

                <div class="info-item">
                  <span class="info-label">Telefone</span>
                  <span class="info-value">{{ obra.clienteTelefone }}</span>
                </div>

                <div class="info-item">
                  <span class="info-label">Email</span>
                  <span class="info-value">{{ obra.clienteEmail }}</span>
                </div>
              </div>
            </div>

            <div class="obs-block">
              <h2 class="col-heading">Observações gerais</h2>
              <p class="info-value">{{ obra.observacoes || 'Nenhuma observação registrada.' }}</p>
            </div>
          </div>

          <!-- ABA HISTÓRICO -->
          <div v-else class="tab-content history-tab-content">
            <!-- Stepper Horizontal -->
            <div class="stepper-scroll-wrapper">
              <div class="horizontal-stepper">
                <template v-for="(etapa, idx) in obra.etapas" :key="etapa.chave">
                  <div class="step-node">
                    <div class="step-circle" :class="`state-${etapa.estado}`">
                      <Check v-if="etapa.icone === 'check'" :size="24" :stroke-width="2.4" />
                      <Factory
                        v-else-if="etapa.icone === 'factory'"
                        :size="24"
                        :stroke-width="1.8"
                      />
                      <Wrench v-else-if="etapa.icone === 'wrench'" :size="22" :stroke-width="2" />
                      <Flag v-else :size="22" :stroke-width="1.8" />
                    </div>

                    <span class="step-title">{{ etapa.titulo }}</span>

                    <span v-if="etapa.estado === 'andamento'" class="step-pill-andamento">
                      em andamento
                    </span>
                    <span v-else-if="etapa.estado === 'pendente'" class="step-sub-pendente">
                      pendente
                    </span>
                    <span v-else class="step-sub-data">
                      {{ etapa.dataCurta }}
                    </span>
                  </div>

                  <div
                    v-if="idx < obra.etapas.length - 1"
                    class="step-connector"
                    :class="`conn-${etapa.estado}`"
                  >
                    <span class="connector-line"></span>
                    <span class="connector-dot"></span>
                    <span class="connector-line"></span>
                  </div>
                </template>
              </div>
            </div>

            <!-- Lista Vertical: Histórico do andamento -->
            <div class="andamento-section">
              <h2 class="andamento-heading">Histórico do andamento</h2>

              <div class="vertical-timeline">
                <div
                  v-for="(item, index) in obra.historicoDetalhado"
                  :key="index"
                  class="v-timeline-row"
                >
                  <div class="v-left-col">
                    <div class="v-circle" :class="`state-${item.estado}`">
                      <Check v-if="item.icone === 'check'" :size="18" :stroke-width="2.4" />
                      <Calculator
                        v-else-if="item.icone === 'medicao'"
                        :size="18"
                        :stroke-width="1.8"
                      />
                      <Factory
                        v-else-if="item.icone === 'factory'"
                        :size="18"
                        :stroke-width="1.8"
                      />
                      <Wrench v-else-if="item.icone === 'wrench'" :size="17" :stroke-width="2" />
                      <Flag v-else :size="17" :stroke-width="1.8" />
                    </div>
                    <div
                      v-if="index < obra.historicoDetalhado.length - 1"
                      class="v-line"
                      :class="`vline-${item.estado}`"
                    ></div>
                  </div>

                  <div class="v-info-col">
                    <span class="v-title">{{ item.titulo }}</span>
                    <span class="v-meta">
                      <Calendar :size="13" class="v-meta-icon" />
                      <span>
                        {{ item.dataCompleta ? `${item.dataCompleta} - ` : '- '
                        }}{{ item.responsavelLabel }}: {{ item.responsavel }}
                      </span>
                    </span>
                  </div>

                  <div class="v-status-col">
                    <span v-if="item.estado === 'concluido'" class="badge-concluido">
                      Concluído
                    </span>
                    <span v-else-if="item.estado === 'andamento'" class="badge-andamento">
                      em andamento
                    </span>
                    <span v-else class="text-pendente">pendente</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Produtos do projeto (exibido na aba Dados Gerais) -->
        <section v-if="abaAtiva === 'gerais'" class="products-section">
          <h2 class="products-section-title">Produtos do projeto</h2>

          <div class="products-box">
            <div class="products-table-wrapper">
              <table class="products-list-table">
                <thead>
                  <tr>
                    <th>Produto</th>
                    <th>Localização</th>
                    <th>Medidas</th>
                    <th>Observações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(prod, index) in obra.produtos" :key="index">
                    <td>{{ prod.nome }}</td>
                    <td>{{ prod.localizacao || '-' }}</td>
                    <td>{{ prod.medidas || '-' }}</td>
                    <td>{{ prod.observacoes || '' }}</td>
                  </tr>
                  <tr v-if="obra.produtos.length === 0">
                    <td colspan="4" class="empty-text">Nenhum produto vinculado a este projeto.</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </section>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import AppSidebar from '@/components/AppSidebar.vue'
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import {
  Home,
  Calendar,
  Wrench,
  ClipboardList,
  Users,
  FileText,
  ArrowLeft,
  Pencil,
  Trash2,
  Check,
  Factory,
  Flag,
  Calculator,
} from 'lucide-vue-next'

const API_URL = 'http://localhost:5006/api'
const STORAGE_INATIVAS_KEY = 'sa_esquadrias_obras_inativas'
const STORAGE_OBRAS_EDITADAS_KEY = 'sa_esquadrias_obras_editadas'

const route = useRoute()
const router = useRouter()

type EstadoEtapa = 'concluido' | 'andamento' | 'pendente'

interface EtapaHorizontal {
  chave: string
  titulo: string
  dataCurta: string
  estado: EstadoEtapa
  icone: 'check' | 'factory' | 'wrench' | 'flag'
}

interface HistoricoAndamentoItem {
  titulo: string
  dataCompleta: string
  responsavelLabel: string
  responsavel: string
  estado: EstadoEtapa
  icone: 'check' | 'medicao' | 'factory' | 'wrench' | 'flag'
}

interface ProdutoProjeto {
  id?: number
  nome: string
  quantidade?: number
  localizacao: string
  medidas: string
  observacoes: string
}

interface ObraDetalhe {
  id: string | number
  numeroProjeto: string
  status: string
  cep?: string
  rua?: string
  numero?: string
  complemento?: string
  bairro?: string
  cidade?: string
  estado?: string
  clienteId?: number | string
  vendedorId?: number | string
  equipeId?: number | string
  prazoIso?: string
  endereco: string
  vendedor: string
  prazoEntrega: string
  clienteNome: string
  clienteTelefone: string
  clienteEmail: string
  observacoes: string
  produtos: ProdutoProjeto[]
  etapas: EtapaHorizontal[]
  historicoDetalhado: HistoricoAndamentoItem[]
}

const abaAtiva = ref<'gerais' | 'historico'>('gerais')
const excluindo = ref(false)

const gerarEtapasPadrao = (
  statusAtual: string,
  vendedorNome = 'Ana Carolina',
): {
  etapas: EtapaHorizontal[]
  historicoDetalhado: HistoricoAndamentoItem[]
} => {
  const s = statusAtual.toLowerCase()

  let estadoProducao: EstadoEtapa = 'concluido'
  let estadoInstalacao: EstadoEtapa = 'andamento'
  let estadoFinalizacao: EstadoEtapa = 'pendente'

  if (s.includes('finalizado') || s.includes('concluído')) {
    estadoProducao = 'concluido'
    estadoInstalacao = 'concluido'
    estadoFinalizacao = 'concluido'
  } else if (
    s.includes('produção') ||
    s.includes('producao') ||
    s.includes('liberação') ||
    s.includes('pendência')
  ) {
    estadoProducao = 'andamento'
    estadoInstalacao = 'pendente'
    estadoFinalizacao = 'pendente'
  }

  return {
    etapas: [
      { chave: 'venda', titulo: 'Venda', dataCurta: '10/06', estado: 'concluido', icone: 'check' },
      {
        chave: 'medicao',
        titulo: 'Medição',
        dataCurta: '12/08',
        estado: 'concluido',
        icone: 'check',
      },
      {
        chave: 'producao',
        titulo: 'Produção',
        dataCurta: '25/08',
        estado: estadoProducao,
        icone: 'factory',
      },
      {
        chave: 'instalacao',
        titulo: 'Instalação',
        dataCurta: '07/10',
        estado: estadoInstalacao,
        icone: 'wrench',
      },
      {
        chave: 'finalizacao',
        titulo: 'Finalização',
        dataCurta: '',
        estado: estadoFinalizacao,
        icone: 'flag',
      },
    ],
    historicoDetalhado: [
      {
        titulo: 'Venda realizada',
        dataCompleta: '10/08/2025',
        responsavelLabel: 'Vendedor',
        responsavel: vendedorNome,
        estado: 'concluido',
        icone: 'check',
      },
      {
        titulo: 'Medição final',
        dataCompleta: '12/08/2025',
        responsavelLabel: 'Responsável',
        responsavel: 'Gabriel',
        estado: 'concluido',
        icone: 'medicao',
      },
      {
        titulo: 'Produção',
        dataCompleta: estadoProducao === 'concluido' ? '12/08/2025' : '',
        responsavelLabel: 'Responsável',
        responsavel: 'Produção',
        estado: estadoProducao,
        icone: 'factory',
      },
      {
        titulo: 'Instalação',
        dataCompleta: estadoInstalacao !== 'pendente' ? '07/10/2025' : '',
        responsavelLabel: 'Responsável',
        responsavel: 'Equipe Externa',
        estado: estadoInstalacao,
        icone: 'wrench',
      },
      {
        titulo: 'Finalização',
        dataCompleta: estadoFinalizacao === 'concluido' ? '20/10/2025' : '',
        responsavelLabel: 'Responsável',
        responsavel: 'Gestor',
        estado: estadoFinalizacao,
        icone: 'flag',
      },
    ],
  }
}

const mocksPorId: Record<string, ObraDetalhe> = {
  '15': {
    id: 15,
    numeroProjeto: '#0015',
    status: 'Aguardando liberação',
    cep: '17500-000',
    rua: 'Rua das Flores',
    numero: '123',
    complemento: '',
    bairro: 'Centro',
    cidade: 'Marília',
    estado: 'SP',
    prazoIso: '2026-12-15',
    endereco: 'Rua das flores, 123 - Marília/SP',
    vendedor: 'Ana Carolina',
    prazoEntrega: '15 a 45 dias',
    clienteNome: 'João Galindo',
    clienteTelefone: '(14) 9986251432',
    clienteEmail: 'joao.galindo@gmail.com',
    observacoes: 'Condomínio em fase da acabamento',
    produtos: [
      {
        id: 1,
        nome: 'Porta',
        quantidade: 1,
        localizacao: 'Sala',
        medidas: '2,10 × 1,00',
        observacoes: '',
      },
    ],
    ...gerarEtapasPadrao('Em instalação', 'Ana Carolina'),
  },
  '18': {
    id: 18,
    numeroProjeto: '#0018',
    status: 'Com pendência',
    cep: '17501-010',
    rua: 'Av. Principal',
    numero: '456',
    complemento: '',
    bairro: 'Centro',
    cidade: 'Marília',
    estado: 'SP',
    prazoIso: '2026-12-20',
    endereco: 'Av. Principal, 456 - Marília/SP',
    vendedor: 'Ana Carolina',
    prazoEntrega: '20 dias',
    clienteNome: 'Edificio Central',
    clienteTelefone: '(14) 99712-3344',
    clienteEmail: 'contato@edificiocentral.com.br',
    observacoes: 'Aguardando confirmação de medidas do vão principal',
    produtos: [
      {
        id: 2,
        nome: 'Janela',
        quantidade: 2,
        localizacao: 'Fachada',
        medidas: '1,50 × 1,20',
        observacoes: 'Vidro temperado',
      },
    ],
    ...gerarEtapasPadrao('Com pendência', 'Ana Carolina'),
  },
  '20': {
    id: 20,
    numeroProjeto: '#0020',
    status: 'Em produção',
    cep: '17502-020',
    rua: 'Rua do Sol',
    numero: '120',
    complemento: '',
    bairro: 'Jardim',
    cidade: 'Marília',
    estado: 'SP',
    prazoIso: '2026-12-25',
    endereco: 'Rua do Sol, 120 - Marília/SP',
    vendedor: 'Ana Carolina',
    prazoEntrega: '25 dias',
    clienteNome: 'Cond. Por do sol',
    clienteTelefone: '(14) 99655-8899',
    clienteEmail: 'administracao@pordosol.com.br',
    observacoes: 'Corte de perfis em andamento na fábrica',
    produtos: [
      {
        id: 3,
        nome: 'Portão',
        quantidade: 1,
        localizacao: 'Garagem',
        medidas: '2,40 × 3,20',
        observacoes: 'Automático',
      },
    ],
    ...gerarEtapasPadrao('Em produção', 'Ana Carolina'),
  },
  '30': {
    id: 30,
    numeroProjeto: '#0030',
    status: 'Em instalação',
    cep: '17503-030',
    rua: 'Rua Babuino',
    numero: '222',
    complemento: '',
    bairro: 'Bela Vista',
    cidade: 'Marília',
    estado: 'SP',
    prazoIso: '2026-12-30',
    endereco: 'Rua Babuino, 222 - Marília/SP',
    vendedor: 'Geovanna',
    prazoEntrega: '30 dias',
    clienteNome: 'Residencial Jardim',
    clienteTelefone: '(14) 99811-2233',
    clienteEmail: 'obras@residencialjardim.com.br',
    observacoes: 'Equipe externa agendada no período da manhã',
    produtos: [
      {
        id: 4,
        nome: 'Box',
        quantidade: 1,
        localizacao: 'Suíte Master',
        medidas: '1,90 × 1,30',
        observacoes: 'Incolor 8mm',
      },
    ],
    ...gerarEtapasPadrao('Em instalação', 'Geovanna'),
  },
}

const obra = ref<ObraDetalhe>(mocksPorId['15']!)

const irPara = (rota: string) => {
  router.push(rota)
}

const irParaEditar = () => {
  router.push(`/editar-obra/${obra.value.id}`)
}

const classeCorStatus = (status: string) => {
  const s = status.toLowerCase()
  if (s.includes('finalizado') || s.includes('instalação') || s.includes('concluído')) {
    return 'status-green'
  }
  return 'status-red'
}

const registrarInativacaoLocal = (id: string | number) => {
  try {
    const raw = localStorage.getItem(STORAGE_INATIVAS_KEY)
    const lista: string[] = raw ? JSON.parse(raw) : []
    if (!lista.includes(String(id))) {
      lista.push(String(id))
      localStorage.setItem(STORAGE_INATIVAS_KEY, JSON.stringify(lista))
    }
  } catch {
    // ignora
  }
}

const inativarObra = async () => {
  const confirmou = window.confirm(
    `Deseja realmente inativar/excluir a obra ${obra.value.numeroProjeto}?\nO registro permanecerá salvo no histórico do banco de dados.`,
  )
  if (!confirmou) return

  excluindo.value = true
  const idAtual = obra.value.id

  try {
    const response = await fetch(`${API_URL}/Obras/${idAtual}/inativar`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ ativo: false }),
    })

    if (!response.ok) {
      await fetch(`${API_URL}/Obras/${idAtual}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          id: idAtual,
          ativo: false,
          status: obra.value.status,
        }),
      })
    }
  } catch (error) {
    console.warn('API indisponível; aplicando inativação lógica localmente.', error)
  } finally {
    registrarInativacaoLocal(idAtual)
    excluindo.value = false
    alert('Obra removida da listagem ativa com sucesso!')
    router.push('/obras')
  }
}

onMounted(async () => {
  const idParam = String(route.params.id ?? '15')

  if (mocksPorId[idParam]) {
    obra.value = { ...mocksPorId[idParam] }
  } else {
    obra.value = {
      ...mocksPorId['15']!,
      id: idParam,
      numeroProjeto: `#${idParam.padStart(4, '0')}`,
    }
  }

  try {
    const editadasRaw = localStorage.getItem(STORAGE_OBRAS_EDITADAS_KEY)
    if (editadasRaw) {
      const mapaEditadas = JSON.parse(editadasRaw)
      if (mapaEditadas[idParam]) {
        const ed = mapaEditadas[idParam]
        obra.value = {
          ...obra.value,
          ...ed,
          endereco: ed.rua
            ? `${ed.rua}, ${ed.numero ?? ''} - ${ed.cidade ?? 'Marília'}/${ed.estado ?? 'SP'}`
            : obra.value.endereco,
          prazoEntrega: ed.prazo || obra.value.prazoEntrega,
          produtos: Array.isArray(ed.produtos)
            ? ed.produtos.map((p: any) => ({
                id: p.id,
                nome: p.nome,
                quantidade: p.quantidade ?? 1,
                localizacao: p.localizacao ?? 'Geral',
                medidas: p.medidas ?? '-',
                observacoes: p.observacoes ?? '',
              }))
            : obra.value.produtos,
          ...gerarEtapasPadrao(ed.status ?? obra.value.status, ed.vendedor ?? obra.value.vendedor),
        }
      }
    }
  } catch {
    // ignora
  }

  try {
    const response = await fetch(`${API_URL}/Obras/${idParam}`)
    if (response.ok) {
      const dados = await response.json()
      const statusCarregado = dados.status ?? obra.value.status
      const vendedorCarregado = dados.vendedor?.nome ?? dados.vendedorNome ?? obra.value.vendedor

      obra.value = {
        ...obra.value,
        id: dados.id ?? idParam,
        numeroProjeto: dados.numeroProjeto ?? `#${String(dados.id ?? idParam).padStart(4, '0')}`,
        status: statusCarregado,
        cep: dados.cep ?? obra.value.cep,
        rua: dados.rua ?? obra.value.rua,
        numero: dados.numero ?? obra.value.numero,
        complemento: dados.complemento ?? obra.value.complemento,
        bairro: dados.bairro ?? obra.value.bairro,
        cidade: dados.cidade ?? obra.value.cidade,
        estado: dados.estado ?? obra.value.estado,
        clienteId: dados.clienteId ?? dados.cliente?.id ?? obra.value.clienteId,
        vendedorId: dados.vendedorId ?? dados.vendedor?.id ?? obra.value.vendedorId,
        equipeId: dados.equipeId ?? dados.equipe?.id ?? obra.value.equipeId,
        prazoIso: dados.prazo ?? obra.value.prazoIso,
        endereco: dados.rua
          ? `${dados.rua}, ${dados.numero ?? ''} - ${dados.cidade ?? 'Marília'}/${dados.estado ?? 'SP'}`
          : (dados.endereco ?? obra.value.endereco),
        vendedor: vendedorCarregado,
        prazoEntrega: dados.prazo ?? obra.value.prazoEntrega,
        clienteNome: dados.cliente?.nome ?? dados.clienteNome ?? obra.value.clienteNome,
        clienteTelefone: dados.cliente?.telefone ?? obra.value.clienteTelefone,
        clienteEmail: dados.cliente?.email ?? obra.value.clienteEmail,
        observacoes: dados.observacoes ?? obra.value.observacoes,
        produtos:
          Array.isArray(dados.produtos) && dados.produtos.length > 0
            ? dados.produtos.map((p: any) => ({
                id: p.id,
                nome: p.nome ?? '-',
                quantidade: p.quantidade ?? 1,
                localizacao: p.localizacao ?? p.ambiente ?? 'Geral',
                medidas: p.medidas ?? '-',
                observacoes: p.observacoes ?? '',
              }))
            : obra.value.produtos,
        ...gerarEtapasPadrao(statusCarregado, vendedorCarregado),
      }
    }
  } catch (error) {
    console.warn('API indisponível; exibindo detalhes locais da obra.', error)
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
  background-color: #ececec;
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
  padding: 28px 56px 24px 36px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.top-bar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 20px;
  margin-bottom: 24px;
  flex-shrink: 0;
}

.top-bar-left {
  display: flex;
  align-items: center;
  gap: 36px;
  flex-wrap: wrap;
}

.top-bar-actions {
  display: flex;
  align-items: center;
  gap: 12px;
}

.btn-excluir-obra {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  background-color: #ffffff;
  color: #dc2626;
  border: 1px solid #dc2626;
  border-radius: 6px;
  padding: 8px 16px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-excluir-obra:hover:not(:disabled) {
  background-color: #fef2f2;
}

.btn-excluir-obra:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-editar-obra {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 9px 18px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-editar-obra:hover {
  background-color: #172554;
}

.btn-voltar {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: transparent;
  border: none;
  color: #6b7280;
  font-size: 16px;
  font-weight: 500;
  cursor: pointer;
  padding: 4px 6px;
  transition: color 0.2s;
}

.btn-voltar:hover {
  color: #111827;
}

.obra-header-info {
  display: flex;
  align-items: center;
  gap: 32px;
  flex-wrap: wrap;
}

.obra-titulo {
  font-size: 26px;
  font-weight: 700;
  color: #000000;
  margin: 0;
}

.obra-subtitulo-cliente {
  font-weight: 400;
  color: #8c8c8c;
  margin-left: 6px;
}

.obra-status {
  font-size: 15px;
  font-weight: 700;
}

.status-red {
  color: #ff0000;
}

.status-green {
  color: #16a34a;
}

.content-scroll-area {
  flex: 1;
  overflow-y: auto;
  padding-right: 4px;
  padding-bottom: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.details-card {
  width: 100%;
  max-width: 780px;
  background-color: #faf6f6;
  border: 1px solid #9ca3af;
  border-radius: 16px;
  padding: 24px 44px 36px 44px;
  margin-bottom: 32px;
}

.details-card.history-mode {
  background-color: #ffffff;
  border-color: #d1d5db;
  max-width: 840px;
  padding: 24px 36px 36px 36px;
}

.tabs-bar {
  display: flex;
  align-items: center;
  gap: 56px;
  margin-bottom: 32px;
}

.tab-btn {
  background: transparent;
  border: none;
  border-bottom: 2px solid transparent;
  padding: 6px 12px;
  font-size: 15px;
  font-weight: 600;
  color: #6b7280;
  cursor: pointer;
  transition: all 0.2s;
}

.tab-btn:hover {
  color: #111827;
}

.tab-btn.active {
  color: #000000;
  border-bottom-color: #f59e0b;
}

.info-columns {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 40px;
}

.info-col {
  display: flex;
  flex-direction: column;
}

.col-heading {
  font-size: 13px;
  font-weight: 700;
  color: #000000;
  margin: 0 0 18px 0;
}

.info-item {
  display: flex;
  flex-direction: column;
  margin-bottom: 24px;
}

.info-label {
  font-size: 12px;
  font-weight: 600;
  color: #6b7280;
  margin-bottom: 3px;
}

.info-value {
  font-size: 12px;
  font-weight: 500;
  color: #4b5563;
  margin: 0;
}

.obs-block {
  margin-top: 8px;
}

.obs-block .col-heading {
  margin-bottom: 10px;
}

/* STEPPER HORIZONTAL */
.stepper-scroll-wrapper {
  width: 100%;
  overflow-x: auto;
  padding-bottom: 8px;
  margin-bottom: 28px;
}

.horizontal-stepper {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  min-width: 620px;
}

.step-node {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 112px;
  flex-shrink: 0;
  text-align: center;
}

.step-circle {
  width: 52px;
  height: 52px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #ffffff;
  margin-bottom: 10px;
}

.step-circle.state-concluido {
  background-color: #2f9e24;
}

.step-circle.state-andamento {
  background-color: #f4be18;
}

.step-circle.state-pendente {
  background-color: #706e6e;
}

.step-title {
  font-size: 15px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 4px;
}

.step-sub-data {
  font-size: 13px;
  color: #6b7280;
}

.step-pill-andamento {
  display: inline-block;
  background-color: #fef3c7;
  color: #78350f;
  border: 1px solid #fde68a;
  border-radius: 999px;
  padding: 3px 12px;
  font-size: 11px;
  font-weight: 600;
  white-space: nowrap;
}

.step-sub-pendente {
  font-size: 13px;
  color: #8c8c8c;
}

.step-connector {
  flex: 1;
  display: flex;
  align-items: center;
  margin-top: 25px;
  min-width: 28px;
}

.connector-line {
  flex: 1;
  height: 2px;
}

.connector-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  flex-shrink: 0;
}

.step-connector.conn-concluido .connector-line,
.step-connector.conn-concluido .connector-dot {
  background-color: #2f9e24;
}

.step-connector.conn-andamento .connector-line,
.step-connector.conn-andamento .connector-dot {
  background-color: #706e6e;
}

.step-connector.conn-andamento .connector-line:first-child {
  background-color: #f4be18;
}

.step-connector.conn-pendente .connector-line,
.step-connector.conn-pendente .connector-dot {
  background-color: #706e6e;
}

/* TIMELINE VERTICAL (HISTÓRICO DO ANDAMENTO) */
.andamento-heading {
  font-size: 16px;
  font-weight: 700;
  color: #000000;
  margin: 0 0 20px 0;
}

.vertical-timeline {
  display: flex;
  flex-direction: column;
}

.v-timeline-row {
  display: flex;
  align-items: flex-start;
  gap: 16px;
}

.v-left-col {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 38px;
  flex-shrink: 0;
}

.v-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #ffffff;
  flex-shrink: 0;
}

.v-circle.state-concluido {
  background-color: #2f9e24;
}

.v-circle.state-andamento {
  background-color: #f4be18;
}

.v-circle.state-pendente {
  background-color: #706e6e;
}

.v-line {
  width: 2px;
  height: 44px;
}

.v-line.vline-concluido {
  background-color: #2f9e24;
}

.v-line.vline-andamento {
  background-color: #f4be18;
}

.v-line.vline-pendente {
  background-color: #706e6e;
}

.v-info-col {
  flex: 1;
  display: flex;
  flex-direction: column;
  padding-top: 2px;
}

.v-title {
  font-size: 15px;
  font-weight: 700;
  color: #000000;
  margin-bottom: 4px;
}

.v-meta {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  color: #6b7280;
}

.v-meta-icon {
  color: #4b5563;
  flex-shrink: 0;
}

.v-status-col {
  padding-top: 6px;
  min-width: 105px;
  text-align: right;
}

.badge-concluido {
  display: inline-block;
  background-color: #c8e6c9;
  color: #2e7d32;
  border-radius: 999px;
  padding: 4px 18px;
  font-size: 12px;
  font-weight: 600;
}

.badge-andamento {
  display: inline-block;
  background-color: #fef3c7;
  color: #78350f;
  border-radius: 999px;
  padding: 4px 14px;
  font-size: 12px;
  font-weight: 600;
}

.text-pendente {
  display: inline-block;
  padding-right: 16px;
  font-size: 13px;
  font-weight: 700;
  color: #706e6e;
}

.products-section {
  width: 100%;
  max-width: 780px;
}

.products-section-title {
  font-size: 16px;
  font-weight: 700;
  color: #000000;
  margin: 0 0 14px 0;
}

.products-box {
  width: 100%;
  border: 1px solid #9ca3af;
  border-radius: 12px;
  padding: 14px 24px;
  background-color: transparent;
}

.products-table-wrapper {
  width: 100%;
  overflow-x: auto;
}

.products-list-table {
  width: 100%;
  min-width: 460px;
  border-collapse: collapse;
  text-align: left;
}

.products-list-table th {
  font-size: 14px;
  font-weight: 700;
  color: #000000;
  padding: 4px 12px 10px 0;
}

.products-list-table td {
  font-size: 13px;
  font-weight: 500;
  color: #6b7280;
  padding: 6px 12px 4px 0;
}

.empty-text {
  font-size: 13px;
  color: #6b7280;
}

@media (max-width: 900px) {
  .main-content {
    padding: 24px;
  }

  .details-card,
  .details-card.history-mode {
    padding: 20px 24px 28px 24px;
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

  .top-bar {
    flex-direction: column;
    align-items: stretch;
    gap: 14px;
    margin-bottom: 18px;
  }

  .top-bar-left {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }

  .top-bar-actions {
    width: 100%;
  }

  .btn-excluir-obra,
  .btn-editar-obra {
    flex: 1;
    justify-content: center;
    padding: 10px 14px;
  }

  .obra-header-info {
    gap: 16px;
  }

  .obra-titulo {
    font-size: 21px;
  }

  .content-scroll-area {
    display: block;
    overflow: visible;
    padding-right: 0;
  }

  .details-card,
  .details-card.history-mode {
    padding: 18px 16px 22px 16px;
    margin-bottom: 24px;
  }

  .tabs-bar {
    gap: 20px;
    margin-bottom: 22px;
  }

  .info-columns {
    grid-template-columns: 1fr;
    gap: 12px;
  }

  .info-item {
    margin-bottom: 16px;
  }

  .v-timeline-row {
    gap: 10px;
  }

  .v-status-col {
    min-width: auto;
  }

  .badge-concluido,
  .badge-andamento {
    padding: 4px 10px;
    font-size: 11px;
  }

  .products-box {
    padding: 12px 14px;
  }
}
</style>
