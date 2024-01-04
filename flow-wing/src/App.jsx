import { Navigate, Route, Router, Routes } from "react-router";
import "./App.css";
import Login from "./pages/Login/Login";
import Home from "./pages/Home/Home";
import Register from "./pages/Register/Register";
import _Layout from "./layout/Layout";
import Sent from "./pages/Sent/Sent";
import Scheduled from "./pages/Scheduled/Scheduled";
import Compose from "./pages/Compose/Compose";
import Inbox from "./pages/Inbox/Inbox";
import Trash from "./pages/Trash/Trash";
import RequireAuth from "./components/RequireAuth";

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route
        path="/home"
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
        path="/sent"
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
        path="/compose-new"
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
        path="/inbox/:index"
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
        path="/trash"
        element={
          <RequireAuth>
            {" "}
            <_Layout>
              <Trash />{" "}
            </_Layout>
          </RequireAuth>
        }
      />
      <Route path="/register" element={<Register />} />
      <Route path="/" element={<Navigate to="/login" />} />
    </Routes>
  );
}

export default App;
