import { Moon, Sun } from "lucide-react";
import { useTheme } from "@/context/ThemeContext";
import { Button } from "@/components/ui/button";

export default function ThemeToggle({ showLabel = false, className }) {
  const { isDark, setIsDark } = useTheme();
  const Icon = isDark ? Sun : Moon;
  const label = isDark ? "Switch to light mode" : "Switch to dark mode";

  return (
    <Button
      type="button"
      variant="ghost"
      size={showLabel ? "default" : "icon"}
      onClick={() => setIsDark((current) => !current)}
      aria-label={label}
      aria-pressed={isDark}
      title={label}
      className={className}
    >
      <Icon className="size-4" />
      {showLabel && <span>Theme</span>}
    </Button>
  );
}
