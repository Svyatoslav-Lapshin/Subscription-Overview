import { Outlet } from "react-router-dom";
import Sidebar from "./AppSidebar";

export default function AuthenticatedLayout() {
  return (
    <div className="flex min-h-screen bg-background">
      <Sidebar />

      <main className="min-w-0 flex-1">
        <Outlet />
      </main>
    </div>
  );
}
