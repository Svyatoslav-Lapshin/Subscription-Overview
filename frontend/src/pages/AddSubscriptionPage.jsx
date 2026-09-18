import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Card, CardContent, CardHeader } from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { ChevronLeft } from "lucide-react";
import { createProvider, getProviders } from "@/api/providersApi";
import { createCategory, getCategories } from "@/api/categoriesApi";
import { Input } from "@/components/ui/input";
import { createSubscription } from "@/api/subscriptionsApi";

export default function AddSubscriptionPage() {
  /*Subscription form data*/
  const [subscription, setSubscription] = useState({
    price: "",
    billingInterval: "",
    categoryId: "",
    providerId: "",
    startDate: "",
    endDate: "",
  });

  /*Form options*/
  const [providers, setProviders] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isCustomProvider, setIsCustomProvider] = useState(false);
  const [isCustomCategory, setIsCustomCategory] = useState(false);
  const [customProviderName, setCustomProviderName] = useState("");
  const [customCategoryName, setCustomCategoryName] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  /*Load providers and categories*/
  useEffect(() => {
    const loadOptions = async () => {
      try {
        const providerResponse = await getProviders();
        const categoryResponse = await getCategories();

        setProviders(providerResponse);
        setCategories(categoryResponse);
      } catch (error) {
        console.error("Form options error:", error);
      }
    };
    loadOptions();
  }, []);

  const handleSubmit = async (event) => {
    event.preventDefault();

    setError("");

    if (!isCustomProvider && !subscription.providerId) {
      setError("Please select a provider");
      return;
    }

    if (isCustomProvider && !customProviderName.trim()) {
      setError("Please select a provider name");
      return;
    }

    if (!isCustomCategory && !subscription.categoryId) {
      setError("Please select a category");
      return;
    }

    if (isCustomCategory && !customCategoryName.trim()) {
      setError("Please select a category name");
      return;
    }

    if (!subscription.price || Number(subscription.price) <= 0) {
      setError("Please enter a valide price");
      return;
    }

    if (!subscription.billingInterval) {
      setError("Please select a billing interval");
      return;
    }

    if (!subscription.startDate) {
      setError("Please select a start date");
      return;
    }

    if (subscription.endDate && subscription.endDate < subscription.startDate) {
      setError("End date cannot be before start date");
      return;
    }

    try {
      let providerId = subscription.providerId;
      let categoryId = subscription.categoryId;

      if (isCustomProvider) {
        const newProvider = await createProvider({
          serviceName: customProviderName.trim(),
        });

        providerId = newProvider.id;
      }

      if (isCustomCategory) {
        const newCategory = await createCategory({
          categoryName: customCategoryName.trim(),
        });

        categoryId = newCategory.id;
      }

      const newSubscription = {
        price: Number(subscription.price),
        billingInterval: Number(subscription.billingInterval),
        categoryId: Number(categoryId),
        providerId: Number(providerId),
        startDate: subscription.startDate,
        endDate: subscription.endDate || null,
      };

      await createSubscription(newSubscription);

      navigate("/subscriptions");
    } catch (error) {
      console.error("Create subscription error:", error);
    }
  };

  const selectedProvider = providers.find(
    (provider) => provider.id.toString() === subscription.providerId,
  );

  const selectedCategory = categories.find(
    (category) => category.id.toString() === subscription.categoryId,
  );
  return (
    <div className="flex min-h-screen items-center justify-center p-6">
      <div className="w-full max-w-md">
        <div className="mb-6 flex items-start gap-3">
          <Button
            type="button"
            variant="ghost"
            size="icon"
            onClick={() => navigate("/subscriptions")}
          >
            <ChevronLeft className="size-4" />
          </Button>
          <div>
            <h1 className="text-2xl font-semibold">Add subscription</h1>
            <p className="text-sm text-muted-foreground">
              Track a new recurring cost
            </p>
          </div>
        </div>
        <form onSubmit={handleSubmit}>
          <Card className="mt-6 gap-0 rounded-xl border-border bg-card p-5 shadow-none">
            <CardHeader className="p-0">
              <p className="text-xs font-semibold uppercase tracking-tight text-muted-foreground">
                Provider
              </p>
            </CardHeader>
            <CardContent className="p-0">
              {/*Provider selection*/}
              <div>
                <label className=" mb-1.5 block text-sm font-medium text-foreground">
                  Select provider
                </label>
                <Select
                  value={isCustomProvider ? "custom" : subscription.providerId}
                  onValueChange={(value) => {
                    if (value === "custom") {
                      setIsCustomProvider(true);

                      setSubscription({
                        ...subscription,
                        providerId: "",
                      });
                    } else {
                      setIsCustomProvider(false);
                      setCustomProviderName("");
                      setSubscription({
                        ...subscription,
                        providerId: value,
                      });
                    }
                  }}
                >
                  <SelectTrigger className="h-10 w-full rounded-lg border-input bg-card px-3 text-sm text-foreground shadow-none">
                    <span>
                      {isCustomProvider
                        ? "Custom provider"
                        : selectedProvider?.serviceName || "Choose a provider"}
                    </span>
                  </SelectTrigger>
                  <SelectContent>
                    {providers.map((provider) => (
                      <SelectItem
                        key={provider.id}
                        value={provider.id.toString()}
                      >
                        {provider.serviceName}
                      </SelectItem>
                    ))}

                    <SelectItem value="custom">Custom provider</SelectItem>
                  </SelectContent>
                </Select>
                {isCustomProvider && (
                  <div className="mt-4">
                    <label className="mb-2 block text-sm font-medium">
                      Provider name
                    </label>

                    <Input
                      type="text"
                      placeholder="e.g. Canva, Grammarly"
                      value={customProviderName}
                      onChange={(event) =>
                        setCustomProviderName(event.target.value)
                      }
                    />
                  </div>
                )}
                {/*Category selection*/}
                <div className="mt-4">
                  <label className=" mb-2 block text-sm font-medium">
                    Category
                  </label>

                  <Select
                    value={
                      isCustomCategory ? "custom" : subscription.categoryId
                    }
                    onValueChange={(value) => {
                      if (value === "custom") {
                        setIsCustomCategory(true);

                        setSubscription({
                          ...subscription,
                          categoryId: "",
                        });
                      } else {
                        setIsCustomCategory(false);
                        setCustomCategoryName("");
                        setSubscription({
                          ...subscription,
                          categoryId: value,
                        });
                      }
                    }}
                  >
                    <SelectTrigger className="w-full">
                      <span>
                        {isCustomCategory
                          ? "Custom category"
                          : selectedCategory?.categoryName ||
                            "Choose a category"}
                      </span>
                    </SelectTrigger>
                    <SelectContent>
                      {categories.map((category) => (
                        <SelectItem
                          key={category.id}
                          value={category.id.toString()}
                        >
                          {category.categoryName}
                        </SelectItem>
                      ))}

                      <SelectItem value="custom">Custom category</SelectItem>
                    </SelectContent>
                  </Select>
                  {isCustomCategory && (
                    <div className="mt-4">
                      <label className="mb-2 block text-sm font-medium">
                        Category name
                      </label>

                      <Input
                        type="text"
                        placeholder="e.g. Education, Fitness"
                        value={customCategoryName}
                        onChange={(event) =>
                          setCustomCategoryName(event.target.value)
                        }
                      />
                    </div>
                  )}
                </div>
              </div>
            </CardContent>
          </Card>
          <Card className="mt-6">
            {/*Billing details*/}
            <CardHeader>
              <p className="text-xs font-semibold uppercase text-muted-foreground">
                Billing
              </p>
            </CardHeader>
            <CardContent>
              <div>
                <label className="mb-2 block text-sm font-medium">Price</label>

                <Input
                  type="number"
                  min="0.01"
                  step="0.01"
                  placeholder="0.00"
                  value={subscription.price}
                  onChange={(event) =>
                    setSubscription({
                      ...subscription,
                      price: event.target.value,
                    })
                  }
                />
              </div>
              <div className="mt-4">
                <label className="mb-2 block text-sm font-medium">
                  Billing interval
                </label>
                <div className="grid grid-cols-3  gap-2 rounded-xl  p-1 ">
                  <Button
                    type="button"
                    variant={
                      subscription.billingInterval === "1"
                        ? "default"
                        : "outline"
                    }
                    className="h-11 rounded-lg px-4 text-sm font-medium"
                    onClick={() => {
                      setSubscription({
                        ...subscription,
                        billingInterval: "1",
                      });
                    }}
                  >
                    Monthly
                  </Button>
                  <Button
                    type="button"
                    variant={
                      subscription.billingInterval === "2"
                        ? "default"
                        : "outline"
                    }
                    className="h-11 rounded-lg px-4 text-sm font-medium"
                    onClick={() => {
                      setSubscription({
                        ...subscription,
                        billingInterval: "2",
                      });
                    }}
                  >
                    Quarterly
                  </Button>

                  <Button
                    type="button"
                    variant={
                      subscription.billingInterval === "3"
                        ? "default"
                        : "outline"
                    }
                    className="h-11 rounded-lg px-4 text-sm font-medium"
                    onClick={() => {
                      setSubscription({
                        ...subscription,
                        billingInterval: "3",
                      });
                    }}
                  >
                    Yearly
                  </Button>
                </div>
              </div>
              <div className="mt-4 grid gap-4 sm:grid-cols-2 ">
                <div>
                  <label className="mb-2 block text-sm font-medium">
                    Start date
                  </label>

                  <Input
                    type="date"
                    value={subscription.startDate}
                    onChange={(event) =>
                      setSubscription({
                        ...subscription,
                        startDate: event.target.value,
                      })
                    }
                  />
                </div>
                <div>
                  <label className="mb-2 block text-sm font-medium">
                    End date
                    <span className="ml-1 text-muted-foreground">
                      (optional)
                    </span>
                  </label>

                  <Input
                    type="date"
                    value={subscription.endDate}
                    onChange={(event) =>
                      setSubscription({
                        ...subscription,
                        endDate: event.target.value,
                      })
                    }
                  />
                </div>
              </div>
            </CardContent>
          </Card>
          {error && <p className="mt-4 text-sm text-destructive">{error}</p>}
          <div className="mt-4 grid grid-cols-2 gap-3">
            <Button
              type="button"
              variant="outline"
              onClick={() => navigate("/subscriptions")}
              className="h-12 w-full rounded-xl text-sm font-medium"
            >
              Cancel
            </Button>
            <Button
              type="submit"
              className="h-12 w-full rounded-xl bg-primary text-sm font-medium text-primary-foreground hover:bg-primary-hover"
            >
              Add subscription
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
