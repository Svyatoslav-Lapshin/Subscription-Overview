import { Outlet } from "react-router-dom";
import Sidebar from "./AppSidebar";
import MobileBottomNav from "./MobileBottomNav";

export default function AuthenticatedLayout() {
  return (
    <div className=" min-h-screen bg-background md:flex">
      <Sidebar />

      <main className="min-w-0 flex-1 pb-24 md:pb-0">
        <Outlet />
      </main>

      <MobileBottomNav />
    </div>
  );
}
