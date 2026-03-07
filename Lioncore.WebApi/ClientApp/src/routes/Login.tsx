import { useSetAtom } from "jotai";
import { Link } from "wouter";
import { writeToast } from "../atoms/toastAtom";
import { navigate } from "wouter/use-browser-location";
import placeholder from "../assets/placeholder.svg";
import logo from "../assets/logo.svg";
import axios from "axios";
import { useMutation } from "@tanstack/react-query";

export default function Route() {
  const alert = useSetAtom(writeToast);

  const mutation = useMutation({
    mutationFn: (account: { email: string; password: string }) => {
      return axios
        .post("/api/login?useCookies=true", account)
        .catch((err) => Promise.reject(err.response.data));
    },
    onError(error) {
      alert(error);
    },
    onSuccess(_data) {
      navigate("/");
    },
  });

  const { isPending } = mutation;

  const onSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = e.target as HTMLFormElement;

    const { value: email } = form["email"];
    const { value: password } = form["password"];

    mutation.mutate({ email, password });
  };
  return (
    <div className="flex min-h-screen justify-center">
      <section id="section-placeholder" className="md:basis-1/2 hidden md:flex">
        <img src={placeholder} className="w-full h-full object-cover" />
      </section>
      <section
        id="section-form"
        className="basis-1/2 grid items-center justify-center"
      >
        <div></div>
        <img src={logo} className="w-40 place-self-center" />
        <form className="fieldset w-xs gap-3" onSubmit={onSubmit}>
          <label className="label">Email</label>
          <input
            type="email"
            name="email"
            placeholder="Email"
            className="input w-full"
            required
          />
          <label className="label">Senha</label>
          <input
            type="password"
            name="password"
            placeholder="Senha"
            className="input w-full"
            required
          />
          <button className={`btn btn-primary  ${isPending && "pointer-none"}`}>
            {!isPending && "Entrar"}
            {isPending && (
              <span className="loading loading-dots loading-xs"></span>
            )}
          </button>
          <Link
            to="/forgotpassword"
            className="link link-hover text-xs text-end"
          >
            Esqueceu sua senha ?
          </Link>
        </form>
        <div className="text-center">
          <Link to="/register" className="link link-hover text-xs">
            Ainda não tem uma conta?
          </Link>
        </div>
      </section>
    </div>
  );
}
