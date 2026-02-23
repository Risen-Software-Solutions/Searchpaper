import { Link } from "wouter";
import Navbar from "./Navbar";
import { useAtomValue } from "jotai";
import { drawerAtom } from "../atoms/drawerAtom";

export default function Drawer(props: { children?: React.ReactNode }) {
  const isDrawerOpen = useAtomValue(drawerAtom);

  return (
    <div className={`drawer ${isDrawerOpen && "lg:drawer-open"}`}>
      <input id="drawer" type="checkbox" className="drawer-toggle" />
      <div className="drawer-content flex flex-col ">
        <Navbar />
        {props.children}
      </div>
      <div className="drawer-side">
        <label
          htmlFor="drawer"
          aria-label="close sidebar"
          className="drawer-overlay"
        />
        <ul
          className={`menu bg-base-200 min-h-full w-64 p-4 gap-3 rounded-e-xl`}
        >
          <li>
            <Link to="/">
              <i className="fa-solid fa-house" />
              Início
            </Link>
          </li>
          <li>
            <Link to="/manageaccount">
              <i className="fa-solid fa-user" />
              Perfil
            </Link>
          </li>
        </ul>
      </div>
    </div>
  );
}
