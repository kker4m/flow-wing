import { Divider } from "antd"
import React from "react"
import { useNavigate } from "react-router"
import { Link } from "react-router-dom"
import "./register.css"
import { TextField } from "@mui/material"
import { Formik } from "formik"
import * as Yup from "yup"
import { useDispatch, useSelector } from "react-redux"
import alertify from "alertifyjs"
import { registerUser } from "../../Redux/authSlice"

const Register = () => {
  let navigate = useNavigate()
  const dispatch = useDispatch()

  // redux state
  const { loading, error } = useSelector((state) => state.user)

  // REGISTER FUNCTION
  const handleRegister = (values) => {
    dispatch(registerUser(values)).then((result) => {
      if (result.payload) {
        alertify.success("Kayıt oluşturuldu")
        navigate("/login")
      }
    })
  }
  const validationSchema = Yup.object({
    username: Yup.string()
      .required("Zorunlu alan")
      .min(4, "Kullanıcı adı en az 4 karakterden oluşmalıdır.")
      .max(8, "Kullanıcı adı en fazla 8 karakterden oluşabilir."),
    password: Yup.string()
      .required("Zorunlu alan")
      .min(4, "Şifre en az 4 karakter içermelidir")
      .max(8, "Şifre en fazla 8 karakterden oluşabilir."),
    email: Yup.string()
      .email("Geçersiz e-mail adresi")
      .required("Zorunlu alan")
      .test(
        "arcelikEmail",
        "Yalnızca Arçelik maillerinizle kayıt yapabilirsiniz",
        function (value) {
          // E-posta adresinin @arcelik.com ile bitip bitmediğini kontrol et
          if (value && value.endsWith("@arcelik.com")) {
            return true // Geçerli
          }
          return false // Geçersiz
        }
      )
  })

  return (
    <div className="register-page-content">
      <div className="register-page-form">
        <p>Hoş Geldiniz</p>
        <Divider />
        <Formik
          initialValues={{
            username: "",
            password: "",
            email: ""
          }}
          validationSchema={validationSchema}
          onSubmit={(values) => {
            handleRegister(values)
          }}
        >
          {({ handleSubmit, handleChange, values, errors }) => (
            <form onSubmit={handleSubmit}>
              <div className="input-areas">
                <TextField
                  name="username"
                  id="standard-basic"
                  label="Kullanıcı Adı"
                  variant="standard"
                  onChange={handleChange}
                  value={values.username}
                />
              </div>
              {errors.username && (
                <div className="error-message">{errors.username}</div>
              )}
              <div className="input-areas">
                <TextField
                  id="standard-basic"
                  name="email"
                  label="Mail "
                  variant="standard"
                  onChange={handleChange}
                  value={values.email}
                />
              </div>
              {errors.email && (
                <div className="error-message">{errors.email}</div>
              )}
              <div className="input-areas">
                <TextField
                  id="standard-basic"
                  name="password"
                  label="Şifre"
                  variant="standard"
                  type="password"
                  onChange={handleChange}
                  value={values.password}
                />
              </div>
              {errors.password && (
                <div className="error-message">{errors.password}</div>
              )}
              <div className="register-link">
                Hesabınız var mı? <Link to="/login">Giriş Yap</Link>
              </div>

              <button className="register-btn" type="submit">
                Hesap Oluştur
              </button>
            </form>
          )}
        </Formik>
      </div>
    </div>
  )
}

export default Register
