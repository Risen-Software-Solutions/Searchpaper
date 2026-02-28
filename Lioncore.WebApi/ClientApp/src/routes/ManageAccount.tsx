import { useAtomValue, useSetAtom } from "jotai";
import Drawer from "../components/Drawer";
import { writeToast } from "../atoms/toastAtom";
import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { userAtom } from "../atoms/userAtom";

export default function Route() {
  const alert = useSetAtom(writeToast);
  const user = useAtomValue(userAtom);

  const changePasswordMutation = useMutation({
    mutationFn: (account: { newPassword: string; oldPassword: string }) => {
      return axios
        .put("/api/manage/changepassword", account)
        .catch((err) => Promise.reject(err.response.data));
    },
    onError(error) {
      alert(error);
    },
    onSuccess(_data) {
      alert(null);
    },
  });

  const { isPending: changePasswordPending } = changePasswordMutation;

  const onSubmitChangePassword = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = e.target as HTMLFormElement;

    const { value: oldPassword } = form["oldpassword"];
    const { value: newPassword } = form["newpassword"];

    changePasswordMutation.mutate({ oldPassword, newPassword });
  };
  const changeInfoMutation = useMutation({
    mutationFn: (account: { fullName: string }) => {
      return axios
        .put("/api/manage/changeinfo", account)
        .catch((err) => Promise.reject(err.response.data));
    },
    onError(error) {
      alert(error);
    },
    onSuccess(_data) {
      alert(null);
    },
  });

  const { isPending: changeInfoPending } = changePasswordMutation;

  const onSubmitChangeInfo = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = e.target as HTMLFormElement;

    const { value: fullName } = form["fullname"];

    changeInfoMutation.mutate({ fullName });
  };

  return (
    <Drawer>
      <div className="flex md:flex-row flex-col lg:items-start items-center gap-3 p-3">
        <form className="fieldset w-xs gap-2" onSubmit={onSubmitChangePassword}>
          <label className="label">Senha Antiga</label>
          <input
            type="password"
            name="oldpassword"
            placeholder="Senha Antiga"
            className="input w-full"
            required
            minLength={6}
            maxLength={100}
          />
          <label className="label">Nova Senha</label>
          <input
            type="password"
            name="newpassword"
            placeholder="Nova Senha"
            className="input w-full"
            required
            minLength={6}
            maxLength={100}
          />
          <button
            className={`btn btn-primary  ${changePasswordPending && "pointer-none"}`}
          >
            {!changePasswordPending && "Alterar Senha"}
            {changePasswordPending && (
              <span className="loading loading-dots loading-xs"></span>
            )}
          </button>
        </form>
        <form className="fieldset w-xs gap-2" onSubmit={onSubmitChangeInfo}>
          <label className="label">Seu Nome</label>
          <input
            type="text"
            name="fullname"
            placeholder="Nome"
            className="input w-full"
            required
            minLength={6}
            maxLength={100}
            defaultValue={user.fullName}
          />

          <button
            className={`btn btn-primary  ${changeInfoPending && "pointer-none"}`}
          >
            {!changeInfoPending && "Alterar Dados"}
            {changeInfoPending && (
              <span className="loading loading-dots loading-xs"></span>
            )}
          </button>
        </form>
      </div>
    </Drawer>
  );
}
