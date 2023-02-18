import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import App from "./App";
import { Hello } from "./Hello";
import "./index.css";
import {Interop} from "./Interop";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: (
      <div>
        <h1>404</h1>
        <p>You know what it means</p>
      </div>
    ),
  },
  {
    path: "hello",
    element: <Hello />,
  },
  {
    path: "interop",
    element: <Interop />,
  },
]);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
