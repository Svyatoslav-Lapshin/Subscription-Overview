export function getCategoryColor(category) {
  if (category === "Streaming") return "var(--color-category-streaming)";
  if (category === "Music") return "var(--color-category-music)";
  if (category === "Software") return "var(--color-category-software)";
  if (category === "Cloud") return "var(--color-category-cloud)";
  if (category === "Productivity") return "var(--color-category-productivity)";
  if (category === "Gaming") return "var(--color-category-gaming)";

  return "var(--color-category-other)";
}

export function getBillingIntervalText(billingInterval) {
  if (billingInterval === 1) return "per month";
  if (billingInterval === 2) return "per quarter";
  if (billingInterval === 3) return "per year";

  return "";
}
