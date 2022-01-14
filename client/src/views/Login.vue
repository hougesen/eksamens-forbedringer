<template>
    <div class="w-full p-8 flex place-content-center place-items-center place-self-center">
        <div class="w-full max-w-md flex flex-col">
            <img src="@/assets/aalogo.png" alt="Alex Andersen logo" class="w-full h-auto text-center" />

            <Title text="Alex Andersen Ølund" size="large" class="text-center mb-4" />

            <form class="flex flex-col gap-4 w-full" autocomplete="off" @submit.prevent="login">
                <InputField
                    label="Email"
                    input-type="text"
                    label-size="small"
                    :required="true"
                    autocomplete="off"
                    @onChange="setEmail"
                />

                <InputField
                    label="Password"
                    input-type="password"
                    label-size="small"
                    :required="true"
                    autocomplete="off"
                    @onChange="setPassword"
                />

                <div>
                    <Button text="Login" type="submit" />
                </div>
            </form>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import { mapActions } from 'vuex';

export default Vue.extend({
    name: 'Login',

    components: {
        Button: () => import('@/components/Button.vue'),
        InputField: () => import('@/components/InputField.vue'),
        Title: () => import('@/components/Title.vue'),
    },

    data() {
        return {
            email: 'admin@admin.dk' as string,
            password: 'admin' as string,
        };
    },

    methods: {
        ...mapActions(['handleLogin']),

        setEmail(value: string): void {
            console.log(value);
            this.email = value;
        },

        setPassword(value: string): void {
            console.log(value);
            this.password = value;
        },

        async login(): Promise<void> {
            const HOST = process.env.VUE_APP_API_URL;
            const URL = `${HOST}/api/login`;

            if (this.email?.length && this.password?.length) {
                const result: User | null = await axios
                    .post(URL, {
                        email: this.email?.toLowerCase(),
                        password: this.password,
                    })
                    .then((res) => res?.data)
                    .catch((error) => {
                        console.error('Error logging in', error);
                        return null;
                    });

                if (result) {
                    this.handleLogin(result);
                    this.$router.push('/');
                } else {
                    this.$swal({
                        icon: 'error',
                        title: `Wrong email/password`,
                        showConfirmButton: false,
                        timer: 1000,
                    });
                }
            }
        },
    },
});
</script>
