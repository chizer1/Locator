import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import { Auth0Provider } from "@auth0/auth0-react";

createRoot(document.getElementById("root")).render(
  <Auth0Provider
    domain="locatorlibrary.us.auth0.com" // tenant / domain
    clientId="11Ufs1SfbMHqaNg1Pxm7Tdc1eQV7QciD" // spa app that it is connecting
    authorizationParams={{
      audience: "API", // custom api we want to connect to that has permissions available
      scope: "openid profile email admin:read", // add permissions that are expected to be used in the app
      redirect_uri: window.location.origin,
    }}
  >
    <App />
  </Auth0Provider>
);
 