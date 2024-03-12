import React, { memo } from "react"
import "./header.css"
import { Dropdown, Input } from "antd"
import { Icon } from "@iconify/react"
import { useDispatch, useSelector } from "react-redux"
import { Divider } from "@mui/material"
import { Link, useNavigate } from "react-router-dom"
import { logoutUser } from "../Redux/authSlice"

// search functions
const { Search } = Input

const Header = ({ search }) => {
  return (
    <div className="header-content">
      <div className="search-section">
        <Search
          placeholder="Postalarda arayın"
          onChange={(e) => {
            const value = e.target.value
            setSearchInput(value)
            search()
          }}
          style={{
            width: 350,
            height: 60
          }}
        />
      </div>

      <Divider />
    </div>
  )
}

export default memo(Header)
