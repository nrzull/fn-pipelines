import "./main.css";
import { createApp } from "vue";
import App from "./App.vue";
import { authHub } from "./api/authHub";

await authHub.start();

const app = createApp(App);

app.mount("#app");
