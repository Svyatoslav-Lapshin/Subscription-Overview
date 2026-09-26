import { ReceiptText, LayoutDashboard, LogOut } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/context/AuthContext";

export default function MobileBottomNav() {
  const location = useLocation();
  const navigate = useNavigate();
  const { logout } = useAuth();

  const handleLogOut = async () => {
    await logout();
    navigate("/login", { replace: true });
  };

  const isDashboard = location.pathname === "/dashboard";
  const isSubscriptions = location.pathname.startsWith("/subscriptions");

  return (
    <nav className="fixed inset-x-0 bottom-0 z-50 flex h-16 border-t border-border bg-card md:hidden">
      <Button
        type="button"
        variant="ghost"
        onClick={() => navigate("/dashboard")}
        className={`h-full flex-1 flex-col gap-1 rounded-none text-xs ${isDashboard ? "text-primary " : "text-muted-foreground"}`}
      >
        <LayoutDashboard className="size-5" />
        Dashboard
      </Button>

      <Button
        type="button"
        variant="ghost"
        onClick={() => navigate("/subscriptions")}
        className={`h-full flex-1 flex-col gap-1 rounded-none text-xs ${isSubscriptions ? "text-primary" : "text-muted-foreground"}`}
      >
        <ReceiptText className="size-5" />
        Subscriptions
      </Button>

      <Button
        type="button"
        variant="ghost"
        onClick={handleLogOut}
        className="flex h-full flex-1 flex-col gap-1 text-muted-foreground"
      >
        <LogOut className="size-5" />
        <span className="text-xs">Logout</span>
      </Button>
    </nav>
  );
}
