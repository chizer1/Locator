import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "./LoginButton";
import LogoutButton from "./LogoutButton";

function App() {
  const { user, isAuthenticated, getAccessTokenSilently } =
    useAuth0();

  const randomApiCall = async () => {
    var accessToken = await getAccessTokenSilently();

    const response = await fetch(
      "http://localhost:5022/getStuff?clientId=5005&databaseTypeId=22",
      {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      }
    );
    const responseData = await response.json();
    console.log(responseData);
  };

  return (
    <>
      {isAuthenticated ? (
        <div>
          <img src={user.picture} alt={user.name} />
          <h2>{user.name}</h2>
          <p>{user.email}</p>
          <button onClick={randomApiCall}>Random API Call</button>
          <LogoutButton />
        </div>
      ) : (
        <div>
          <h2>Log in to view profile</h2>
          <LoginButton />
        </div>
      )}
    </>
  );
}

export default App;
