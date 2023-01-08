import { FC } from "react";
import { Link } from "react-router-dom";

export const Hello: FC = () => {
  return (
    <div>
      <h1>Hello, World!</h1>
      <Link to="/">Back to home</Link>
    </div>
  );
};
