import React, { useEffect, useState } from "react";
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons";
import { Button, Menu } from "antd";
import { Icon } from "@iconify/react";
import { Link, useLocation } from "react-router-dom";
function getItem(label, key, icon, children, type) {
  return {
    key,
    icon,
    children,
    label,
    type,
  };
}
const items = [
  getItem(
    <Link to="/home">Gelen Kutusu</Link>,
    "1",
    <Icon icon="quill:mail" />
  ),
  getItem(
    <Link to="/sent">Gönderilenler</Link>,
    "2",
    <Icon icon="icon-park-outline:message-sent" />
  ),
  getItem(
    <Link to="/scheduled">Planlanmış Mailler</Link>,
    "3",
    <Icon icon="ic:sharp-schedule" />
  ),
  getItem(
    <Link to="/compose-new">Mail Oluştur</Link>,
    "4",
    <Icon icon="streamline:chat-bubble-square-write" />
  ),
];
const Sidebar = () => {
  const location = useLocation();

  useEffect(() => {
    const path = location.pathname;
    // Burada path'e göre seçili anahtarı ayarlayabilirsiniz.
  }, [location]);

  const setStoredSelectedKey = (key) => {
    localStorage.setItem("selectedKey", key);
  };

  useEffect(() => {
    const path = location.pathname;
    const storedSelectedKey = localStorage.getItem("selectedKey");
    const defaultSelectedKey = "1"; // Default selected key

    // Use the stored key if it exists, otherwise use the default
    const selectedKey = storedSelectedKey
      ? storedSelectedKey
      : defaultSelectedKey;

    setStoredSelectedKey(selectedKey); // Store the selected key
  }, [location]);
  return (
    <div
      style={{
        width: 256,
      }}
    >
      <Menu
        defaultSelectedKeys={["1"]}
        defaultOpenKeys={["sub1"]}
        mode="inline"
        items={items}
        selectedKeys={[localStorage.getItem("selectedKey") || "1"]}
        onSelect={({ key }) => setStoredSelectedKey(key)}
      />
    </div>
  );
};
export default Sidebar;
