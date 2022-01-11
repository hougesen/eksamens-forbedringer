<template>
    <div class="w-full p-8 flex" style="place-content: center; place-items: center; place-self: center">
        <div class="w-full max-w-md flex flex-col">
            <img src="@/assets/aalogo.png" alt="Alex Andersen logo" class="w-full h-auto text-center" />

            <Title text="Alex Andersen Ølund" size="large" class="text-center mb-4" />

            <form class="flex flex-col gap-4 w-full" @submit.prevent="login">
                <InputField label="Email" input-type="text" label-size="small" @onChange="setEmail" />

                <InputField label="Password" input-type="password" label-size="small" @onChange="setPassword" />

                <div>
                    <Button text="Login" type="submit" />
                </div>
            </form>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    name: 'Login',

    components: {
        Title: () => import('@/components/Title.vue'),
        Button: () => import('@/components/Button.vue'),
        InputField: () => import('@/components/InputField.vue'),
    },

    data() {
        return {
            email: '' as string,
            password: '' as string,
        };
    },

    methods: {
        setEmail(value: string): void {
            console.log(value);
            this.email = value;
        },

        setPassword(value: string): void {
            console.log(value);
            this.password = value;
        },

        login(): void {
            console.log('login');
        },
    },
});
</script>
