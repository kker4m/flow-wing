import React from "react";
import Sidebar from "../components/Sidebar";
import Header from "../components/Header"

const Layout = ({ children }) => {
  return (
    <div style={{ display: "flex" }}>
      <div>
        <Sidebar />
      </div>
<Header/>
      <div style={{ flex: "1" }}>{children}</div>
    </div>
  );
};

export default Layout;
