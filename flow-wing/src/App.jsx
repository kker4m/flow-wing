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

function App() {
  return (
   
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/home"
          element={
            <_Layout>
              <Home />{" "}
            </_Layout>
          }
        />
          <Route
          path="/sent"
          element={
            <_Layout>
              <Sent />{" "}
            </_Layout>
          }
        />
          <Route
          path="/scheduled"
          element={
            <_Layout>
              <Scheduled />{" "}
            </_Layout>
          }
        />
         <Route
          path="/compose-new"
          element={
            <_Layout>
              <Compose />{" "}
            </_Layout>
          }
        />
         <Route
         path="/inbox/:index"
          element={
            <_Layout>
              <Inbox />{" "}
            </_Layout>
          }
        />
           <Route
         path="/trash"
          element={
            <_Layout>
              <Trash />{" "}
            </_Layout>
          }
        />
        <Route path="/register" element={<Register />} />
        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
   
  );
}

export default App;
