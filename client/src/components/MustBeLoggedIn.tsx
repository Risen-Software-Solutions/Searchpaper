import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Redirect, Route, type RouteProps } from "wouter";
import LoadingScreen from "./LoadingScreen";
import { useSetAtom } from "jotai";
import { userAtom } from "../atoms/userAtom";

export default function MustBeLoggedIn(props: RouteProps) {
  const setUser = useSetAtom(userAtom);

  const { isFetching, error, data } = useQuery({
    queryKey: ["info"],
    queryFn: () => axios("/api/account/info"),
    retry: 0,
  });

  if (isFetching) {
    return <LoadingScreen />;
  }

  if (error != null) {
    return <Redirect to="/login" />;
  }

  if (data) {
    setUser(data.data);
  }

  return <Route {...props} />;
}
