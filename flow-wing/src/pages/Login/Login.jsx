import dayjs from "dayjs";
import React from "react";

const Login = () => {


  return (
    <div>
      <label for="email">Mail</label>
      <input type="text" id="email" name="email" value="" />

      <label for="password">Şifre</label>
      <input type="password" id="password" name="password" value="" />
    </div>
  );
};

export default Login;
