import React from "react";
import Sidebar from "../components/Sidebar";
import Header from "../components/Header";

const Layout = ({ children }) => {
  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
      <Header />

      <div style={{ display: "flex", flex: "1" }}>
        <Sidebar />
        <div style={{ flex: "1", overflowY: "auto" }}>{children}</div>
      </div>
    </div>
  );
};

export default Layout;

