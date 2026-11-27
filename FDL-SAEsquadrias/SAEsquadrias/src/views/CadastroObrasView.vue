<template>
  <div class="app-container">
    <aside class="sidebar">
      <div class="logo-container">
        <img src="@/assets/logoSA.png" alt="SA Esquadrias" class="logo-img" />
      </div>

      <nav class="sidebar-nav">
        <button type="button" class="nav-item active" title="Início">
          <Home :size="20" />
        </button>

        <button type="button" class="nav-item" title="Agenda">
          <Calendar :size="20" />
        </button>

        <button
          type="button"
          class="nav-item"
          title="Cadastrar Produto"
          @click="irPara('/cadastro-produtos')"
        >
          <Wrench :size="20" />
        </button>

        <button type="button" class="nav-item" title="Ordens / Checklists">
          <ClipboardList :size="20" />
        </button>

        <button type="button" class="nav-item" title="Clientes e Equipes">
          <Users :size="20" />
        </button>

        <button type="button" class="nav-item" title="Relatórios">
          <FileText :size="20" />
        </button>
      </nav>
    </aside>

    <main class="main-content">
      <h1 class="page-title">Cadastro de Obras</h1>

      <form @submit.prevent="cadastrarObra" class="form-wrapper">
        <div class="form-scroll-area">
          <section class="form-section">
            <div class="section-header">
              <h2 class="section-title active-underline">Endereço da Obra</h2>
              <div class="section-line"></div>
            </div>

            <div class="grid-row-4">
              <div class="form-group">
                <label>Cep *</label>
                <div class="input-with-icon">
                  <input
                    v-model="form.cep"
                    type="text"
                    maxlength="9"
                    placeholder="Ex. 17500-000"
                    required
                  />
                  <button
                    type="button"
                    class="icon-btn"
                    @click="buscarCep()"
                    :disabled="buscandoCep"
                    title="Buscar CEP"
                  >
                    <Search :size="16" color="#9CA3AF" />
                  </button>
                </div>
              </div>

              <div class="form-group">
                <label>Rua *</label>
                <input v-model="form.rua" type="text" placeholder="Ex. Rua Bela Vista.." required />
              </div>

              <div class="form-group">
                <label>Número *</label>
                <input v-model="form.numero" type="text" placeholder="Ex. 123" required />
              </div>

              <div class="form-group">
                <label>Complemento</label>
                <input v-model="form.complemento" type="text" placeholder="Ex. Apto 10" />
              </div>
            </div>

            <div class="grid-row-3 mt-4">
              <div class="form-group">
                <label>Bairro *</label>
                <input v-model="form.bairro" type="text" placeholder="Ex. Centro" required />
              </div>

              <div class="form-group">
                <label>Cidade *</label>
                <input v-model="form.cidade" type="text" placeholder="Ex. Marília" required />
              </div>

              <div class="form-group">
                <label>Estado *</label>
                <select v-model="form.estado" required>
                  <option value="" disabled>Selecionar seu Estado</option>
                  <option v-for="uf in estados" :key="uf" :value="uf">{{ uf }}</option>
                </select>
              </div>
            </div>
          </section>

          <section class="form-section">
            <div class="section-header">
              <h2 class="section-title">Informações da Obra</h2>
              <div class="section-line"></div>
            </div>

            <div class="grid-row-3">
              <div class="form-group">
                <label>Cliente *</label>
                <div class="select-with-action">
                  <select v-model="form.clienteId" required>
                    <option value="" disabled>Selecione o Cliente</option>
                    <option v-for="cli in clientes" :key="cli.id" :value="cli.id">
                      {{ cli.nome }}
                    </option>
                  </select>
                  <button type="button" class="btn-outline-blue" @click="abrirModal('cliente')">
                    + Cliente
                  </button>
                </div>
              </div>

              <div class="form-group">
                <label>Vendedor *</label>
                <div class="select-with-action">
                  <select v-model="form.vendedorId" required>
                    <option value="" disabled>Selecione o Vendedor</option>
                    <option v-for="vend in vendedores" :key="vend.id" :value="vend.id">
                      {{ vend.nome }}
                    </option>
                  </select>
                  <button type="button" class="btn-outline-blue" @click="abrirModal('vendedor')">
                    + Vendedor
                  </button>
                </div>
              </div>

              <div class="form-group">
                <label>Equipe *</label>
                <div class="select-with-action">
                  <select v-model="form.equipeId" required>
                    <option value="" disabled>Selecione a Equipe</option>
                    <option v-for="eq in equipes" :key="eq.id" :value="eq.id">
                      {{ eq.nome }}
                    </option>
                  </select>
                  <button type="button" class="btn-outline-blue" @click="abrirModal('equipe')">
                    + Equipe
                  </button>
                </div>
              </div>
            </div>

            <div class="grid-row-prazo-status mt-4">
              <div class="form-group">
                <label>Prazo *</label>
                <div class="input-with-left-icon">
                  <Calendar :size="16" color="#9CA3AF" class="left-icon" />
                  <input v-model="form.prazo" type="date" required />
                </div>
              </div>

              <div class="form-group">
                <label>Status *</label>
                <select v-model="form.status" required>
                  <option value="Andamento">Andamento</option>
                  <option value="Pendente">Pendente</option>
                  <option value="Concluído">Concluído</option>
                </select>
              </div>
            </div>

            <div class="form-group mt-4">
              <label>Observações</label>
              <div class="textarea-wrapper">
                <Calendar :size="16" color="#9CA3AF" class="textarea-icon" />
                <textarea
                  v-model="form.observacoes"
                  rows="2"
                  placeholder="Observações gerais da obra..."
                ></textarea>
              </div>
            </div>
          </section>

          <section class="form-section">
            <div class="section-header">
              <h2 class="section-title">Produtos utilizados</h2>
              <div class="section-line"></div>
              <div class="header-actions ml-4">
                <button
                  type="button"
                  class="btn-outline-blue btn-header-height"
                  @click="irPara('/cadastro-produtos')"
                >
                  + Novo Produto
                </button>
                <button type="button" class="btn-primary-blue" @click="adicionarProduto">
                  + Adicionar Produto
                </button>
              </div>
            </div>

            <div class="table-container">
              <table class="products-table">
                <thead>
                  <tr>
                    <th>Produto</th>
                    <th class="text-center" style="width: 120px">Quantidade</th>
                    <th class="text-center" style="width: 180px">Medidas</th>
                    <th>Observações</th>
                    <th class="text-center" style="width: 150px">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in produtosAtivos" :key="item.id">
                    <template v-if="item.editando">
                      <td>
                        <select
                          v-model="item.nome"
                          class="table-input"
                          @change="aoSelecionarProduto(item)"
                        >
                          <option value="" disabled>Selecione o produto</option>
                          <option
                            v-for="prod in catalogoProdutos"
                            :key="prod.id"
                            :value="prod.nome"
                          >
                            {{ prod.nome }}
                          </option>
                        </select>
                      </td>
                      <td class="text-center">
                        <input
                          v-model.number="item.quantidade"
                          type="number"
                          min="1"
                          class="table-input text-center"
                          @keydown.enter.prevent="salvarLinhaProduto(item)"
                        />
                      </td>
                      <td class="text-center">
                        <input
                          v-model="item.medidas"
                          type="text"
                          class="table-input text-center"
                          placeholder="Ex. 2,5 × 1,0"
                          @keydown.enter.prevent="salvarLinhaProduto(item)"
                        />
                      </td>
                      <td>
                        <input
                          v-model="item.observacoes"
                          type="text"
                          class="table-input"
                          placeholder="Ex. Branco, linha Suprema"
                          @keydown.enter.prevent="salvarLinhaProduto(item)"
                        />
                      </td>
                      <td class="text-center actions-cell">
                        <button type="button" class="action-save" @click="salvarLinhaProduto(item)">
                          Salvar
                        </button>
                        <button
                          type="button"
                          class="action-delete"
                          @click="desativarProduto(item.id)"
                        >
                          Excluir
                        </button>
                      </td>
                    </template>

                    <template v-else>
                      <td>{{ item.nome }}</td>
                      <td class="text-center">{{ item.quantidade }}</td>
                      <td class="text-center">{{ item.medidas || '-' }}</td>
                      <td>{{ item.observacoes || '-' }}</td>
                      <td class="text-center actions-cell">
                        <button type="button" class="action-edit" @click="editarProduto(item.id)">
                          Editar
                        </button>
                        <button
                          type="button"
                          class="action-delete"
                          @click="desativarProduto(item.id)"
                        >
                          Excluir
                        </button>
                      </td>
                    </template>
                  </tr>

                  <tr v-if="produtosAtivos.length === 0">
                    <td colspan="5" class="empty-row">Nenhum produto adicionado.</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </section>
        </div>

        <div class="form-footer">
          <button type="button" class="btn-cancel" @click="cancelar" :disabled="salvando">
            Cancelar
          </button>
          <button type="submit" class="btn-submit" :disabled="salvando">
            {{ salvando ? 'Cadastrando...' : 'Cadastrar' }}
          </button>
        </div>
      </form>
    </main>

    <ModalCadastroCliente
      :is-open="modalClienteAberto"
      @close="modalClienteAberto = false"
      @saved="aoSalvarCliente"
    />

    <ModalCadastroVendedor
      :is-open="modalVendedorAberto"
      @close="modalVendedorAberto = false"
      @saved="aoSalvarVendedor"
    />

    <ModalCadastroEquipe
      :is-open="modalEquipeAberto"
      @close="modalEquipeAberto = false"
      @saved="aoSalvarEquipe"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, computed, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Home, Calendar, Wrench, ClipboardList, Users, FileText, Search } from 'lucide-vue-next'
