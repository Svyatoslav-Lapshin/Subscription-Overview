import axiosClient from "./axiosClient";

export async function registerUser(data) {
  const response = await axiosClient.post("/api/auth/register", data);

  return response.data;
}
export async function loginUser(data) {
  const response = await axiosClient.post("/api/auth/login", data);
  return response.data;
}
export async function refreshToken() {
  const response = await axiosClient.post("/api/auth/refresh");
  return response.data;
}

export async function logoutUser() {
  await axiosClient.post("/api/auth/logout");
}
