import axiosClient from "./axiosClient";

export async function getCategories() {
  const response = await axiosClient.get("/api/categories");
  return response.data;
}

export async function createCategory(newCategory) {
  const response = await axiosClient.post("/api/categories", newCategory);
  return response.data;
}
