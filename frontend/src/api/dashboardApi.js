import axiosClient from "./axiosClient";

export async function getDashboardSummary() {
  const response = await axiosClient.get("/api/subscriptions/summary");
  return response.data;
}

