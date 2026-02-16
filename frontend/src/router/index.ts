import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import AdminView from '../views/AdminView.vue';
import Entidad1ListView from '../views/Entidad1ListView.vue';
import Entidad1DetailView from '../views/Entidad1DetailView.vue';
import Entidad2ListView from '../views/Entidad2ListView.vue';
import Entidad2DetailView from '../views/Entidad2DetailView.vue';
import { useAuthStore } from '@/stores/auth.store';

const routes: RouteRecordRaw[] = [
  { path: '/', name: 'home', component: HomeView },
  { path: '/login', name: 'login', component: LoginView },
  { path: '/admin', name: 'admin', component: AdminView, meta: { requiresAuth: true, requiresAdmin: true } },
  { path: '/entidad1', name: 'entidad1-list', component: Entidad1ListView, meta: { requiresAuth: true } },
  { path: '/entidad1/:id', name: 'entidad1-detail', component: Entidad1DetailView, meta: { requiresAuth: true } },
  { path: '/entidad2', name: 'entidad2-list', component: Entidad2ListView, meta: { requiresAuth: true } },
  { path: '/entidad2/:id', name: 'entidad2-detail', component: Entidad2DetailView, meta: { requiresAuth: true } },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
   const auth = useAuthStore();
   if (to.meta.requiresAuth && !auth.isAuthenticated)
    { return next({ name: 'login' }); } 
   if (to.meta.requiresAdmin && !auth.isAdmin) 
    { return next({ name: 'home' }); } 
   next(); });

export default router;
