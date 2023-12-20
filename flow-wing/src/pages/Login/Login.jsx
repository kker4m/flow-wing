import dayjs from "dayjs";
import React from "react";
import "./login.css";
import Header from "../../components/Header";
import { useNavigate } from "react-router";
import { Divider, Input } from "antd";
import { Link } from "react-router-dom";

const Login = () => {
let navigate = useNavigate()

// login handler function
const handleLogin=()=>{
  navigate("/home")
}

  return (
    <>
    <Header/>
        <div className="login-page-content">
   
        <p>Hoş Geldiniz</p>
        <Divider/>
      <div className="input-areas">
        {" "}
        <label for="username">Kullanıcı Adı</label>
        <Input type="text" id="username" name="username" required />
       
      </div>

      <div className="input-areas">
        {" "}
        <label for="email">Mail</label>
        <Input type="text" id="email" name="email" required  />
      </div>

      <div className="input-areas">
        {" "}
        <label for="password">Şifre</label>
        <Input.Password className="input-password" required  />
      </div>
      <div>
  Hesabınız yok mu? <Link to="/register">Kayıt Ol</Link>
</div>
      <button className="sign-in-btn" onClick={handleLogin}>Giriş Yap</button>
    </div>
    </>
  
  );
};

export default Login;
