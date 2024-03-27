import React, { useEffect, useState } from "react"
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons"
import { Avatar, Button, Divider, Dropdown, Menu } from "antd"
import { Icon } from "@iconify/react"
import { Link, useLocation, useNavigate } from "react-router-dom"
import {
  COMPOSE_NEW_ROUTE,
  HOME_ROUTE,
  SENT_ROUTE,
  TRASH_ROUTE
} from "../routes"
import { useDispatch, useSelector } from "react-redux"
import { logoutUser } from "../Redux/authSlice"
import "./sidebar.css"

function getItem(label, key, icon, children, type) {
  return {
    key,
    icon,
    children,
    label,
    type
  }
}
const items = [
  getItem(
    <Link to={HOME_ROUTE}>Gelen Kutusu</Link>,
    "1",
    <Icon icon="quill:mail" />
  ),
  getItem(
    <Link to={SENT_ROUTE}>Gönderilenler</Link>,
    "2",
    <Icon icon="icon-park-outline:message-sent" />
  ),

  getItem(
    <Link to={COMPOSE_NEW_ROUTE}>Mail Oluştur</Link>,
    "3",
    <Icon icon="streamline:chat-bubble-square-write" />
  ),
  getItem(
    <Link to={TRASH_ROUTE}>Çöp Kutusu</Link>,
    "4",
    <Icon icon="bi:trash" />
  )
]
const Sidebar = () => {
  const [open, setOpen] = useState(false)

  // Use the useDispatch hook to get the dispatch function
  const dispatch = useDispatch()
  const navigate = useNavigate()
  // Logout function
  const handleLogout = () => {
    dispatch(logoutUser())
    navigate("/login")
  }
  // Use the useSelector hook to get the user from the Redux store
  const user = useSelector((state) => state.user.user)

  const location = useLocation()

  useEffect(() => {
    const path = location.pathname
    // Burada path'e göre seçili anahtarı ayarlayabilirsiniz.
  }, [location])

  const setStoredSelectedKey = (key) => {
    localStorage.setItem("selectedKey", key)
  }

  useEffect(() => {
    const path = location.pathname
    const storedSelectedKey = localStorage.getItem("selectedKey")
    const defaultSelectedKey = "1" // Default selected key

    // Use the stored key if it exists, otherwise use the default
    const selectedKey = storedSelectedKey
      ? storedSelectedKey
      : defaultSelectedKey

    setStoredSelectedKey(selectedKey) // Store the selected key
  }, [location])
  return (
    <div
      style={{
        width: 256
      }}
    >
      <div className="user-section-sidebar">
        <div className="user-icon-display" onClick={() => setOpen(!open)}>
          {" "}
          <Avatar
            size={64}
            style={{ backgroundColor: "#191970 ", color: "#add8e6 " }}
          >
            <div className="user-name-display">
              {user ? <>{user.username.charAt(0)}</> : <div>Kullanıcı yok</div>}
            </div>
          </Avatar>
          {open === true ? (
            <div>
              <button className="logout-btn-sidebar" onClick={handleLogout}>
                Çıkış Yap
              </button>
            </div>
          ) : null}
        </div>

        <Divider />
      </div>
      <Menu
        defaultSelectedKeys={["1"]}
        defaultOpenKeys={["sub1"]}
        mode="inline"
        items={items}
        selectedKeys={[localStorage.getItem("selectedKey") || "1"]}
        onSelect={({ key }) => setStoredSelectedKey(key)}
        style={{ fontSize: "16px", width: 256 }}
      />
    </div>
  )
}
export default Sidebar
