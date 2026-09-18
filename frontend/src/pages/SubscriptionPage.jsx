import {
  deleteSubscription,
  getUserSubscriptions,
} from "@/api/subscriptionsApi";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Pencil, Trash2 } from "lucide-react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
export default function SubscriptionPage() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [subscriptions, setSubscriptions] = useState([]);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [subscriptionToDelete, setSubscriptionToDelte] = useState(null);

  const handleDelete = async (id) => {
    try {
      await deleteSubscription(id);
      setSubscriptions((currentSubscriptions) =>
        currentSubscriptions.filter((subscription) => subscription.id !== id),
      );

      setIsDeleteDialogOpen(false);
      setSubscriptionToDelte(null);
    } catch (error) {
      console.error("Delete subscription error", error);
    }
  };
  /*Load subscriptions*/
  useEffect(() => {
    const loadSubscriptions = async () => {
      try {
        const subscriptionResponse = await getUserSubscriptions();
        setSubscriptions(subscriptionResponse);
      } catch (error) {
        console.error("Subscriptions error:", error);
        setError("Failed to load subscriptions");
      } finally {
        setIsLoading(false);
      }
    };
    loadSubscriptions();
  }, []);
  /*Filte subscrtion (will remove to the backend)*/
  const today = new Date().toISOString().split("T")[0];
  const activeSubscriptions = subscriptions.filter(
    (subscription) => !subscription.endDate || subscription.endDate >= today,
  );
  const endedSubscriptions = subscriptions.filter(
    (subscription) => subscription.endDate && subscription.endDate < today,
  );

  /*Calculate monthly cost*/
  const totalMonthlyCost = activeSubscriptions.reduce(
    (total, subscription) => total + subscription.monthlyCost,
    0,
  );

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

  return (
    <div className="p-6 ">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold">Subscriptions</h1>
          <p className="text-sm text-muted-foreground">
            Manage all your subscriptions
          </p>
        </div>
        <Button onClick={() => navigate("/subscriptions/add")}>
          + Add subscription
        </Button>
      </div>
      <Card className="mt-6">
        <CardHeader className="flex flex-row items-center justify-between">
          <p className="text-xs text-muted-foreground">
            {activeSubscriptions.length} active · {endedSubscriptions.length}{" "}
            ended
          </p>
          <p className="text-xs text-muted-foreground">
            {totalMonthlyCost} kr/mo
          </p>
        </CardHeader>
        <CardContent>
          {subscriptions.map((subscription) => {
            /*Check subscription status*/
            const isActive =
              !subscription.endDate || subscription.endDate >= today;
            return (
              <div
                key={subscription.id}
                className="group mt-3 flex items-center gap-3 justify-between rounded-xl border p-3 transition hover:bg-muted/30"
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

                <div className="flex shrink-0 items-center gap-3">
                  <div className="flex shrink-0 flex-col items-end gap-1">
                    <p className="font-semibold">{subscription.price} kr</p>
                    <Badge
                      variant="secondary"
                      className={
                        isActive
                          ? "rounded-full bg-success-background text-[0.625rem] font-medium text-success"
                          : "rounded-full text-[0.625rem] font-medium"
                      }
                    >
                      {isActive ? "Active" : "Ended"}
                    </Badge>
                  </div>
                </div>

                <div className="flex items-center gap-2 opacity-100 transition-opacity lg:opacity-0 lg:group-hover:opacity-100">
                  <Button
                    type="button"
                    className="rounded-md p-2 text-muted-foreground hover:bg-muted hover:text-foreground"
                    onClick={() =>
                      navigate(`/subscriptions/${subscription.id}/edit`)
                    }
                  >
                    <Pencil className="w-4 h-4" />
                  </Button>
                  <Button
                    type="button"
                    className="rounded-md p-2 text-muted-foreground hover:bg-muted hover:text-destructive"
                    onClick={() => {
                      setSubscriptionToDelte(subscription);
                      setIsDeleteDialogOpen(true);
                    }}
                  >
                    <Trash2 className="w-4 h-4" />
                  </Button>
                </div>
              </div>
            );
          })}
        </CardContent>
      </Card>

      <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
        <DialogContent showCloseButton={false}>
          <DialogHeader>
            <DialogTitle>Delete subscription?</DialogTitle>

            <DialogDescription>
              Are you sure you want to delete{" "}
              <span className="font-medium text-foreground">
                {subscriptionToDelete?.serviceName}
              </span>{" "}
              ? This action cannot be undine
            </DialogDescription>
          </DialogHeader>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              className="h-11 min-w-24 rounded-lg px-5 text-sm font-medium"
              onClick={() => setIsDeleteDialogOpen(false)}
            >
              Cancel
            </Button>
            <Button
              type="button"
              variant="destructive"
              className="h-11 min-w-24 rounded-lg px-5 text-sm font-medium"
              onClick={() => handleDelete(subscriptionToDelete.id)}
            >
              Delete
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
