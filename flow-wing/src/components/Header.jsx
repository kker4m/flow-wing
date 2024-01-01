import React, { useState } from "react";
import "./header.css";
import { Input } from "antd";
import { Icon } from "@iconify/react";

const getUser = () => {
  let user = localStorage.getItem("user");
  if (user) {
    user = JSON.parse(user);
    console.log(user);
  } else {
    user = null;
  }
  return user;
};

// search functions
const { Search } = Input;

const onSearch = (value, _e, info) => console.log(info?.source, value);

const Header = () => {
  const [user, setUser] = useState(getUser());

  return (
    <div className="header-content">
      <div className="logo-section">
        <img
          className="logo-img"
          src="https://res.cloudinary.com/dirtkkfqn/image/upload/v1703573682/arcelik_logo_lztrqj.png"
          alt="Logo"
        />
      </div>

      <div className="search-section">
        <Search
          placeholder="Postalarda arayın"
          onSearch={onSearch}
          style={{
            width: 350,
          }}
        />
      </div>
      <div className="user-section">
        <div className="user-icon">
          <Icon icon="ph:user-light" width="30" />{" "}
        </div>
        {user ? (
          <div  className="user-name">{user.fullname} Tuğçe Özelmaci</div>
        ) : (
          <div>Kullanıcı yok</div>
        )}
      </div>
    </div>
  );
};

export default Header;
