<template>
  <div v-if="isOpen" class="modal-backdrop" @click.self="fechar">
    <div class="modal-card">
      <button type="button" class="close-btn" @click="fechar" title="Fechar">
        <X :size="24" />
      </button>

      <h2 class="modal-title">Cadastro de Equipes</h2>

      <form @submit.prevent="salvarEquipe" class="modal-form">
        <div class="form-group">
          <label>Nome da Equipe*</label>
          <input v-model="form.nome" type="text" placeholder="Ex. Equipe Alfa" required />
        </div>

        <div class="form-group">
          <label>Responsável pela Equipe*</label>
          <input
            v-model="form.responsavel"
            type="text"
            placeholder="Ex. Geovanna Guerra"
            required
          />
        </div>

        <div class="form-group">
          <label>Quantidade de integrantes *</label>
          <input
            v-model.number="form.quantidadeIntegrantes"
            type="number"
            min="1"
            placeholder="Ex. 5"
            required
          />
        </div>

        <div class="modal-footer">
          <button type="button" class="btn-cancel" @click="fechar" :disabled="salvando">
            Cancelar
          </button>
          <button type="submit" class="btn-submit" :disabled="salvando">
            <Plus :size="16" />
            <span>{{ salvando ? 'Cadastrando...' : 'Cadastrar Equipe' }}</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { X, Plus } from 'lucide-vue-next'

const props = defineProps<{
  isOpen: boolean
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'saved', equipe: any): void
}>()

const API_URL = 'http://localhost:5006/api'
const salvando = ref(false)

const form = reactive({
  nome: '',
  responsavel: '',
  quantidadeIntegrantes: null as number | null,
})

const limparFormulario = () => {
  form.nome = ''
  form.responsavel = ''
  form.quantidadeIntegrantes = null
}

const fechar = () => {
  if (!salvando.value) {
    limparFormulario()
    emit('close')
  }
}

const salvarEquipe = async () => {
  salvando.value = true
  try {
    const response = await fetch(`${API_URL}/Equipe`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        nome: form.nome,
        responsavel: form.responsavel,
        quantidadeIntegrantes: Number(form.quantidadeIntegrantes),
      }),
    })

    if (!response.ok) {
      const erroTexto = await response.text()
      throw new Error(erroTexto || `Erro HTTP: ${response.status}`)
    }

    const equipeCriada = await response.json()
    emit('saved', equipeCriada)
    limparFormulario()
    emit('close')
  } catch (error: any) {
    console.error('Erro ao cadastrar equipe:', error)
    alert(error.message || 'Erro ao cadastrar equipe na API.')
  } finally {
    salvando.value = false
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

.modal-backdrop {
  position: fixed;
  inset: 0;
  background-color: rgba(15, 23, 42, 0.75);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 16px;
}

.modal-card {
  position: relative;
  width: 100%;
  max-width: 480px;
  background-color: #ffffff;
  border-radius: 16px;
  padding: 44px 48px 40px 48px;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.25);
}

.close-btn {
  position: absolute;
  top: 20px;
  right: 22px;
  background: none;
  border: none;
  color: #cbd5e1;
  cursor: pointer;
  padding: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s;
}

.close-btn:hover {
  color: #64748b;
}

.modal-title {
  font-size: 26px;
  font-weight: 700;
  color: #000000;
  text-align: center;
  margin: 0 0 36px 0;
}

.modal-form {
  display: flex;
  flex-direction: column;
  gap: 22px;
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

input {
  width: 100%;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  padding: 10px 12px;
  font-size: 13px;
  color: #1e293b;
  background-color: #ffffff;
  outline: none;
  transition: border-color 0.2s;
}

input::placeholder {
  color: #cbd5e1;
}

input:focus {
  border-color: #1e3a8a;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 28px;
}

.btn-cancel {
  background-color: #ffffff;
  color: #1e3a8a;
  border: 1px solid #1e3a8a;
  border-radius: 6px;
  padding: 9px 22px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-cancel:hover {
  background-color: #f8fafc;
}

.btn-submit {
  display: flex;
  align-items: center;
  gap: 8px;
  background-color: #1e3a8a;
  color: #ffffff;
  border: none;
  border-radius: 6px;
  padding: 9px 20px;
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
</style>
