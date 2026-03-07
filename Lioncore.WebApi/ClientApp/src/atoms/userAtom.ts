import { atom } from "jotai";

export const userAtom = atom<User>({
  email: "",
  fullName: "",
  id: "",
  role: "",
});
