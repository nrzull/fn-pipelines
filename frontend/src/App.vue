<template>
  <template v-if="!codeFn">
    <input type="text" placeholder="phone" v-model="phone" />
    <button @click="submitPhone()">submit phone</button>
  </template>
  <template v-else>
    <input type="text" placeholder="code" v-model="code" />
    <button @click="code ? codeFn(code!) : null">submit code</button>
  </template>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { authHub } from "./api/authHub";

const phone = ref<string>();
const code = ref<string>();
const codeFn = ref<(_: string) => void>();
const codeHandle = ref<Promise<string>>();

authHub.on("Login", async () => {
  codeHandle.value = new Promise((resolve) => {
    codeFn.value = resolve;
  });

  const resultCode = await codeHandle.value;
  codeFn.value = undefined;
  code.value = "";

  return resultCode;
});

const submitPhone = async () => {
  if (!phone.value) {
    return;
  }

  await authHub.send("Login", phone.value);
  phone.value = "";
};
</script>
