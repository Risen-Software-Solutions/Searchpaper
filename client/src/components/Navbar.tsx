import { useAtom } from "jotai";
import { drawerAtom } from "../atoms/drawerAtom";
import { useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { navigate } from "wouter/use-browser-location";

export default function Navbar() {
  const [isOpen, setIsOpen] = useAtom(drawerAtom);
  const handleDrawer = () => {
    setIsOpen(!isOpen);
  };

  const mutation = useMutation({
    mutationFn: () => {
      return axios.post("/api/account/logoff");
    },

    onSettled(_data) {
      navigate("login");
    },
  });

  const { isPending } = mutation;

  const logoff = () => {
    mutation.mutate();
  };

  useEffect(() => {
    localStorage.setItem("isDrawerOpen", String(isOpen));
  }, [isOpen]);

  return (
    <div className="navbar bg-base-100">
      <div className="navbar-start">
        <button
          className="btn btn-circle btn-ghost hidden lg:block"
          onClick={handleDrawer}
        >
          <i className="fa-solid fa-align-left" />
        </button>
        <label
          htmlFor="drawer"
          className="btn btn-circle btn-ghost drawer-button lg:hidden"
        >
          <i className="fa-solid fa-align-left" />
        </label>
      </div>
      <div className="navbar-center"></div>
      <div className="navbar-end">
        <button
          className={`btn btn-circle btn-ghost ${isPending && "pointer-events-none"}`}
          onClick={logoff}
        >
          {!isPending && <i className="fa-solid fa-right-from-bracket" />}
          {isPending && (
            <span className="loading loading-spinner loading-xs"></span>
          )}
        </button>
      </div>
    </div>
  );
}
