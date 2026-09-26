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
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import LogoIcon from "@/assets/LogoICon.svg?react";
import { validateEmail } from "@/lib/validateEmail";
import { loginUser } from "@/api/authApi";
import { useAuth } from "@/context/AuthContext";
import { tokenStore } from "@/lib/tokenStore";

export default function LoginPage() {
  /*Login form data*/
  const [credentials, setCredentials] = useState({
    email: "",
    password: "",
  });

  const navigate = useNavigate();
  const { setAccessToken, setUser } = useAuth();

  /*Form errors*/
  const [errors, setErrors] = useState({});
  /*Update input values*/
  const handleInput = (e) => {
    const { name, value } = e.target;

    setCredentials((prev) => ({
      ...prev,
      [name]: value,
    }));

    setErrors((prev) => ({
      ...prev,
      [name]: "",
    }));
  };
  /*Validate login fields*/
  const validateField = (name, value) => {
    if (!value.trim()) {
      return "This field is required";
    }

    if (name === "email") {
      return validateEmail(value);
    }

    return "";
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const newErrors = {};

    const emailError = validateField("email", credentials.email);

    if (emailError) {
      newErrors.email = emailError;
    }

    const passwordError = validateField("password", credentials.password);

    if (passwordError) {
      newErrors.password = passwordError;
    }

    setErrors(newErrors);

    if (Object.keys(newErrors).length > 0) {
      return;
    }

    const loginData = {
      email: credentials.email,
      password: credentials.password,
    };
    /*Send login request*/
    try {
      const response = await loginUser(loginData);
      /*Save access token*/
      setAccessToken(response.accessToken);
      tokenStore.set(response.accessToken);
      setUser({
        firstName: response.firstName,
        lastName: response.lastName,
        email: response.email,
      });
      /*Go to dashboard*/
      navigate("/dashboard", { replace: true });
    } catch (error) {
      /*Handle invalid login*/
      if (error.response?.status === 401) {
        setErrors((prev) => ({
          ...prev,
          password: "Invalid email or password",
        }));
        return;
      }

      console.error("Login error:", error);
    }
  };

  return (
    <div className="flex flex-col min-h-svh items-center justify-center px-4">
      <div className="mb-8 flex items-center gap-2">
        <LogoIcon className="size-8" />

        <p className="text-lg font-semibold">
          Subscription <span className="text-primary">Overview</span>
        </p>
      </div>
      <Card className="w-full max-w-[25.625rem]">
        <CardHeader>
          <CardTitle className="text-xl font-semibold">Welcome back</CardTitle>
          <CardDescription>Sign in to your account</CardDescription>
        </CardHeader>

        <form onSubmit={handleSubmit} noValidate>
          <CardContent>
            <div className="grid gap-2 mt-4 ">
              <Label htmlFor="email">Email</Label>
              <Input
                name="email"
                id="email"
                type="email"
                placeholder="Enter your email"
                className="px-4 h-[2.625rem]"
                value={credentials.email}
                onChange={handleInput}
                required
                aria-invalid={!!errors.email}
              />
              {errors.email && (
                <p className="text-sm text-destructive"> {errors.email}</p>
              )}
            </div>
            <div className="grid gap-2 mt-4">
              <div className="flex items-center justify-between">
                <Label htmlFor="password">Password</Label>
                <span className="text-xs text-primary">Forgot password?</span>
              </div>
              <Input
                name="password"
                id="password"
                type="password"
                placeholder="Enter password"
                className="px-4 h-[2.625rem]"
                value={credentials.password}
                onChange={handleInput}
                required
                aria-invalid={!!errors.password}
              />
              {errors.password && (
                <p className="text-sm text-destructive">{errors.password}</p>
              )}
            </div>
          </CardContent>
          <CardFooter className="bg-card border-t-0 flex-col gap-4">
            <Button type="submit" className="w-full h-[2.625rem] text-base">
              Sign in
            </Button>

            <p className="text-sm text-muted-foreground">
              No account?{" "}
              <Link to="/register" className="text-primary">
                Create one
              </Link>
            </p>
          </CardFooter>
        </form>
      </Card>

      <div className="mt-6 text-center">
        <Link
          to="/"
          className="text-sm text-muted-foreground transition-colors hover:text-foreground"
        >
          ← Back to home
        </Link>
      </div>
    </div>
  );
}
