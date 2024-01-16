import axios from "axios";

export default class UserService {
  // createUser(values) {
  //   const { email, username, password } = values;

  //   // Convert values to strings if necessary
  //   const userData = {
  //     Email: String(email),
  //     Password: String(password),
  //     Username: String(username),
  //   };

  //   return axios.post(process.env.REACT_APP_API_URL+"api/Auth/signup", userData, {
  //     headers: {
  //       "Content-Type": "application/json",
  //     },
  //     mode: "cors",
  //   });
  // }

  // login(values) {
  //   const { email, password } = values;

  //   // Convert values to strings if necessary
  //   const userData = {
  //     Email: String(email),
  //     Password: String(password),
  //   };
  //   return axios.post(process.env.REACT_APP_API_URL+"api/Auth/login", userData, {
  //     headers: {
  //       "Content-Type": "application/json",
  //     },
  //     mode: "cors",
  //   });
  // }

  getUsers(){
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;
    return axios.get("http://localhost:5232/api/Users", {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }
}
