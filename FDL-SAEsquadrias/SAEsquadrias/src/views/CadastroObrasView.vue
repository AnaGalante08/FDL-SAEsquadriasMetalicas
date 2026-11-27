<template>
  <div class="app-container">
    <aside class="sidebar">
      <div class="logo-container">
        <svg width="42" height="38" viewBox="0 0 42 38" fill="none">
          <path d="M21 2L4 34H14L21 20L28 34H38L21 2Z" fill="#3B82F6" />
          <path d="M21 10L11 30H31L21 10Z" fill="#94A3B8" opacity="0.85" />
        </svg>
      </div>

      <nav class="sidebar-nav">
        <button type="button" class="nav-item active" title="Início">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
            <polyline points="9 22 9 12 15 12 15 22"></polyline>
          </svg>
        </button>

        <button type="button" class="nav-item" title="Agenda">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
            <line x1="16" y1="2" x2="16" y2="6"></line>
            <line x1="8" y1="2" x2="8" y2="6"></line>
            <line x1="3" y1="10" x2="21" y2="10"></line>
          </svg>
        </button>

        <button type="button" class="nav-item" title="Obras e Serviços">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"></path>
          </svg>
        </button>

        <button type="button" class="nav-item" title="Ordens / Checklists">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path>
            <rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect>
          </svg>
        </button>

        <button type="button" class="nav-item" title="Clientes e Equipes">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
            <circle cx="9" cy="7" r="4"></circle>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
          </svg>
        </button>

        <button type="button" class="nav-item" title="Relatórios">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
            <polyline points="14 2 14 8 20 8"></polyline>
            <line x1="16" y1="13" x2="8" y2="13"></line>
            <line x1="16" y1="17" x2="8" y2="17"></line>
          </svg>
        </button>
      </nav>
    </aside>

    <main class="main-content">
      <h1 class="page-title">Cadastro de Obras</h1>

      <form @submit.prevent="cadastrarObra" class="form-wrapper">
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
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#9CA3AF" stroke-width="2">
                    <circle cx="11" cy="11" r="8"></circle>
                    <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
                  </svg>
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

        <!-- Seção 2: Informações da Obra -->
        <section class="form-section">
          <div class="section-header">
            <h2 class="section-title">Informações da Obra</h2>
            <div class="section-line"></div>
          </div>

          <div class="grid-row-3">
            <!-- Cliente -->
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

            <!-- Vendedor -->
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

            <!-- Equipe -->
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
                <svg class="left-icon" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#9CA3AF" stroke-width="2">
                  <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
                  <line x1="16" y1="2" x2="16" y2="6"></line>
                  <line x1="8" y1="2" x2="8" y2="6"></line>
                  <line x1="3" y1="10" x2="21" y2="10"></line>
                </svg>
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
              <svg class="textarea-icon" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#9CA3AF" stroke-width="2">
                <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
                <line x1="16" y1="2" x2="16" y2="6"></line>
                <line x1="8" y1="2" x2="8" y2="6"></line>
                <line x1="3" y1="10" x2="21" y2="10"></line>
              </svg>
              <textarea
                v-model="form.observacoes"
                rows="3"
                placeholder="Observações gerais da obra..."
              ></textarea>
            </div>
          </div>
        </section>

        <!-- Seção 3: Produtos utilizados -->
        <section class="form-section">
          <div class="section-header">
            <h2 class="section-title">Produtos utilizados</h2>
            <div class="section-line"></div>
            <button type="button" class="btn-primary-blue ml-4" @click="adicionarProduto">
              + Adicionar Produto
            </button>
          </div>

          <div class="table-container">
            <table class="products-table">
              <thead>
                <tr>
                  <th>Produto</th>
                  <th class="text-center">Quantidade</th>
                  <th class="text-center">Medidas</th>
                  <th>Observações</th>
                  <th class="text-center">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item, index) in produtos" :key="item.id">
                  <td>{{ item.nome }}</td>
                  <td class="text-center">{{ item.quantidade }}</td>
                  <td class="text-center">{{ item.medidas }}</td>
                  <td>{{ item.observacoes }}</td>
                  <td class="text-center actions-cell">
                    <button type="button" class="action-edit" @click="editarProduto(index)">
                      Editar
                    </button>
                    <button type="button" class="action-delete" @click="removerProduto(index)">
                      Excluir
                    </button>
                  </td>
                </tr>
                <tr v-if="produtos.length === 0">
                  <td colspan="5" class="empty-row">Nenhum produto adicionado.</td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>

        <!-- Botões de Rodapé -->
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
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, watch, onMounted } from 'vue';
import axios from 'axios';

// ==========================================
// 1. CONFIGURAÇÃO DA SUA API (BACKEND .NET)
// ==========================================
const api = axios.create({
  baseURL: 'http://localhost:5006', // Se suas rotas no Swagger começarem com /api, altere para 'http://localhost:5006/api'
  headers: {
    'Content-Type': 'application/json'
  }
});

// ==========================================
// 2. TIPAGENS (TYPESCRIPT)
// ==========================================
interface SelectOption {
  id: number | string;
  nome: string;
}

interface ProdutoItem {
  id: number;
  nome: string;
  quantidade: number;
  medidas: string;
  observacoes: string;
}

interface ObraForm {
  cep: string;
  rua: string;
  numero: string;
  complemento: string;
  bairro: string;
  cidade: string;
  estado: string;
  clienteId: number | string;
  vendedorId: number | string;
  equipeId: number | string;
  prazo: string;
  status: string;
  observacoes: string;
}

// ==========================================
// 3. ESTADOS E DADOS REATIVOS
// ==========================================
const estados = [
  'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
  'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN',
  'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'
];

