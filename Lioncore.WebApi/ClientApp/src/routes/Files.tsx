import Drawer from "../components/Drawer";
import { useEffect, useState } from "react";
import { Link } from "wouter";
import { useBrowserLocation } from "wouter/use-browser-location";

export default function Route() {
  const [filesResponse, setFilesResponse] = useState<FilesResponse>({
    files: [],
    commonPrefixes: [],
    prefix: "",
  });

  const [location, navigate] = useBrowserLocation();

  const { files, commonPrefixes, prefix } = filesResponse;

  const handleBreadcrumb = (folder: string) => {
    let breadcrumbs = prefix.split("/");
    breadcrumbs.pop();

    const position = breadcrumbs.indexOf(folder);

    breadcrumbs = breadcrumbs.slice(0, position + 1);

    const destination = breadcrumbs.join("/");

    navigate(`/files/${destination}/`);
  };

  useEffect(() => {
    fetch(`/api${location}`)
      .then((response) => response.json())
      .then(setFilesResponse);
  }, [location]);

  return (
    <Drawer>
      <main className="flex grow container mx-auto gap-3">
        <div className="overflow-x-auto grow">
          <div className="flex justify-between">
            <div className="breadcrumbs text-sm">
              <ul>
                <li>
                  <Link to="~/files/">Início</Link>
                </li>
                {prefix.split("/").map((folder) => (
                  <li>
                    <a onClick={() => handleBreadcrumb(folder)}>{folder}</a>
                  </li>
                ))}
              </ul>
            </div>
          </div>
          <table className="table">
            {/* head */}
            <thead>
              <tr>
                <th>Nome</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {commonPrefixes.map((folder) => (
                <tr>
                  <td>
                    <span
                      className="cursor-pointer"
                      onClick={() => navigate(`/files/${folder}`)}
                    >
                      <i className="fa-solid fa-box-archive me-3 hover:text-lg" />
                      {folder.replace(prefix, "").replace("/", "")}
                    </span>
                  </td>
                  <td></td>
                </tr>
              ))}
              {files.map((file) => (
                <tr>
                  <td>
                    <a
                      href={`/api/files/${encodeURI(file.key)}`}
                      download={file.key}
                    >
                      <i className="fa-regular fa-file me-3" />
                      {file.key.replace(prefix, "")}
                    </a>
                  </td>
                  <td></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div className="basis-1/4">
          <ul className="menu bg-base-200 rounded-box  w-full h-full">
            <li>
              <a>Item 1</a>
            </li>
            <li>
              <a>Parent</a>
              <ul>
                <li>
                  <a>Submenu 1</a>
                </li>
                <li>
                  <a>Submenu 2</a>
                </li>
                <li>
                  <a>Parent</a>
                  <ul>
                    <li>
                      <a>Submenu 1</a>
                    </li>
                    <li>
                      <a>Submenu 2</a>
                    </li>
                  </ul>
                </li>
              </ul>
            </li>
            <li>
              <a>Item 3</a>
            </li>
          </ul>
        </div>
      </main>
    </Drawer>
  );
}
