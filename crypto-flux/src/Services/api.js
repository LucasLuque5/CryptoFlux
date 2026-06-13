import axios from 'axios'

const apiClient = axios.create({
  baseURL: 'https://localhost:7212',
  headers: {
    'Content-Type': 'application/json'
  }
})

export const transaccionesApi = {
  getAll() {
    return apiClient.get('/transacciones')
  },
  
  create(data) {
    return apiClient.post('/transacciones', data)
  },

  delete(id) {
  return apiClient.delete(`/transacciones/${id}`)
  },

  getById(id) {
    return apiClient.get(`/transacciones/${id}`)
  },

  update(id, data) {
  return apiClient.put(`/transacciones/${id}`, data)
  },
  
}


