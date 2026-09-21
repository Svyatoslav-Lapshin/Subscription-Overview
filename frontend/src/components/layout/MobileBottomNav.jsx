import { ReceiptText, LayoutDashboard } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";

export default function MobileBottomNav() {
  const location = useLocation();
  const navigate = useNavigate();

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
    </nav>
  );
}
