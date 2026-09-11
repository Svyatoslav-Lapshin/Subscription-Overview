import { useAuth } from "@/context/AuthContext";
import { Button } from "@/components/ui/button";

export default function DashboardPage() {
  const { logout } = useAuth();

  const handleLogout = async () => {
    await logout();
  };

  return (
    <div>
      <h1>Dashboard</h1>
      <Button onClick={handleLogout}>LogOut</Button>
    </div>
  );
}
