import dayjs from "dayjs";
import React from "react";
import "./login.css";
import Header from "../../components/Header";
import { useNavigate } from "react-router";
import { Divider, Input } from "antd";
import { Link } from "react-router-dom";
import { TextField } from "@mui/material";
import { Formik, useFormik } from "formik";
import * as Yup from "yup";
import UserService from "../../services/userService";
import { useDispatch, useSelector } from "react-redux";
import { loginUser } from "../../Redux/authSlice";

const Login = () => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  // redux state
  const { loading, error } = useSelector((state) => state.user);
  // login handler function
  const handleLogin = (values) => {
    dispatch(loginUser(values)).then((result) => {
      if (result.payload) {
        navigate("/home");
      }
    });
  };
  const validationSchema = Yup.object({
    password: Yup.string()
      .required("Zorunlu alan")
      .min(4, "Şifre en az 4 karakter içermelidir")
      .max(8, "Şifre en fazla 8 karakterden oluşabilir."),
    email: Yup.string()
      .email("Geçersiz e-mail adresi")
      .required("Zorunlu alan"),
  });

  return (
    <>
      <Header />
      <div className="login-page-content">
        <p>Hoş Geldiniz</p>
        <Divider />
        <Formik
          initialValues={{
            password: "",
            email: "",
          }}
          validationSchema={validationSchema}
          onSubmit={(values) => {
            console.log(values);
            handleLogin(values);
          }}
        >
          {({ handleSubmit, handleChange, values, errors }) => (
            <form>
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

              {/* LINK TO REGISTER PAGE IF USER DOESN'T HAVE AN ACCOUNT */}
              <div className="register-link">
                Hesabınız yok mu? <Link to="/register">Kayıt Ol</Link>
              </div>

              {/* SUBMIT BUTTON */}

              <button className="sign-in-btn" onClick={handleSubmit}>
                {loading ? (
                  <span className="loading loading-dots loading-lg"></span>
                ) : (
                  "Giriş Yap"
                )}
              </button>
              {error && <div>{error}</div>}
            </form>
          )}
        </Formik>
      </div>
    </>
  );
};

export default Login;
