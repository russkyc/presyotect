<script setup lang="ts">

import {ref} from 'vue';
import {useToast} from "primevue/usetoast";
import {Button, InputText, Message, Password} from "primevue";
import {Form} from '@primevue/forms';
import {login} from "@features/auth/auth-service.ts";
import {zodResolver} from '@primevue/forms/resolvers/zod';
import router from '@/router';
import {z} from 'zod';

const toast = useToast();

const initialValues = ref({
  username: '',
  password: ''
});

const resolver = ref(zodResolver(
    z.object({
      username: z.string().min(1, {message: 'Username is required.'}),
      password: z.string().min(1, {message: 'Password is required.'})
    })
));

const onFormSubmit = async (form: any) => {
  if (!form.valid) {
    toast.add({
      severity: 'error',
      summary: 'Login Failed',
      detail: 'Please fill-out all required fields.',
      life: 2000
    });
    return;
  }
  await authenticate(form);
};

const authenticate = async (form: any) => {
  initialValues.value = {
    username: '',
    password: ''
  };
  const authState = await login(form.values.username, form.values.password);
  if (!authState.isAuthenticated){
    toast.add({
      severity: 'error',
      summary: 'Login Failed',
      detail: authState.data,
      life: 2000
    });
    return;
  }
  toast.add({
    severity: 'success',
    summary: 'Login Successful',
    detail: `Welcome back ${authState.data.username}!`,
    life: 2000
  });
  await router.push('/');
}
</script>

<template>
  <div class="flex grow bg-[--p-primary-contrast-color] text-[--p-text-color]">
    <div class="relative flex flex-col w-full lg:border-r lg:w-2/5 bg-[--p-surface-0] text-[--p-text-color]">
      <img alt="" src="/branding/logo-horizontal.svg" class="absolute top-8 left-8 h-6 block max-md:hidden"/>
      <div class="max-md:m-6 max-md:my-auto m-auto md:w-80 flex flex-col gap-12">
        <div class="flex flex-col grow">
          <img alt="" src="/branding/logo-horizontal.svg" class="mr-auto mb-2 h-5 max-md:block hidden"/>
          <h1 class="font-medium text-2xl md:text-3xl">Welcome Back</h1>
          <h2 class="">Sign into your account</h2>
        </div>
        <Form v-slot="$form" :resolver="resolver" :initialValues="initialValues" @submit="onFormSubmit"
              class="flex flex-col gap-8 w-full">
          <div class="flex flex-col gap-4">
            <div class="flex flex-col gap-1">
              <label for="username">Username</label>
              <InputText variant="filled" name="username" type="text" fluid/>
              <Message v-if="$form.username?.invalid" severity="error" size="small" variant="simple">
                {{ $form.username.error?.message }}
              </Message>
            </div>
            <div class="flex flex-col gap-1">
              <label for="password">Password</label>
              <Password variant="filled" name="password" placeholder="Password" :feedback="false" fluid toggleMask/>
              <Message v-if="$form.password?.invalid" severity="error" size="small" variant="simple">
                {{ $form.password.error?.message }}
              </Message>
            </div>
          </div>
          <Button type="submit" label="Submit">Login</Button>
        </Form>
      </div>
      <div class="flex p-6 absolute bottom-0 w-full justify-center">
        <p class="text-zinc-400">Presyotect VNext 0.0.1</p>
      </div>
    </div>
    <div
        class="grow bg-[url('/branding/login-wallpaper.jpg')] hidden max-md:collapse lg:block bg-cover bg-center">
    </div>
  </div>
</template>

<style scoped>

</style>