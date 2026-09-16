import { useEffect, useState } from "react";
import { getDashboardSummary } from "@/api/dashboardApi";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { getUserSubscriptions } from "@/api/subscriptionsApi";
import { Badge } from "@/components/ui/badge";
import { Pie, PieChart } from "recharts";
import { ChartContainer } from "@/components/ui/chart";

export default function DashboardPage() {
  /*Dashboard data*/
  const [summary, setSummary] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [subscriptions, setSubscriptions] = useState([]);
  /*Load dashboard data */
  useEffect(() => {
    const loadDashboardData = async () => {
      try {
        const response = await getDashboardSummary();
        const subscriptionResponse = await getUserSubscriptions();
        setSummary(response);
        setSubscriptions(subscriptionResponse);
      } catch (error) {
        console.error("Summary error:", error);
        setError("Failed to load dashboard");
      } finally {
        setIsLoading(false);
      }
    };
    loadDashboardData();
  }, []);
  /*Calculate coast by category */
  const categoryTotals = subscriptions.reduce((totals, subscription) => {
    const categoryName = subscription.categoryName;
    if (categoryName) {
      totals[categoryName] =
        /*Look if we have value or we take 0 as a value */
        (totals[categoryName] || 0) + subscription.monthlyCost;
    }
    return totals;
  }, {});
  /*Get category color*/
  const getCategoryColor = (category) => {
    if (category === "Streaming") return "var(--color-category-streaming)";
    if (category === "Music") return "var(--color-category-music)";
    if (category === "Software") return "var(--color-category-software)";
    if (category === "Cloud") return "var(--color-category-cloud)";
    if (category === "Productivity")
      return "var(--color-category-productivity)";
    if (category === "Gaming") return "var(--color-category-gaming)";

    return "var(--color-category-other)";
  };
  /*Prepare chart data*/
  const categoryData = Object.entries(categoryTotals).map(
    ([category, value]) => ({
      category,
      value,
      fill: getCategoryColor(category),
    }),
  );
  /*Format billing interval*/
  const getBillingIntervalText = (billingInterval) => {
    if (billingInterval === 1) return "per month";
    if (billingInterval === 2) return "per quarter";
    if (billingInterval === 3) return "per year";

    return "";
  };

  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }
  /*Calculate average monthly cost*/
  const averagePerSubscription =
    summary.activeSubscriptionsCount > 0
      ? Math.round(
          summary.totalMonthlyPayments / summary.activeSubscriptionsCount,
        )
      : 0;
  return (
    <div className="p-6">
      <h1 className="text-2xl font-semibold mb-6">Dashboard</h1>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4 ">
        <Card className="gap-0 rounded-xl border-primary bg-primary p-5 text-primary-foreground">
          <CardHeader className="p-0">
            <CardTitle className="text-xs font-medium uppercase tracking-tight">
              Monthly cost
            </CardTitle>
          </CardHeader>
          <CardContent className="p-0">
            <p className="pt-2 text-3xl font-bold">
              {summary.totalMonthlyPayments} kr
            </p>
            <p className="pt-2 text-xs opacity-80">
              {summary.activeSubscriptionsCount} active
            </p>
          </CardContent>
        </Card>
        <Card className="gap-0 rounded-xl p-5">
          <CardHeader className="p-0">
            <CardTitle className="text-xs font-medium uppercase tracking-tight text-muted-foreground">
              Active
            </CardTitle>
          </CardHeader>
          <CardContent className="p-0">
            <p className="pt-2 text-3xl font-bold">
              {summary.activeSubscriptionsCount}
            </p>
            <p className="pt-2 text-xs text-muted-foreground">
              of {summary.totalSubscriptionsCount} total
            </p>
          </CardContent>
        </Card>
        <Card className="gap-0 rounded-xl p-5">
          <CardHeader className="p-0">
            <CardTitle className="text-xs font-medium uppercase tracking-tight text-muted-foreground">
              Yearly est.
            </CardTitle>
          </CardHeader>
          <CardContent className="p-0">
            <p className="pt-2 text-3xl font-bold">
              {summary.totalYearlyPayments} kr
            </p>
            <p className="pt-2 text-xs text-muted-foreground">
              projected spend
            </p>
          </CardContent>
        </Card>
        <Card className="gap-0 rounded-xl p-5">
          <CardHeader className="p-0">
            <CardTitle className="text-xs font-medium uppercase tracking-tight text-muted-foreground">
              Avg. per sub
            </CardTitle>
          </CardHeader>
          <CardContent className="p-0">
            <p className="pt-2 text-3xl font-bold">
              {averagePerSubscription} kr
            </p>
            <p className="pt-2 text-xs text-muted-foreground">per month</p>
          </CardContent>
        </Card>
      </div>
      <div className="mt-6 grid gap-4 md:grid-cols-2 lg:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Cost by category</CardTitle>
            <p className="text-xs text-muted-foreground">
              Monthly, active only
            </p>
          </CardHeader>
          <ChartContainer config={{}} className="mx-auto  h-[15.625rem] w-full">
            <PieChart>
              <Pie
                data={categoryData}
                dataKey="value"
                nameKey="category"
                innerRadius={55}
                outerRadius={85}
              />
            </PieChart>
          </ChartContainer>
          <div className="mt-4 flex flex-wrap justify-center gap-3">
            {categoryData.map((item) => (
              <div key={item.category} className="flex items-center gap-1.5">
                <span
                  className="size-2 rounded-full"
                  style={{ backgroundColor: item.fill }}
                />
                <span className="text-xs text-muted-foreground">
                  {item.category}
                </span>
              </div>
            ))}
          </div>
        </Card>

        <Card>
          <CardHeader className="text-lg font-semibold">
            <CardTitle>Active subscriptions </CardTitle>
          </CardHeader>

          <CardContent>
            {subscriptions.map((subscription) => (
              <div
                key={subscription.id}
                className="mt-3 flex items-center gap-3 justify-between rounded-xl border p-3"
              >
                <div
                  className="flex size-9 shrink-0 items-center justify-center rounded-lg bg-primary text-xs font-bold text-white"
                  style={{
                    backgroundColor: getCategoryColor(
                      subscription.categoryName,
                    ),
                  }}
                >
                  {subscription.serviceName.charAt(0).toUpperCase()}
                </div>
                <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-semibold">
                    {subscription.serviceName}
                  </p>
                  <p className="text-xs font-semibold">
                    {subscription.categoryName} ·{" "}
                    {getBillingIntervalText(subscription.billingInterval)}
                  </p>
                </div>
                <div className="flex shrink-0 flex-col items-end gap-1">
                  <p className="font-semibold">{subscription.price} kr</p>
                  <Badge
                    variant="secondary"
                    className="rounded-full bg-success-background text-[0.625rem] font-medium text-success"
                  >
                    Active
                  </Badge>
                </div>
              </div>
            ))}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
