import type { User } from '@/models/user'
import axios from 'axios'
import { defineStore } from 'pinia'

const apiUrl = import.meta.env.VITE_APP_API_URL

export const useManagerStore = defineStore('manager', {
  state: () => ({
    users: [] as User[],
    user: {} as User | undefined
  }),
  getters: {
    getUser(state) {
      return state.user
    },
    getUsers(state) {
      return state.users
    }
  },
  actions: {
    selectUser(id: string) {
      this.user = { ...this.users.find((u) => u.id === id) } as User
    },
    resetUser() {
      this.user = {} as User
    },
    async fetchUsers() {
      try {
        const data = await axios.get(`${apiUrl}/users`)
        this.users = data.data
      } catch (error) {
        console.log(error)
        alert(error)
      }
    },
    async editUser() {
      try {
        const response = await axios.put(`${apiUrl}/users/${this.user!.id}`, this.user)
        console.log('Response', response)
        await this.fetchUsers()
      } catch (error) {
        console.log('Error', error)
        alert(error)
      }
    },
    async deleteUser() {
      try {
        await axios.delete(`${apiUrl}/users/${this.user!.id}`)
        await this.fetchUsers()
      } catch (error) {
        console.log(error)
        alert(error)
      }
    },
    async addUser() {
      try {
        const response = await axios.post(`${apiUrl}/users`, this.user)
        console.log('Response', response)
        await this.fetchUsers()
      } catch (error) {
        console.log(error)
        alert(error)
      }
    }
  }
})
