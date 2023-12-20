import { Divider, Input } from 'antd'
import React from 'react'
import { useNavigate } from 'react-router'
import { Link } from 'react-router-dom'
import "./register.css"

const Register = () => {

  let navigate = useNavigate()

// register handler function
const handleRegister=()=>{
  navigate("/login")
}
  return (
    <>
  
        <div className="register-page-content">
   
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

      <div className="input-areas">
        {" "}
        <label for="password">Şifre Tekrar</label>
        <Input.Password className="input-password" required  />
      </div>
<div>
  Hesabınız var mı? <Link to="/login">Giriş Yap</Link>
</div>
      <button className="register-btn" onClick={handleRegister}>Hesap Oluştur</button>
    </div>
    </>
  )
}

export default Register