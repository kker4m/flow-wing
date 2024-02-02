import React, { memo } from "react";
import "./header.css";
import { Dropdown, Input } from "antd";
import { Icon } from "@iconify/react";
import { useDispatch, useSelector } from "react-redux";
import { Divider } from "@mui/material";
import { Link, useNavigate } from "react-router-dom";
import { logoutUser } from "../Redux/authSlice";

// search functions
const { Search } = Input;

const Header = ({ search }) => {
  // Use the useDispatch hook to get the dispatch function
  const dispatch = useDispatch();
  const navigate = useNavigate();
  // Logout function
  const handleLogout = () => {
    dispatch(logoutUser());
    navigate("/login");
  };
  // Use the useSelector hook to get the user from the Redux store
  const user = useSelector((state) => state.user.user);

  //USER MENU DROPDOWN
  const handleMenuClick = () => {
    handleLogout();
  };

  const items = [
    {
      label: "  Çıkış Yap",
      key: "1",
      icon: <Icon icon="material-symbols-light:logout-sharp" width="26" height="26"  style={{color: "black"}} />,
    },
  ];

  const menuProps = {
    items,
    onClick: (e) => handleMenuClick( e),
  };

  return (
    <div className="header-content">
      <div className="logo-section">
        <Link to="/home">
          {" "}
          <img
            className="logo-img"
            src="https://res.cloudinary.com/dirtkkfqn/image/upload/v1703573682/arcelik_logo_lztrqj.png"
            alt="Logo"
          />
        </Link>
      </div>
      
      <div className="search-section">
        <Search
          placeholder="Postalarda arayın"
          onChange={(e) => {
              const value = e.target.value;
              setSearchInput(value);
              search();
            }}
          style={{
            width: 350,
            height:60
          }}
        />
      </div>
      <div className="user-section">
        <Dropdown.Button
          menu={menuProps}
          placement="bottom"
          icon={<Icon icon="ph:user-light" width="30" />}
        >
          {user ? (
            <div className="user-name">{user.username} </div>
          ) : (
            <div>Kullanıcı yok</div>
          )}
        </Dropdown.Button>
      </div>
      <Divider />
    </div>
  );
};

export default memo(Header);
