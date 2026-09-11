import { useAuth } from "@/context/AuthContext";
import { Navigate } from "react-router-dom";

/*Its main task is to check whether the user has permission to view the page and, if not, to redirect them to another page (for example, the login page).*/
export default function ProtectedRoute({ children }) {
  const { accessToken, isAuthLoading } = useAuth();

  /*Wait for auth check*/
  if (isAuthLoading) {
    return <p>Loading...</p>;
  }
  /*Redirect unathenticated user*/
  if (!accessToken) {
    return <Navigate to="/login" replace />;
  }

  return children;
}
