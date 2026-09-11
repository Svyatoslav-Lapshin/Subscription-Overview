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
import LogoIcon from "@/assets/LogoICon.svg?react";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { validateEmail } from "@/lib/validateEmail";
import { registerUser } from "@/api/authApi";
import { useAuth } from "@/context/AuthContext";

export default function RegisterPage() {
  /*Register form data*/
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  /*Form errors*/
  const [errors, setErrors] = useState({});
  const { setAccessToken } = useAuth();
  const navigate = useNavigate();
  /*Update input values*/
  const handleInput = (e) => {
    const { name, value } = e.target;

    setUser((prev) => ({
      ...prev,
      [name]: value,
    }));

    setErrors((prev) => ({
      ...prev,
      [name]: "",
    }));
  };
  /*Validate password*/
  const validatePassword = (password) => {
    if (password.length < 12) {
      return "Password must contain at least 12 characters";
    }

    /*Check uppercase*/
    if (!/[A-Z]/.test(password)) {
      return "Password must contain an uppercase letter";
    }
    /*Check lowercase*/
    if (!/[a-z]/.test(password)) {
      return "Password must contain a lowercase letter";
    }
    /*Check number*/
    if (!/\d/.test(password)) {
      return "Password must contain a number";
    }

    return "";
  };
  /*Validate form field*/
  const validateField = (name, value) => {
    if (!value.trim()) {
      return "This field is required";
    }

    if (name === "email") {
      return validateEmail(value);
    }

    if (name === "password") {
      return validatePassword(value);
    }

    return "";
  };
  /*Submit register form*/
  const handleSubmit = async (e) => {
    e.preventDefault();

    const newErrors = {};
    const firstNameError = validateField("firstName", user.firstName);
    if (firstNameError) {
      newErrors.firstName = firstNameError;
    }

    const lastNameError = validateField("lastName", user.lastName);
    if (lastNameError) {
      newErrors.lastName = lastNameError;
    }
    const emailError = validateField("email", user.email);

    if (emailError) {
      newErrors.email = emailError;
    }

    const passwordError = validateField("password", user.password);

    if (passwordError) {
      newErrors.password = passwordError;
    }

    const confirmPasswordError = validateField(
      "confirmPassword",
      user.confirmPassword,
    );

    if (confirmPasswordError) {
      newErrors.confirmPassword = confirmPasswordError;
    } else if (user.password !== user.confirmPassword) {
      newErrors.confirmPassword = "Passwords do not match";
    }

    setErrors(newErrors);

    if (Object.keys(newErrors).length > 0) {
      return;
    }
    /*Prepare register data*/
    const registerData = {
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      password: user.password,
    };
    /*Send register request*/
    try {
      const response = await registerUser(registerData);
      setAccessToken(response.accessToken);
      navigate("/dashboard", { replace: true });
    } catch (error) {
      /*Handle existing email*/
      if (error.response?.status === 409) {
        setErrors((prev) => ({
          ...prev,
          email: "An account with this email already exists",
        }));
        return;
      }
      console.error("Register error:", error);
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
          <CardTitle className="text-xl font-semibold">
            Create your account
          </CardTitle>
          <CardDescription>Enter your details to get started</CardDescription>
        </CardHeader>
        <form onSubmit={handleSubmit} noValidate className="grid gap-6 pt-2">
          <CardContent>
            <div className="grid gap-2">
              <Label htmlFor="firstName">First Name</Label>
              <Input
                id="firstName"
                name="firstName"
                type="text"
                placeholder="Enter your first name"
                className="px-4 h-[2.625rem]"
                value={user.firstName}
                onChange={handleInput}
                required
                aria-invalid={!!errors.firstName}
              />
              {errors.firstName && (
                <p className="text-sm text-destructive"> {errors.firstName}</p>
              )}
            </div>
            <div className="grid gap-2 mt-4">
              <Label htmlFor="lastName">Last Name</Label>
              <Input
                id="lastName"
                name="lastName"
                type="text"
                placeholder="Enter your last name"
                className="px-4 h-[2.625rem]"
                value={user.lastName}
                onChange={handleInput}
                required
                aria-invalid={!!errors.lastName}
              />
              {errors.lastName && (
                <p className="text-sm text-destructive"> {errors.lastName}</p>
              )}
            </div>
            <div className="grid gap-2 mt-4 ">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                name="email"
                type="email"
                placeholder="Enter your email"
                className="px-4 h-[2.625rem]"
                value={user.email}
                onChange={handleInput}
                required
                aria-invalid={!!errors.email}
              />
              {errors.email && (
                <p className="text-sm text-destructive"> {errors.email}</p>
              )}
            </div>
            <div className="grid gap-2 mt-4">
              <Label htmlFor="password">Password</Label>
              <Input
                id="password"
                name="password"
                type="password"
                placeholder="Min. 12 characters"
                className="px-4 h-[2.625rem]"
                value={user.password}
                onChange={handleInput}
                required
                minLength={12}
                aria-invalid={!!errors.password}
              />
              {errors.password && (
                <p className="text-sm text-destructive">{errors.password}</p>
              )}
            </div>
            <div className="grid gap-2 mt-4">
              <Label htmlFor="confirmPassword">Confirm Password</Label>
              <Input
                id="confirmPassword"
                name="confirmPassword"
                type="password"
                placeholder="Re-enter password"
                className="px-4 h-[2.625rem]"
                value={user.confirmPassword}
                onChange={handleInput}
                required
                minLength={12}
                aria-invalid={!!errors.confirmPassword}
              />
              {errors.confirmPassword && (
                <p className="text-sm text-destructive">
                  {errors.confirmPassword}
                </p>
              )}
            </div>
          </CardContent>
          <CardFooter className="bg-card border-t-0 flex-col gap-4 mt-4">
            <Button type="submit" className="w-full h-[2.625rem] text-base">
              Create Account
            </Button>

            <p className="text-sm text-muted-foreground">
              Already have an account?{" "}
              <Link to="/login" className="text-primary">
                Sign in
              </Link>
            </p>
          </CardFooter>
        </form>
      </Card>
      <p className="mt-6 text-sm text-muted-foreground "> ← Back to home</p>
    </div>
  );
}