import ModalCadastroCliente from '@/components/ModalCadastroCliente.vue'
import ModalCadastroVendedor from '@/components/ModalCadastroVendedor.vue'
import ModalCadastroEquipe from '@/components/ModalCadastroEquipe.vue'

const API_URL = 'http://localhost:5006/api'
const STORAGE_PRODUTOS_KEY = 'sa_esquadrias_catalogo_produtos'

const router = useRouter()

interface SelectOption {
  id: number | string
  nome: string
}

interface ProdutoCatalogo {
  id: number | string
  nome: string
  medidasPadrao: string
  observacoesPadrao?: string
}

interface ProdutoItem {
  id: number
  nome: string
  quantidade: number
  medidas: string
  observacoes: string
  ativo: boolean
  editando?: boolean
}

interface ObraForm {
  cep: string
  rua: string
  numero: string
  complemento: string
  bairro: string
  cidade: string
  estado: string
  clienteId: number | string
  vendedorId: number | string
  equipeId: number | string
  prazo: string
  status: string
  observacoes: string
}

const estados = [
  'AC',
  'AL',
  'AP',
  'AM',
  'BA',
  'CE',
  'DF',
  'ES',
  'GO',
  'MA',
  'MT',
  'MS',
  'MG',
  'PA',
  'PB',
  'PR',
  'PE',
  'PI',
  'RJ',
  'RN',
  'RS',
  'RO',
  'RR',
  'SC',
  'SP',
  'SE',
  'TO',
]

