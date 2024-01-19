import './assets/main.css'
import '@mdi/font/css/materialdesignicons.css'

import { createPinia } from 'pinia'
import { createApp } from 'vue'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import App from './App.vue'

const vuetify = createVuetify({
  components,
  directives,
  icons: {
    defaultSet: 'mdi'
  },
  theme: {
    defaultTheme: 'dark'
  }
})

const app = createApp(App)

app.use(vuetify)

app.use(createPinia())

app.mount('#app')
