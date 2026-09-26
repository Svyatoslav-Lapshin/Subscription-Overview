import { tokenStore } from "@/lib/tokenStore";
import axios from "axios";

//Create a reusable Axios instance with shared configurations
const axiosClient = axios.create({
  //Set the base URL dynamically from VITE env variable
  baseURL: import.meta.env.VITE_API_BASE_URL,
  //Automatically include cookies with every request
  withCredentials: true,
});

/*Add access token to request*/
axiosClient.interceptors.request.use((config) => {
  const token = tokenStore.get();

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default axiosClient;
