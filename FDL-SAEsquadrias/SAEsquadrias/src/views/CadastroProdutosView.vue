<template>
  <div class="app-container">
    <aside class="sidebar">
      <div class="logo-container">
        <img src="@/assets/logoSA.png" alt="SA Esquadrias" class="logo-img" />
      </div>

      <nav class="sidebar-nav">
        <button type="button" class="nav-item" title="Início" @click="irPara('/')">
          <Home :size="20" />
        </button>

        <button type="button" class="nav-item" title="Agenda">
          <Calendar :size="20" />
        </button>

        <button type="button" class="nav-item active" title="Obras e Produtos">
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
      <header class="page-header">
        <h1 class="page-title">Cadastro de Produtos</h1>
        <p class="page-subtitle">Preencha as informações do produto para cadastrá-lo no sistema</p>
      </header>

      <form @submit.prevent="cadastrarProduto" class="form-wrapper">
        <div class="form-scroll-area">
          <div class="form-group">
            <label for="nome-produto">Nome do produto</label>
            <input
              id="nome-produto"
              v-model="form.nome"
              type="text"
              placeholder="Ex: Janela de alumínio"
              required
            />
          </div>

          <div class="form-group mt-4">
            <label for="descricao-produto">Descrição</label>
            <textarea
              id="descricao-produto"
              v-model="form.descricao"
              rows="4"
              placeholder="Descreva as características do produto..."
            ></textarea>
          </div>

          <section class="measures-section mt-4">
            <h2 class="section-label">Medidas</h2>

            <div class="grid-row-2">
              <div class="form-group">
                <label for="largura">Largura</label>
                <input id="largura" v-model="form.largura" type="text" placeholder="Ex: 100 cm" />
              </div>

              <div class="form-group">
                <label for="profundidade">Profundidade</label>
                <input
                  id="profundidade"
                  v-model="form.profundidade"
                  type="text"
                  placeholder="Ex: 5 cm"
                />
              </div>
            </div>

            <div class="grid-row-2 mt-3">
              <div class="form-group">
                <label for="altura">Altura</label>
                <input id="altura" v-model="form.altura" type="text" placeholder="Ex: 120 cm" />
              </div>

              <div class="form-group">
                <label for="cor">Cor</label>
                <input id="cor" v-model="form.cor" type="text" placeholder="Ex: Branco" />
              </div>
            </div>
          </section>

          <div class="form-group mt-4">
            <label for="quantidade">Quantidade necessária</label>
            <div class="input-with-suffix">
              <input
                id="quantidade"
                v-model.number="form.quantidadeNecessaria"
                type="number"
                min="1"
                placeholder="Ex: 25"
                required
              />
              <span class="input-suffix">unidades</span>
            </div>
            <span class="helper-text">
              informe a quantidade de unidades necessárias deste produto.
            </span>
          </div>
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
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Home, Calendar, Wrench, ClipboardList, Users, FileText } from 'lucide-vue-next'

const STORAGE_PRODUTOS_KEY = 'sa_esquadrias_catalogo_produtos'
// Caso crie o controller no backend, descomente e use a URL abaixo:
// const API_URL = 'http://localhost:5006/api/Produtos'

const router = useRouter()
const salvando = ref(false)

const form = reactive({
  nome: '',
  descricao: '',
  largura: '',
  profundidade: '',
  altura: '',
  cor: '',
  quantidadeNecessaria: null as number | null,
})

const irPara = (rota: string) => {
  router.push(rota)
}

const montarMedidaFormatada = () => {
  const partes = [form.largura.trim(), form.altura.trim(), form.profundidade.trim()].filter(Boolean)
  return partes.length > 0 ? partes.join(' × ') : '-'
}

const montarObservacaoFormatada = () => {
  const partes = [form.cor.trim() ? `Cor: ${form.cor.trim()}` : '', form.descricao.trim()].filter(
    Boolean,
  )
  return partes.join(' - ')
}

