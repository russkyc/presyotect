<script lang="ts" setup>

import {Form, type FormSubmitEvent} from "@primevue/forms";
import {zodResolver} from "@primevue/forms/resolvers/zod";
import {Button, InputText, Message, Password} from "primevue";
import {useToast} from "primevue/usetoast";
import {ref} from "vue";
import {z} from "zod";
import router from "@/router";
import {login} from "@/services/auth/auth-service.ts";

const toast = useToast();
const version = import.meta.env.VITE_APP_VERSION;

const initialValues = ref({
    username: "",
    password: ""
});

const resolver = ref(zodResolver(
    z.object({
        username: z.string().min(1, {message: "Username is required."}),
        password: z.string().min(1, {message: "Password is required."})
    })
));

const onFormSubmit = async (form: FormSubmitEvent) => {
    if (!form.valid) {
        toast.add({
            severity: "error",
            summary: "Login Failed",
            detail: "Please fill-out all required fields.",
            life: 2000
        });
        return;
    }
    await authenticate(form);
};

const authenticate = async (form: FormSubmitEvent) => {
    initialValues.value = {
        username: "",
        password: ""
    };
    const authState = await login(form.values.username, form.values.password);
    if (!authState.isAuthenticated) {
        toast.add({
            severity: "error",
            summary: "Login Failed",
            detail: authState.data,
            life: 2000
        });
        return;
    }
    if (!authState.data || typeof authState.data === "string") {
        toast.add({
            severity: "error",
            summary: "Login Failed",
            detail: authState.data,
            life: 2000
        });
        return;
    }
    toast.add({
        severity: "success",
        summary: "Login Successful",
        detail: `Welcome back ${authState.data.username}!`,
        life: 2000
    });
    await router.push("/");
}
</script>

<template>
  <div class="flex grow bg-surface-100 dark:bg-surface-950 text-surface-700 dark:text-surface-0">
    <div class="relative flex flex-col w-full border-surface-200 dark:border-surface-700 lg:border-r lg:w-2/5 bg-white dark:bg-surface-950 text-surface-700 dark:text-surface-0">
      <img
        alt=""
        class="absolute top-8 left-8 h-6 block max-md:hidden"
        src="/branding/logo-horizontal.svg"
      >
      <div class="max-md:m-6 max-md:my-auto m-auto md:w-80 flex flex-col gap-12">
        <div class="flex flex-col grow">
          <img
            alt=""
            class="mr-auto mb-2 h-5 max-md:block hidden"
            src="/branding/logo-horizontal.svg"
          >
          <h1 class="font-medium text-2xl md:text-3xl">
            Welcome Back
          </h1>
          <h2 class="">
            Sign into your account
          </h2>
        </div>
        <Form
          v-slot="$form"
          :initial-values="initialValues"
          :resolver="resolver"
          class="flex flex-col gap-8 w-full"
          @submit="onFormSubmit"
        >
          <div class="flex flex-col gap-4">
            <div class="flex flex-col gap-1">
              <label for="username">Username</label>
              <InputText
                fluid
                name="username"
                type="text"
                variant="filled"
              />
              <Message
                v-if="$form.username?.invalid"
                severity="error"
                size="small"
                variant="simple"
              >
                {{ $form.username.error?.message }}
              </Message>
            </div>
            <div class="flex flex-col gap-1">
              <label for="password">Password</label>
              <Password
                :feedback="false"
                fluid
                name="password"
                placeholder="Password"
                toggle-mask
                variant="filled"
              />
              <Message
                v-if="$form.password?.invalid"
                severity="error"
                size="small"
                variant="simple"
              >
                {{ $form.password.error?.message }}
              </Message>
            </div>
          </div>
          <Button
            label="Submit"
            type="submit"
          >
            Login
          </Button>
        </Form>
      </div>
      <div class="flex p-6 absolute bottom-0 w-full justify-center">
        <p class="text-zinc-400">
          Presyotect Next v{{ version }}
        </p>
      </div>
    </div>
    <div
      class="grow bg-[url('/branding/login-wallpaper.jpg')] hidden max-md:collapse lg:block bg-cover bg-center"
    />
  </div>
</template>

<style scoped>

</style>