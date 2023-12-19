import React from "react";
import Sidebar from "../components/Sidebar";

const Layout = ({ children }) => {
  return (
    <div style={{ display: "flex" }}>
      <div>
        <Sidebar />
      </div>

      <div style={{ flex: "1" }}>{children}</div>
    </div>
  );
};

export default Layout;