const limparFormulario = () => {
  form.nome = ''
  form.descricao = ''
  form.largura = ''
  form.profundidade = ''
  form.altura = ''
  form.cor = ''
  form.quantidadeNecessaria = null
}

const cadastrarProduto = async () => {
  salvando.value = true
  try {
    const novoItemCatalogo = {
      id: Date.now(),
      nome: form.nome.trim(),
      descricao: form.descricao.trim(),
      largura: form.largura.trim(),
      profundidade: form.profundidade.trim(),
      altura: form.altura.trim(),
      cor: form.cor.trim(),
      quantidadeNecessaria: Number(form.quantidadeNecessaria),
      medidasPadrao: montarMedidaFormatada(),
      observacoesPadrao: montarObservacaoFormatada(),
    }

    const existentesRaw = localStorage.getItem(STORAGE_PRODUTOS_KEY)
    const listaAtual = existentesRaw ? JSON.parse(existentesRaw) : []
    listaAtual.push(novoItemCatalogo)
    localStorage.setItem(STORAGE_PRODUTOS_KEY, JSON.stringify(listaAtual))

    alert('Produto cadastrado com sucesso!')
    limparFormulario()

    // Se veio da tela de obras, volta para lá automaticamente
    if (window.history.length > 1) {
      router.back()
    }
  } catch (error: any) {
    console.error('Erro ao cadastrar produto:', error)
    alert(error.message || 'Erro ao cadastrar produto.')
  } finally {
    salvando.value = false
  }
}

const cancelar = () => {
  if (window.history.length > 1) {
    router.back()
  } else {
    limparFormulario()
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
  padding: 32px 56px 0 56px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.page-header {
  margin-bottom: 24px;
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
  padding-bottom: 20px;
}

.measures-section {
  display: flex;
  flex-direction: column;
}

.section-label {
  font-size: 15px;
  font-weight: 700;
  color: #0f172a;
  margin: 0 0 12px 0;
}

.grid-row-2 {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 32px;
}

.mt-3 {
  margin-top: 14px;
}

.mt-4 {
  margin-top: 18px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group > label {
  font-size: 14px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 6px;
}

.measures-section .form-group > label {
  font-size: 13px;
  font-weight: 700;
}

input,
textarea {
  width: 100%;
  border: 1px solid #cbd5e1;
  border-radius: 6px;
  padding: 10px 14px;
  font-size: 13px;
  color: #1e293b;
  background-color: #ffffff;
  outline: none;
  transition:
    border-color 0.2s,
    box-shadow 0.2s;
}

textarea {
  resize: vertical;
  min-height: 96px;
}

input::placeholder,
textarea::placeholder {
  color: #94a3b8;
}

input:focus,
textarea:focus {
  border-color: #0284c7;
  box-shadow: 0 0 0 2px rgba(2, 132, 199, 0.2);
}

.input-with-suffix {
  position: relative;
  display: flex;
  align-items: center;
}

.input-with-suffix input {
  padding-right: 84px;
}

.input-suffix {
  position: absolute;
  right: 14px;
  font-size: 13px;
  color: #64748b;
  pointer-events: none;
}

.helper-text {
  font-size: 11px;
  color: #64748b;
  margin-top: 5px;
}

.form-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 16px 0 24px 0;
  background-color: #f8f9fa;
  flex-shrink: 0;
}

.btn-cancel {
  background-color: #787878;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 9px 28px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-cancel:hover {
  background-color: #646464;
}

.btn-submit {
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 9px 28px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-submit:hover {
  background-color: #172554;
}

.btn-submit:disabled,
.btn-cancel:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

@media (max-width: 900px) {
  .grid-row-2 {
    grid-template-columns: 1fr;
    gap: 14px;
  }

  .main-content {
    padding: 24px 24px 0 24px;
  }
}
</style>
