import { useLocation, useNavigate } from "react-router-dom";
import { LayoutDashboard, ReceiptText, Plus, LogOut } from "lucide-react";
import LogoIcon from "@/assets/LogoICon.svg?react";
import { Button } from "../ui/button";
import { useAuth } from "@/context/AuthContext";

export default function Sidebar() {
  const navigate = useNavigate();
  const location = useLocation();

  const isDashboard = location.pathname === "/dashboard";
  const isSubscriptions = location.pathname.startsWith("/subscriptions");
  const { logout } = useAuth();
  const { user } = useAuth();

  const handleLogOut = async () => {
    await logout();
    navigate("/login", { replace: true });
  };

  return (
    <aside className="hidden h-screen w-56 shrink-0 flex-col border-r border-border bg-card md:flex">
      {/*Logo*/}
      <div className="flex h-14 items-center gap-2.5 border-b border-border justify-center">
        <LogoIcon className="size-7 shrink-0 " />
        <div className="flex flex-col">
          <span className="text-xs font-semibold leading-3 text-foreground">
            Subscription
          </span>
          <span className="pt-0.5 text-xs font-semibold leading-3 text-primary">
            Overview
          </span>
        </div>
      </div>

      <nav className="flex flex-1 flex-col gap-0.5 px-3 py-4">
        <Button
          type="button"
          variant="ghost"
          onClick={() => navigate("/dashboard")}
          className={`h-10 w-full items-center gap-3 rounded-xl px-3 text-sm font-medium ${isDashboard ? "bg-primary-light text-primary hover:bg-primary-light hover:text-primary" : "text-muted-foreground hover:bg-secondary hover:text-foreground"}`}
        >
          <LayoutDashboard className="size-4" />
          Dashboard
        </Button>

        <Button
          type="button"
          variant="ghost"
          onClick={() => navigate("/subscriptions")}
          className={`h-10 w-full items-center gap-3 rounded-xl px-3 text-sm font-medium ${isSubscriptions ? "bg-primary-light text-primary hover:bg-primary-light hover:text-primary" : "text-muted-foreground hover:bg-secondary hover:text-foreground"}`}
        >
          <ReceiptText className="size-4" />
          Subscriptions
        </Button>
      </nav>

      <div className="px-3 pb-3">
        <Button
          type="button"
          onClick={() => navigate("/subscriptions/add")}
          className="h-10 w-full rounded-lg"
        >
          <Plus className="size-4" />
          Add subscription
        </Button>
      </div>
      {/*User*/}
      <div className="flex items-center gap-3 border-t border-border p-4">
        <div className="flex size-8 shrink-0 items-center justify-center rounded-full bg-primary-light text-sm font-semibold text-primary">
          {user?.firstName?.charAt(0).toUpperCase()}
        </div>
        <div className="min-w-0 flex-1">
          <p className="truncate text-sm font-medium text-foreground">
            {user?.firstName} {user?.lastName}
          </p>
          <p className="truncate text-sm font-medium text-foreground">
            {user?.email}
          </p>
        </div>
        <Button
          type="button"
          variant="ghost"
          size="icon"
          onClick={handleLogOut}
          className="shrink-0 text-muted-foreground hover:text-foreground"
        >
          <LogOut className="size-4" />
        </Button>
      </div>
    </aside>
  );
}