const produtosIniciais: ProdutoCatalogo[] = [
  { id: 1, nome: 'Porta', medidasPadrao: '2,5 × 1,0', observacoesPadrao: 'Branco, linha Suprema' },
  { id: 2, nome: 'Janela', medidasPadrao: '1,2 × 1,5', observacoesPadrao: 'Vidro temperado' },
  { id: 3, nome: 'Basculante', medidasPadrao: '0,6 × 0,6', observacoesPadrao: '' },
  { id: 4, nome: 'Portão', medidasPadrao: '2,2 × 3,0', observacoesPadrao: '' },
  { id: 5, nome: 'Box', medidasPadrao: '1,9 × 1,2', observacoesPadrao: 'Vidro incolor 8mm' },
]

const catalogoProdutos = ref<ProdutoCatalogo[]>([...produtosIniciais])

const buscandoCep = ref(false)
const salvando = ref(false)
const modalClienteAberto = ref(false)
const modalVendedorAberto = ref(false)
const modalEquipeAberto = ref(false)

const clientes = ref<SelectOption[]>([])
const vendedores = ref<SelectOption[]>([])
const equipes = ref<SelectOption[]>([])

const form = reactive<ObraForm>({
  cep: '',
  rua: '',
  numero: '',
  complemento: '',
  bairro: '',
  cidade: '',
  estado: '',
  clienteId: '',
  vendedorId: '',
  equipeId: '',
  prazo: '',
  status: 'Andamento',
  observacoes: '',
})

