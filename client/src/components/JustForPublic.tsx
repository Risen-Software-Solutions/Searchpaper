import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Redirect, Route, type RouteProps } from "wouter";
import LoadingScreen from "./LoadingScreen";

export default function JustForPublic(props: RouteProps) {
  const { isFetching, error } = useQuery({
    queryKey: ["info"],
    queryFn: () => axios("/api/account/info"),
    retry: 0,
  });

  if (isFetching) {
    return <LoadingScreen />;
  }

  if (error == null) {
    return <Redirect to="/" />;
  }

  return <Route {...props} />;
}
