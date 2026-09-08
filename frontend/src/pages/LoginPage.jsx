import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import { useState } from "react";
import LogoIcon from "../assets/LogoICon.svg?react";

export default function LoginPage() {
  return (
    <div className="flex flex-col min-h-svh items-center justify-center px-4">
      <div className="mb-8 flex items-center gap-2">
        <LogoIcon className=" size-8 " />

        <p className="text-lg font-semibold">
          Subscription <span className="text-primary">Overview</span>
        </p>
      </div>
      <Card className="w-full max-w-[25.625rem]">
        <CardHeader>
          <CardTitle className="text-xl font-semibold">Welcome back</CardTitle>
          <CardDescription>Sign in to your account</CardDescription>
        </CardHeader>

        <CardContent>
          <div className="grid gap-2 mt-4 ">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="Enter your email"
              className="px-4 h-[2.625rem]"
            />
          </div>
          <div className="grid gap-2 mt-4">
            <div className="flex items-center justify-between">
              <Label htmlFor="password">Password</Label>
              <span className="text-xs text-primary">Forgot password?</span>
            </div>
            <Input
              id="password"
              type="password"
              placeholder="Enter password"
              className="px-4 h-[2.625rem]"
            />
          </div>
        </CardContent>
        <CardFooter className="bg-card border-t-0 flex-col gap-4">
          <Button className="w-full h-[2.625rem] text-base">Sign in</Button>

          <p className="text-sm text-muted-foreground">
            No account?{" "}
            <Link to="/register" className="text-primary">
              Create one
            </Link>
          </p>
        </CardFooter>
      </Card>
      <p className="mt-6 text-sm text-muted-foreground "> ← Back to home</p>
    </div>
  );
}
