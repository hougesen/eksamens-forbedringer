import Vue from 'vue';
import VueSweetalert2 from 'vue-sweetalert2';
import App from './App.vue';
import './registerServiceWorker';
import router from './router';
import store from './store';
import './style/index.css';
import 'sweetalert2/dist/sweetalert2.min.css';

Vue.config.productionTip = false;

Vue.use(VueSweetalert2);

new Vue({
    router,
    store,
    render: (h) => h(App),
}).$mount('#app');
