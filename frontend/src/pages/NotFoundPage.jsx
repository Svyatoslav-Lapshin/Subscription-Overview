import { Button } from "@/components/ui/button";
import { useAuth } from "@/context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function NotFoundPage() {
  const navigate = useNavigate();
  const { accessToken } = useAuth();
  return (
    <div className="flex min-h-svh flex-col items-center justify-center px-4 text-center">
      <p className="text-7xl font-bold text-primary">404</p>

      <h1 className="mt-4 text-2xl font-semibold text-foreground">
        Page not found
      </h1>

      <p className="mt-2 max-w-md text-m text-muted-foreground">
        The page you are looking for does not exist or may have been moved.
      </p>

      <Button
        type="button"
        onClick={() => navigate(accessToken ? "/dashboard" : "/")}
        className="mt-6 h-10 px-10"
      >
        {accessToken ? "Back to dashboard" : "Back to home"}
      </Button>
    </div>
  );
}
