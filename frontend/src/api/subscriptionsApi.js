import axiosClient from "./axiosClient";
export async function getUserSubscriptions() {
  const response = await axiosClient.get("/api/subscriptions");
  return response.data;
}

export async function createSubscription(newSubscription) {
  const response = await axiosClient.post(
    "/api/subscriptions",
    newSubscription,
  );
  return response.data;
}
