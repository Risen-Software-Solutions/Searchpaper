import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import LoadingScreen from "../components/LoadingScreen";
import { Link, useSearchParams } from "wouter";

export default function Route() {
  const [searchParams] = useSearchParams();

  const userId = searchParams.get("userId");
  const code = searchParams.get("code");

  const { isFetching, isSuccess, isError } = useQuery({
    queryKey: ["confirmemail"],
    queryFn: () =>
      axios(`/api/account/confirmemail?userId=${userId}&code=${code}`),
    retry: 0,
  });

  if (isFetching) {
    return <LoadingScreen />;
  }

  return (
    <main className="flex flex-col justify-center grow max-w-md mx-auto text-center gap-3">
      {isSuccess && (
        <>
          <div className="text-center">
            <i className="fa-solid fa-champagne-glasses text-7xl" />
            <h1 className="text-4xl font-bold">Parabéns</h1>
          </div>
          <p className="text-xl flex-wrap">
            Seu E-mail foi confirmado com sucesso e já está pronto para ser
            usado
          </p>
          <Link
            to="/login"
            className="link link-primary link-hover text-center"
          >
            Entre com a sua conta
          </Link>
        </>
      )}
      {isError && (
        <>
          <div className="text-center">
            <i className="fa-solid fa-bug text-7xl" />
            <h1 className="text-4xl font-bold">Algo deu Errado</h1>
          </div>
          <p className="text-xl flex-wrap">
            Entre em contato com o provedor desse serviço
          </p>
          <Link
            to="/login"
            className="link link-primary link-hover text-center"
          >
            Retorne a página inicial
          </Link>
        </>
      )}
    </main>
  );
}
