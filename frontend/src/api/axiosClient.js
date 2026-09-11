import axios from "axios";

//Create a reusable Axios instance with shared configurations
const axiosClient = axios.create({
  //Set the base URL dynamically from VITE env variable
  baseURL: import.meta.env.VITE_API_BASE_URL,
  //Automatically include HTTP cookies/auth tokens with every request
  withCredentials: true,
});

export default axiosClient;
