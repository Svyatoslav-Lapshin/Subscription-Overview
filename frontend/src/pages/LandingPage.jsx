import LandingNavBar from "@/components/layout/LandingNavbar";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { WalletCards, ChartPie, CalendarDays } from "lucide-react";

const features = [
  {
    title: "Track every cost",
    description:
      "All your subscriptions in one place. Understand exactly where your money goes each month.",
    icon: WalletCards,
  },
  {
    title: "Spending insights",
    description:
      "Category breakdowns and period totals help you see where you can cut.",
    icon: ChartPie,
  },
  {
    title: "Renewal dates",
    description:
      "See when every subscription renews so you are never surprised by a charge.",
    icon: CalendarDays,
  },
];
export default function LandingPage() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-background">
      <LandingNavBar />

      <main>
        <section className="flex flex-col items-center px-4 py-14 text-center sm:px-6 sm:py-20">
          <h1 className="max-w-2xl text-3xl font-bold leading-tight text-foreground sm:text-4xl">
            All your subscriptions,{" "}
            <span className="text-primary">one clear view.</span>
          </h1>

          <p className="mt-5 max-w-md text-m leading-6 text-muted-foreground">
            Stop losing track of what you pay for. Stay in control of your
            monthly costs in seconds.
          </p>

          <Button
            type="button"
            onClick={() => navigate("/register")}
            className="h-10 px-3 mt-7 w-full  max-w-xs sm:w-auto"
          >
            Create free account
          </Button>
          <p className="mt-6 text-m text-muted-foreground">
            Know exactly what you pay each month.
          </p>
        </section>

        {/*Features*/}
        <section className="border-t border-border bg-card px-4 py-14 sm:px-6 sm:py-16">
          <div className="mx-auto max-w-4xl">
            <h2 className="text-center text-2xl font-bold text-foreground">
              Everything you need to manage subscriptions
            </h2>
            <div className="mt-10 grid gap-6 md:grid-cols-3">
              {features.map((feature) => {
                const Icon = feature.icon;

                return (
                  <div
                    key={feature.title}
                    className="rounded-xl border border-border bg-background p-6"
                  >
                    <div className="flex size-9 items-center justify-center rounded-lg bg-primary-light">
                      <Icon className="size-4 text-primary" />
                    </div>
                    <h3 className="mt-4 text-base font-semibold text-foreground">
                      {feature.title}
                    </h3>

                    <p className="mt-1.5 text-sm leading-6 text-muted-foreground">
                      {feature.description}
                    </p>
                  </div>
                );
              })}
            </div>
          </div>
        </section>
      </main>
      <footer className="flex w-full flex-col items-center border-t border-border p-5">
        <p className="text-center text-sm font-normal leading-5 text-muted-foreground">
          © {new Date().getFullYear()} Subscription Overview
        </p>
      </footer>
    </div>
  );
}
