<script setup lang="ts">
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import { useManagerStore } from './stores/manager';
import type { User } from './models/user';
import axios from 'axios';

const store = useManagerStore();

//#region Data
const apiUrl = import.meta.env.VITE_APP_API_URL;
const headers: any[] = [
  {
    title: 'Name',
    align: 'start',
    key: 'fullName',
  },
  { title: 'Email', key: 'email' },
  { title: 'Credits', key: 'credits' },
  { title: 'Actions', key: 'actions', sortable: false }
]
const rules = {
  required: (value: any) => !!value || 'Field is required',
  email: (value: string) => /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(value) || 'Incorrect format'
}
//#endregion

//#region Reactive
const dialog = ref(false)
const dialogDelete = ref(false)
const url = ref("");
const response = ref("")
//#endregion

//#region Computed
const users = computed(() => store.users)
const formTitle = computed(() => store.user?.id ? 'Edit User' : 'New User')
//#endregion

//#region Watchers
watch(dialog, (val) => val || close())

watch(dialogDelete, (val) => val || closeDelete())
//#endregion

//#region Functions
function deleteUser(user: User) {
  store.selectUser(user.id!)
  dialogDelete.value = true
}

async function deleteUserConfirm() {
  await store.deleteUser()
  closeDelete()
}

async function editUser(user: User) {
  store.selectUser(user.id!)
  dialog.value = true
}

async function close() {
  dialog.value = false
  await nextTick()
  store.resetUser()
}

async function closeDelete() {
  dialogDelete.value = false
  await nextTick()
  store.resetUser()
}

function save() {
  if (store.user?.id) {
    store.editUser()
  } else {
    store.addUser()
  }
  close()
}

async function connect() {
  try {
    response.value = await axios.get(url.value)
  } catch (error) {
    response.value = error as string
  }
}
//#endregion

//#region Lifecycle
onMounted(() => {
  store.fetchUsers();
})
//#endregion
</script>

<template>
  <main>
    <v-data-table class="userTable" :items="users" :headers="headers">
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Users</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-dialog v-model="dialog" max-width="700px">
            <template v-slot:activator="{ props }">
              <v-btn color="primary" dark class="mb-2" v-bind="props">
                New User
              </v-btn>
            </template>
            <v-card>
              <v-form @submit.prevent="save" ref="form">
                <v-card-title>
                  <span class="text-h5">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="store.user!.fullName" label="Name"
                          :rules="[rules.required]"></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6" md="4">
                        <v-text-field type="email" v-model="store.user!.email" label="Email"
                          :rules="[rules.required, rules.email]"></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6" md="4">
                        <v-text-field type="number" v-model="store.user!.credits" label="Credits"
                          :rules="[rules.required]"></v-text-field>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue-darken-1" variant="text" @click="close">
                    Cancel
                  </v-btn>
                  <v-btn :disabled="!$refs.form || (!($refs.form as any).isValid)" color="blue-darken-1" type="submit"
                    variant="text">
                    Save
                  </v-btn>
                </v-card-actions>
              </v-form>
            </v-card>
          </v-dialog>
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="text-h5">Are you sure you want to delete this user?</v-card-title>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
                <v-btn color="blue-darken-1" variant="text" @click="deleteUserConfirm">OK</v-btn>
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon size="small" class="me-2" @click="editUser(item)">
          mdi-pencil
        </v-icon>
        <v-icon size="small" @click="deleteUser(item)">
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </main>
</template>

<style scoped>
main {
  min-height: 100vh;
  display: flex;
  justify-content: center;
}

.userTable {
  max-width: 900px;
}
</style>
