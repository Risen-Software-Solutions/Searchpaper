import { atom } from "jotai";

export const drawerAtom = atom(
  JSON.parse(localStorage.getItem("isDrawerOpen") ?? "true") as boolean,
);
