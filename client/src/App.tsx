import { Route, Switch } from "wouter";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import Toast from "./components/Toast";

import Confirm from "./routes/Confirm";
import ForgotPassword from "./routes/ForgotPassword";
import Home from "./routes/Home";
import Login from "./routes/Login";
import ManageAccount from "./routes/ManageAccount";
import Register from "./routes/Register";
import ResetPassword from "./routes/ResetPassword";

import JustForPublic from "./components/JustForPublic";
import MustBeLoggedIn from "./components/MustBeLoggedIn";

const App = () => {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <Toast />
      <Switch>
        <JustForPublic path="/login" component={Login} />
        <JustForPublic path="/register" component={Register} />
        <JustForPublic path="/confirmemail" component={Confirm} />
        <JustForPublic path="/forgotpassword" component={ForgotPassword} />
        <JustForPublic path="/resetpassword" component={ResetPassword} />
        <MustBeLoggedIn path="/" component={Home} />
        <MustBeLoggedIn path="/manageaccount" component={ManageAccount} />
        <Route>404: No such page!</Route>
      </Switch>
    </QueryClientProvider>
  );
};

export default App;
