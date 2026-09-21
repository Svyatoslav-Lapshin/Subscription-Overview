import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import LogoIcon from "@/assets/LogoICon.svg?react";

export default function LandingNavBar() {
  const navigate = useNavigate();

  return (
    <header className="flex h-16 items-center justify-between bg-card border-b px-4 sm:px-6 lg:px-12">
      <div className="flex items-center gap-2">
        <LogoIcon className="size shrink-0" />
        <p className="text-xs font-semibold sm:text-sm">
          Subscription <span className="text-primary">Overview</span>
        </p>
      </div>
      <div className="flex items-center gap-1 sm:gap-3">
        <Button
          type="button"
          variant="ghost"
          onClick={() => navigate("/login")}
          className="px-2 text-xs sm:px-4 sm:text-sm"
        >
          Log in
        </Button>
        <Button
          type="button"
          onClick={() => navigate("/register")}
          className="h-10 px-5 text-xs sm:px-4 sm:text-sm"
        >
          Get started
        </Button>
      </div>
    </header>
  );
}
