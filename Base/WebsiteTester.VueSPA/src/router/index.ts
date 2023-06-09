import { createRouter, createWebHistory } from 'vue-router'
import WebsiteTester from '../views/WebsiteTesterView.vue'
import TestDetailsViewVue from '@/views/TestDetailsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: WebsiteTester
    },
    {
      path: '/details/:id',
      name: 'TestDetails',
      component: TestDetailsViewVue,      
      props: true
    }
  ]
})

export default router
