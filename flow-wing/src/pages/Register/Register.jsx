import { Divider, Input } from "antd";
import React from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";
import "./register.css";
import { TextField } from "@mui/material";
import { Formik, useFormik } from "formik";
import * as Yup from "yup";

const Register = () => {
  let navigate = useNavigate();

  // register handler function
  // const handleRegister = () => {
  //   navigate("/login");
  // };

  // YUP VALIDATION SCHEMA
  const validationSchema = Yup.object({
    userName: Yup.string()
      .required("Zorunlu alan")
      .min(4, "Kullanıcı adı en az 4 karakterden oluşmalıdır.")
      .max(8, "Kullanıcı adı en fazla 8 karakterden oluşabilir."),
    password: Yup.string()
      .required("Zorunlu alan")
      .min(4, "Şifre en az 4 karakter içermelidir")
      .max(8, "Şifre en fazla 8 karakterden oluşabilir."),
    email: Yup.string()
      .email("Geçersiz e-mail adresi")
      .required("Zorunlu alan"),
    passwordConfirmation: Yup.string()
      .oneOf([Yup.ref("password"), null], "Şifreler eşleşmiyor.")
      .required("Zorunlu alan"),
  });

  return (
    <>
      <div className="register-page-content">
        <p>Hoş Geldiniz</p>
        <Divider />
        <Formik
          initialValues={{
            userName: "",
            password: "",
            email: "",
            passwordConfirmation: "",
          }}
          validationSchema={validationSchema}
          onSubmit={(values) => {
            console.log(values);
            navigate("/login");
          }}
        >
          {({ handleSubmit, handleChange, values, errors }) => (
            <form>
              <div className="input-areas">
                {" "}
                <TextField
                  name="userName"
                  id="standard-basic"
                  label="Kulanıcı Adı"
                  variant="standard"
                  onChange={handleChange}
                  value={values.userName}
                />
              </div>
              {errors.userName && errors.userName}
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

              {errors.email ? errors.email : null}
            
              <div className="input-areas">
                {" "}
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
              {errors.password && errors.password}
              <div className="input-areas">
                {" "}
                <TextField
                  id="standard-basic"
                  label="Şifre Tekrar"
                  variant="standard"
                  type="password"
                  name="passwordConfirmation"
                  onChange={handleChange}
                  value={values.passwordConfirmation}
                />
              </div>
              {errors.passwordConfirmation && errors.passwordConfirmation}
              {/* LINK TO LOGIN PAGE IF USER HAS AN ACCOUNT */}
              <div className="register-link">
                Hesabınız var mı? <Link to="/login">Giriş Yap</Link>
              </div>

              {/* SUBMIT BUTTON */}

              <button className="register-btn" onClick={handleSubmit}>
                Hesap Oluştur
              </button>
            </form>
          )}
        </Formik>
      </div>
    </>
  );
};

export default Register;
