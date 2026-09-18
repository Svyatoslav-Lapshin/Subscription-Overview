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

export async function getSubscriptionById(id) {
  const response = await axiosClient.get(`/api/subscriptions/${id}`);
  return response.data;
}

export async function editSubscription(id, updatedSubscription) {
  const response = await axiosClient.put(
    `/api/subscriptions/${id}`,
    updatedSubscription,
  );
  return response.data;
}

export async function deleteSubscription(id) {
  await axiosClient.delete(`/api/subscriptions/${id}`);
}
