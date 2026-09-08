import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { TooltipProvider } from "@/components/ui/tooltip";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./app/styles/globals.css";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <TooltipProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/login" element={<LoginPage />} />
        </Routes>
      </BrowserRouter>
    </TooltipProvider>
  </StrictMode>,
);
