import { atom } from "jotai";
import { v4 as uuidv4 } from "uuid";

export const toastAtom = atom<ToastAlert[]>([]);

export const writeToast = atom(
  null,
  (
    _get,
    set,
    error: Nullable<Error>,
    message: string = error ? "Algo deu Errado" : "Operação bem Sucedida",
  ) => {
    const id = uuidv4();
    const success = error == null;
    const alert: ToastAlert = { message, success, id };

    if (error) {
      console.log(error);
    }

    set(toastAtom, (prev) => [...prev, alert]);

    setTimeout(() => {
      set(toastAtom, (prev) => [...prev].filter((alert) => alert.id != id));
    }, 5_000);
  },
);
