import axiosClient from "./axiosClient";

export async function getProviders() {
  const response = await axiosClient.get("/api/providers");
  return response.data;
}
export async function createProvider(newProvider) {
  const response = await axiosClient.post("/api/providers", newProvider);
  return response.data;
}
