import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import { Auth0Provider } from "@auth0/auth0-react";

createRoot(document.getElementById("root")).render(
  <Auth0Provider
    domain="dev-xshhwrh4f1vis6lb.us.auth0.com"
    clientId="RCbDL6LnErLJfuXz1s3hvLf6bVJklNFl"
    authorizationParams={{
      audience: "consultifi",
      scope: "openid profile email admin:read admin:write",
      redirect_uri: window.location.origin,
    }}
  >
    <App />
  </Auth0Provider>
);
 