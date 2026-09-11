import { refreshToken, logoutUser } from "@/api/authApi";
import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  /*Current access token*/
  const [accessToken, setAccessToken] = useState(null);
  /*Auth check loading*/
  const [isAuthLoading, setIsAuthLoading] = useState(true);
  /*Logout current user*/
  const logout = async () => {
    await logoutUser();
    setAccessToken(null);
  };

  useEffect(() => {
    /*Restore user session*/
    const restoreSession = async () => {
      try {
        const response = await refreshToken();
        setAccessToken(response.accessToken);
      } catch {
        setAccessToken(null);
      } finally {
        setIsAuthLoading(false);
      }
    };
    restoreSession();
  }, []);

  return (
    /*Share auth data*/
    <AuthContext.Provider
      value={{ accessToken, setAccessToken, isAuthLoading, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};
/*Provides auth data to child components*/
export const useAuth = () => useContext(AuthContext);
