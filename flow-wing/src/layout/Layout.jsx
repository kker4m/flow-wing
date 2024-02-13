import React from "react"
import Sidebar from "../components/Sidebar"
import Header from "../components/Header"

const Layout = ({ children }) => {
  const onSearch = (value, _e, info) => console.log(info?.source, value)
  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
      <Header onSearch={onSearch} />

      <div style={{ display: "flex", flex: "1" }}>
        <Sidebar />
        <div style={{ flex: "1", overflowY: "auto" }}>{children}</div>
      </div>
    </div>
  )
}

export default Layout
