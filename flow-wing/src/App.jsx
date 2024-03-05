import { Navigate, Route, Routes } from "react-router"
import "./App.css"
import Login from "./pages/Login/Login"
import Home from "./pages/Home/Home"
import Register from "./pages/Register/Register"
import _Layout from "./layout/Layout"
import Sent from "./pages/Sent/Sent"
import Scheduled from "./pages/Scheduled/Scheduled"
import Compose from "./pages/Compose/Compose"
import Inbox from "./pages/Inbox/Inbox"
import Trash from "./pages/Trash/Trash"
import RequireAuth from "./components/RequireAuth"
import {
  COMPOSE_NEW_ROUTE,
  ERROR_ROUTE,
  HOME_ROUTE,
  INBOX_ROUTE,
  LOGIN_ROUTE,
  REGISTER_ROUTE,
  SENT_ROUTE,
  TRASH_ROUTE
} from "./routes"
import ErrorPage from "./pages/404/ErrorPage"

function App() {
  return (
    <Routes>
      <Route path={LOGIN_ROUTE} element={<Login />} />
      <Route
        path={HOME_ROUTE}
        element={
          <RequireAuth>
            {" "}
            <_Layout>
              <Home />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route
        path={SENT_ROUTE}
        element={
          <RequireAuth>
            <_Layout>
              <Sent />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route
        path="/scheduled"
        element={
          <RequireAuth>
            {" "}
            <_Layout>
              <Scheduled />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route
        path={COMPOSE_NEW_ROUTE}
        element={
          <RequireAuth>
            {" "}
            <_Layout>
              <Compose />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route
        path={INBOX_ROUTE}
        element={
          <RequireAuth>
            {" "}
            <_Layout>
              <Inbox />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route
        path={TRASH_ROUTE}
        element={
          <RequireAuth>
            {" "}
            <_Layout>
              <Trash />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route path={REGISTER_ROUTE} element={<Register />} />
      <Route path="/" element={<Navigate to="/login" />} />
      <Route path={ERROR_ROUTE} element={<ErrorPage />} />
    </Routes>
  )
}

export default App
