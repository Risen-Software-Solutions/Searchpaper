import { useSetAtom } from "jotai";
import { Link } from "wouter";
import { writeToast } from "../atoms/toastAtom";
import { navigate } from "wouter/use-browser-location";
import placeholder from "../assets/placeholder.svg";
import logo from "../assets/logo.svg";
import { useMutation } from "@tanstack/react-query";
import axios from "axios";

export default function Route() {
  const alert = useSetAtom(writeToast);

  const mutation = useMutation({
    mutationFn: (account: {
      fullName: string;
      email: string;
      password: string;
    }) => {
      return axios
        .post("/api/account/register", account)
        .catch((err) => Promise.reject(err.response.data));
    },
    onError(error) {
      alert(error);
    },
    onSuccess(_data) {
      alert(null, "Verifque seu E-mail");
      navigate("/login");
    },
  });

  const { isPending } = mutation;

  const onSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = e.target as HTMLFormElement;

    const { value: fullName } = form["fullname"];
    const { value: email } = form["email"];
    const { value: password } = form["password"];

    mutation.mutate({ email, fullName, password });
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
        <form className="fieldset w-xs gap-2" onSubmit={onSubmit}>
          <label className="label">Nome</label>
          <input
            type="text"
            name="fullname"
            placeholder="Nome"
            className="input w-full"
            required
          />
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
            minLength={6}
            maxLength={100}
          />
          <button className={`btn btn-primary  ${isPending && "pointer-none"}`}>
            {!isPending && "Registrar"}
            {isPending && (
              <span className="loading loading-dots loading-xs"></span>
            )}
          </button>
        </form>
        <div className="text-center">
          <Link to="/login" className="link link-hover text-xs">
            Já tem uma conta?
          </Link>
        </div>
      </section>
    </div>
  );
}
