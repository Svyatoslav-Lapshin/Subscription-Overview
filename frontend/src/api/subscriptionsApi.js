import axiosClient from "./axiosClient";
export async function getUserSubscriptions() {
  const response = await axiosClient.get("/api/subscriptions");
  return response.data;
}