const produtos = ref<ProdutoItem[]>([])

const produtosAtivos = computed(() => produtos.value.filter((item) => item.ativo))

const irPara = (rota: string) => {
  router.push(rota)
}

watch(
  () => form.cep,
  (novoCep) => {
    const cepLimpo = novoCep.replace(/\D/g, '')

    if (cepLimpo.length <= 8) {
      const formatado = cepLimpo.replace(/^(\d{5})(\d)/, '$1-$2')
      if (form.cep !== formatado) {
        form.cep = formatado
      }
    }

    if (cepLimpo.length === 8) {
      buscarCep(cepLimpo)
    }
  },
)

const buscarCep = async (cepParam?: string) => {
  const cep = typeof cepParam === 'string' ? cepParam : form.cep.replace(/\D/g, '')
  if (cep.length !== 8 || buscandoCep.value) return

  buscandoCep.value = true
  try {
    const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`)
    const data = await response.json()

    if (data.erro) {
      alert('CEP não encontrado.')
      return
    }

    form.rua = data.logradouro || ''
    form.bairro = data.bairro || ''
    form.cidade = data.localidade || ''
    form.estado = data.uf || ''
  } catch (error) {
    console.error('Erro ao buscar CEP:', error)
    alert('Não foi possível consultar o CEP no momento.')
  } finally {
    buscandoCep.value = false
  }
}

onMounted(async () => {
  const salvos = localStorage.getItem(STORAGE_PRODUTOS_KEY)
  if (salvos) {
    try {
      const listaSalva = JSON.parse(salvos)
      if (Array.isArray(listaSalva) && listaSalva.length > 0) {
        catalogoProdutos.value = listaSalva
      }
    } catch {
      catalogoProdutos.value = [...produtosIniciais]
    }
  } else {
    localStorage.setItem(STORAGE_PRODUTOS_KEY, JSON.stringify(produtosIniciais))
  }

  try {
    const [resClientes, resVendedores, resEquipes] = await Promise.all([
      fetch(`${API_URL}/Clientes`),
      fetch(`${API_URL}/Vendedores`),
      fetch(`${API_URL}/Equipe`),
    ])

    if (resClientes.ok) clientes.value = await resClientes.json()
    if (resVendedores.ok) vendedores.value = await resVendedores.json()
    if (resEquipes.ok) equipes.value = await resEquipes.json()
  } catch (error) {
    console.warn(
      'Verifique se a API está rodando em localhost:5006 e se o CORS está habilitado:',
      error,
    )
  }
})

const aoSelecionarProduto = (item: ProdutoItem) => {
  const encontrado = catalogoProdutos.value.find((p) => p.nome === item.nome)
  if (encontrado) {
    item.medidas = encontrado.medidasPadrao
    if (encontrado.observacoesPadrao && !item.observacoes) {
      item.observacoes = encontrado.observacoesPadrao
    }
  }
}

const cadastrarObra = async () => {
  const linhaEmEdicao = produtosAtivos.value.find((p) => p.editando)
  if (linhaEmEdicao) {
    alert('Salve ou exclua o produto que está em edição antes de cadastrar a obra.')
    return
  }

  salvando.value = true

  const payload = {
    cep: form.cep.replace(/\D/g, ''),
    rua: form.rua,
    numero: form.numero,
    complemento: form.complemento,
    bairro: form.bairro,
    cidade: form.cidade,
    estado: form.estado,
    clienteId: form.clienteId,
    vendedorId: form.vendedorId,
    equipeId: form.equipeId,
    prazo: form.prazo,
    status: form.status,
    observacoes: form.observacoes,
    produtos: produtosAtivos.value.map(({ editando, ...resto }) => resto),
  }

  try {
    const response = await fetch(`${API_URL}/Obras`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    })

    if (!response.ok) {
      const erroTexto = await response.text()
      throw new Error(erroTexto || `Erro HTTP: ${response.status}`)
    }

    const data = await response.json()
    alert('Obra cadastrada com sucesso!')
    console.log('Resposta do servidor:', data)
  } catch (error: any) {
    console.error('Erro ao cadastrar obra:', error)
    alert(error.message || 'Erro ao conectar com a API em localhost:5006.')
  } finally {
    salvando.value = false
  }
}

const abrirModal = (tipo: string) => {
  if (tipo === 'cliente') {
    modalClienteAberto.value = true
  } else if (tipo === 'vendedor') {
    modalVendedorAberto.value = true
  } else if (tipo === 'equipe') {
    modalEquipeAberto.value = true
  }
}

const aoSalvarCliente = (novoCliente: SelectOption) => {
  clientes.value.push(novoCliente)
  form.clienteId = novoCliente.id
}

const aoSalvarVendedor = (novoVendedor: SelectOption) => {
  vendedores.value.push(novoVendedor)
  form.vendedorId = novoVendedor.id
}

const aoSalvarEquipe = (novaEquipe: SelectOption) => {
  equipes.value.push(novaEquipe)
  form.equipeId = novaEquipe.id
}

const adicionarProduto = () => {
  const jaTemLinhaAberta = produtosAtivos.value.find((p) => p.editando)
  if (jaTemLinhaAberta) {
    alert('Finalize o preenchimento do produto atual antes de adicionar outro.')
    return
  }

  produtos.value.push({
    id: Date.now(),
    nome: '',
    quantidade: 1,
    medidas: '',
    observacoes: '',
    ativo: true,
    editando: true,
  })
}

const salvarLinhaProduto = (item: ProdutoItem) => {
  if (!item.nome.trim()) {
    alert('Selecione um produto.')
    return
  }
  if (!item.quantidade || item.quantidade < 1) {
    item.quantidade = 1
  }
  item.nome = item.nome.trim()
  item.medidas = item.medidas.trim()
  item.observacoes = item.observacoes.trim()
  item.editando = false
}

const editarProduto = (id: number) => {
  const item = produtos.value.find((p) => p.id === id)
  if (item) {
    item.editando = true
  }
}

const desativarProduto = (id: number) => {
  const item = produtos.value.find((p) => p.id === id)
  if (item) {
    item.ativo = false
    item.editando = false
  }
}

const cancelar = () => {
  if (confirm('Deseja limpar o formulário?')) {
    form.cep = ''
    form.rua = ''
    form.numero = ''
    form.complemento = ''
    form.bairro = ''
    form.cidade = ''
    form.estado = ''
    form.clienteId = ''
    form.vendedorId = ''
    form.equipeId = ''
    form.prazo = ''
    form.status = 'Andamento'
    form.observacoes = ''
    produtos.value = []
  }
}
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
  padding: 24px 48px 0 48px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.page-title {
  font-size: 24px;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 18px 0;
  flex-shrink: 0;
}

.form-wrapper {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-height: 0;
}

.form-scroll-area {
  flex: 1;
  overflow-y: auto;
  padding-right: 6px;
  padding-bottom: 16px;
}

.form-section {
  margin-bottom: 18px;
}

.section-header {
  display: flex;
  align-items: center;
  margin-bottom: 12px;
}

.section-title {
  font-size: 15px;
  font-weight: 600;
  color: #1e293b;
  margin: 0;
  white-space: nowrap;
  padding-bottom: 2px;
}

.section-title.active-underline {
  border-bottom: 2px solid #0284c7;
}

.section-line {
  flex: 1;
  height: 1px;
  background-color: #475569;
  margin-left: 14px;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}

.btn-header-height {
  height: 31px;
}

.grid-row-4 {
  display: grid;
  grid-template-columns: 1.2fr 1.6fr 0.7fr 1.1fr;
  gap: 20px;
}

.grid-row-3 {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}

.grid-row-prazo-status {
  display: grid;
  grid-template-columns: 1.4fr 0.8fr 1fr;
  gap: 20px;
}

.mt-4 {
  margin-top: 12px;
}

.ml-4 {
  margin-left: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group label {
  font-size: 13px;
  font-weight: 600;
  color: #475569;
  margin-bottom: 5px;
}

input,
select,
textarea {
  width: 100%;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  padding: 8px 12px;
  font-size: 13px;
  color: #1e293b;
  background-color: #ffffff;
  outline: none;
  transition: border-color 0.2s;
}

input::placeholder,
textarea::placeholder {
  color: #cbd5e1;
}

select {
  color: #64748b;
}

input:focus,
select:focus,
textarea:focus {
  border-color: #1d4ed8;
}

.table-input {
  padding: 6px 8px;
  font-size: 13px;
}

.input-with-icon {
  position: relative;
  display: flex;
  align-items: center;
}

.input-with-icon input {
  padding-right: 36px;
}

.icon-btn {
  position: absolute;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
}

.input-with-left-icon {
  position: relative;
  display: flex;
  align-items: center;
}

.input-with-left-icon .left-icon {
  position: absolute;
  left: 12px;
  pointer-events: none;
}

.input-with-left-icon input {
  padding-left: 36px;
}

.textarea-wrapper {
  position: relative;
}

.textarea-icon {
  position: absolute;
  top: 10px;
  left: 12px;
  pointer-events: none;
}

.textarea-wrapper textarea {
  padding-left: 36px;
  resize: vertical;
}

.select-with-action {
  display: flex;
  gap: 8px;
}

.btn-outline-blue {
  background-color: #ffffff;
  color: #1e3a8a;
  border: 1px solid #1e3a8a;
  border-radius: 6px;
  padding: 0 14px;
  font-size: 12px;
  font-weight: 600;
  white-space: nowrap;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-outline-blue:hover {
  background-color: #eff6ff;
}

.btn-primary-blue {
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 8px 16px;
  font-size: 12px;
  font-weight: 600;
  white-space: nowrap;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-primary-blue:hover {
  background-color: #172554;
}

.table-container {
  background-color: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  overflow: hidden;
  min-height: 105px;
}

.products-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}

.products-table thead {
  background-color: #d9d9d9;
  color: #475569;
}

.products-table th {
  padding: 10px 16px;
  font-weight: 600;
  text-align: left;
}

.products-table td {
  padding: 10px 12px;
  color: #334155;
  border-bottom: 1px solid #f1f5f9;
  vertical-align: middle;
}

.text-center {
  text-align: center !important;
}

.actions-cell {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  min-height: 42px;
}

.action-save {
  background: none;
  border: none;
  color: #1e3a8a;
  font-weight: 600;
  cursor: pointer;
}

.action-save:hover {
  text-decoration: underline;
}

.action-edit {
  background: none;
  border: none;
  color: #475569;
  font-weight: 500;
  cursor: pointer;
}

.action-edit:hover {
  text-decoration: underline;
}

.action-delete {
  background: none;
  border: none;
  color: #dc2626;
  font-weight: 600;
  cursor: pointer;
}

.action-delete:hover {
  text-decoration: underline;
}

.empty-row {
  text-align: center;
  color: #94a3b8;
  padding: 24px !important;
}

.form-footer {
  display: flex;
  justify-content: flex-end;
  gap: 14px;
  padding: 14px 0 20px 0;
  background-color: #f8f9fa;
  border-top: 1px solid #e2e8f0;
  flex-shrink: 0;
}

.btn-cancel {
  background-color: #ffffff;
  color: #1e3a8a;
  border: 1px solid #1e3a8a;
  border-radius: 6px;
  padding: 8px 24px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
}

.btn-cancel:hover {
  background-color: #f8fafc;
}

.btn-submit {
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 8px 26px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
}

.btn-submit:hover {
  background-color: #172554;
}

.btn-submit:disabled,
.btn-cancel:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

@media (max-width: 1024px) {
  .grid-row-4,
  .grid-row-3,
  .grid-row-prazo-status {
    grid-template-columns: 1fr;
  }
  .main-content {
    padding: 24px 24px 0 24px;
  }
}
</style>
