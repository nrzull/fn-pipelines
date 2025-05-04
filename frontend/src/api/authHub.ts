import * as signalR from "@microsoft/signalr";

export const authHub = new signalR.HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_API_WEB_PUBLIC_URL}/authHub`)
  .withAutomaticReconnect()
  .build();
