import React from "react";
import "./header.css";
import { Input } from "antd";
import { Icon } from "@iconify/react";
import { useSelector } from "react-redux";
import { Divider } from "@mui/material";
import { Link } from "react-router-dom";

// search functions
const { Search } = Input;

const onSearch = (value, _e, info) => console.log(info?.source, value);

const Header = () => {
  // Use the useSelector hook to get the user from the Redux store
  const user = useSelector((state) => state.user.user);

  return (
    <div className="header-content">
      <div className="logo-section">
        <img
          className="logo-img"
          src="https://res.cloudinary.com/dirtkkfqn/image/upload/v1703573682/arcelik_logo_lztrqj.png"
          alt="Logo"
        />
      </div>

      <div className="go-to-home-btn">
        <Link to="/home">
          {" "}
          <button className="home-btn">
            <Icon icon="teenyicons:home-outline" width="30" />
          </button>
        </Link>
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
          <div className="user-name">{user.username} </div>
        ) : (
          <div>Kullanıcı yok</div>
        )}
      </div>
      <Divider />
    </div>
  );
};

export default Header;
