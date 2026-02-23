import { useAtomValue } from "jotai";
import { toastAtom } from "../atoms/toastAtom";

export default function Toast() {
  const toast = useAtomValue(toastAtom);

  const alerts = toast.map((alert) => (
    <div key={alert.id}>
      {alert.success && (
        <div className="alert alert-success">
          <span>{alert.message}</span>
        </div>
      )}

      {!alert.success && (
        <div className="alert alert-error">
          <span>{alert.message}</span>
        </div>
      )}
    </div>
  ));

  return <div className="toast toast-center toast-top z-50">{alerts}</div>;
}