const buscandoCep = ref(false);
const salvando = ref(false);

const clientes = ref<SelectOption[]>([]);
const vendedores = ref<SelectOption[]>([]);
const equipes = ref<SelectOption[]>([]);

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
  observacoes: ''
});

const produtos = ref<ProdutoItem[]>([
  {
    id: 1,
    nome: 'Porta',
    quantidade: 1,
    medidas: '2,5 × 1,0',
    observacoes: 'Branco, linha Suprema'
  }
]);

// ==========================================
// 4. INTEGRAÇÃO COM VIACEP (AUTOMÁTICA)
// ==========================================
watch(
  () => form.cep,
  (novoCep) => {
    const cepLimpo = novoCep.replace(/\D/g, '');

    if (cepLimpo.length <= 8) {
      const formatado = cepLimpo.replace(/^(\d{5})(\d)/, '$1-$2');
      if (form.cep !== formatado) {
        form.cep = formatado;
      }
    }

    if (cepLimpo.length === 8) {
      buscarCep(cepLimpo);
    }
  }
);

const buscarCep = async (cepParam?: string) => {
  const cep = typeof cepParam === 'string' ? cepParam : form.cep.replace(/\D/g, '');
  if (cep.length !== 8 || buscandoCep.value) return;

  buscandoCep.value = true;
  try {
    const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
    const data = await response.json();

    if (data.erro) {
      alert('CEP não encontrado.');
      return;
    }

    form.rua = data.logradouro || '';
    form.bairro = data.bairro || '';
    form.cidade = data.localidade || '';
    form.estado = data.uf || '';
  } catch (error) {
    console.error('Erro ao buscar CEP:', error);
    alert('Não foi possível consultar o CEP no momento.');
  } finally {
    buscandoCep.value = false;
  }
};

// ==========================================
// 5. INTEGRAÇÃO COM A SUA API (GET E POST)
// ==========================================
onMounted(async () => {
  try {
    const [resClientes, resVendedores, resEquipes] = await Promise.all([
      api.get('/Clientes'),
      api.get('/Vendedores'),
      api.get('/Equipes')
    ]);

    clientes.value = resClientes.data;
    vendedores.value = resVendedores.data;
    equipes.value = resEquipes.data;
  } catch (error) {
    console.warn('API offline ou rotas diferentes de /Clientes, /Vendedores, /Equipes:', error);
  }
});

const cadastrarObra = async () => {
  salvando.value = true;

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
    produtos: produtos.value
  };

  try {
    const response = await api.post('/Obras', payload);
    alert('Obra cadastrada com sucesso!');
    console.log('Resposta do servidor:', response.data);
  } catch (error: any) {
    console.error('Erro ao cadastrar obra:', error);
    alert(error.response?.data?.message || 'Erro ao conectar com a API em localhost:5006.');
  } finally {
    salvando.value = false;
  }
};

// ==========================================
// 6. AÇÕES AUXILIARES DA TELA
// ==========================================
const abrirModal = (tipo: string) => {
  alert(`Abrir modal para cadastrar novo(a): ${tipo}`);
};

const adicionarProduto = () => {
  produtos.value.push({
    id: Date.now(),
    nome: 'Janela',
    quantidade: 2,
    medidas: '1,2 × 1,5',
    observacoes: 'Vidro temperado'
  });
};

const editarProduto = (index: number) => {
  const item = produtos.value[index];
  if (item) {
    alert(`Editando produto: ${item.nome}`);
  }
};

const removerProduto = (index: number) => {
  produtos.value.splice(index, 1);
};

const cancelar = () => {
  if (confirm('Deseja limpar o formulário?')) {
    form.cep = '';
    form.rua = '';
    form.numero = '';
    form.complemento = '';
    form.bairro = '';
    form.cidade = '';
    form.estado = '';
    form.clienteId = '';
    form.vendedorId = '';
    form.equipeId = '';
    form.prazo = '';
    form.status = 'Andamento';
    form.observacoes = '';
  }
};
</script>

<style scoped>
* {
  box-sizing: border-box;
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}

.app-container {
  display: flex;
  min-height: 100vh;
  background-color: #f8f9fa;
}

.sidebar {
  width: 84px;
  background-color: #071126;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 28px;
  flex-shrink: 0;
}

.logo-container {
  margin-bottom: 48px;
}

.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 28px;
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
  padding: 36px 48px;
  max-width: 1400px;
}

.page-title {
  font-size: 26px;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 28px 0;
}

.form-section {
  margin-bottom: 28px;
}

.section-header {
  display: flex;
  align-items: center;
  margin-bottom: 18px;
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
  margin-top: 16px;
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
  margin-bottom: 6px;
}

input,
select,
textarea {
  width: 100%;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  padding: 9px 12px;
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
  top: 11px;
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
  padding: 9px 16px;
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
  min-height: 150px;
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
  padding: 12px 16px;
  font-weight: 600;
  text-align: left;
}

.products-table td {
  padding: 16px;
  color: #334155;
  border-bottom: 1px solid #f1f5f9;
}

.text-center {
  text-align: center !important;
}

.actions-cell {
  display: flex;
  justify-content: center;
  gap: 10px;
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
  padding: 32px !important;
}

.form-footer {
  display: flex;
  justify-content: flex-end;
  gap: 14px;
  margin-top: 24px;
}

.btn-cancel {
  background-color: #ffffff;
  color: #1e3a8a;
  border: 1px solid #1e3a8a;
  border-radius: 6px;
  padding: 9px 24px;
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
  padding: 9px 26px;
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
    padding: 24px;
  }
}
</style>